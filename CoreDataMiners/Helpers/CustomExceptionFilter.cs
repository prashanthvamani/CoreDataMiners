using DataminersDAL.Repositories;
using DataminersModel;
using DataminersModel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace DataMinersWeb.Helpers
{
    public class CustomExceptionFilter : Attribute, IExceptionFilter
    {
        private readonly ErrorLogRepository _errorLog;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;
        public CustomExceptionFilter(ErrorLogRepository errorLog,IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            _errorLog = errorLog;
            _httpContextAccessor = httpContextAccessor;
            _env = env;
        }
        public void OnException(ExceptionContext context)
        {
            // Use IHttpContextAccessor to access HttpContext
            string userId = _httpContextAccessor.HttpContext?.Session.GetString("userid") ?? "Unknown";
            //throw new NotImplementedException();

            // Capture additional information
            var request = _httpContextAccessor.HttpContext.Request;
            var additionalInfo = new Dictionary<string, string>
            {
                { "UserAgent", request.Headers["User-Agent"].ToString() },
                { "ContentType", request.ContentType },
                { "Method", request.Method },
                // Example of capturing query parameters (be mindful of sensitive data)
                { "QueryString", request.QueryString.ToString() }
            };

            // Serialize the additional info to JSON
            string additionalInfoJson = JsonSerializer.Serialize(additionalInfo);


            var errorlog = new ErrorLog
            {
                ErrorMessage = context.Exception.Message,
                StackTrace = context.Exception.StackTrace,
                ControllerName = context.RouteData.Values["controller"]?.ToString(),
                ActionName = context.RouteData.Values["action"]?.ToString(),
                UserName = userId,
                AdditionalInfo = additionalInfoJson // Add more info if needed
            };

            _errorLog.SaveLogError(
                errorlog.ErrorMessage,
                errorlog.StackTrace,
                errorlog.ControllerName,
                errorlog.ActionName,
                errorlog.UserName,
                errorlog.AdditionalInfo
                );

            //Handle UnauthorizedAccessException(missing/ invalid token)
            if (context.Exception is UnauthorizedAccessException)
            {
                // Redirect to login page for browser requests
                if (IsApiRequest(request))
                {
                    context.Result = new JsonResult(new { message = "Unauthorized. Please login again." })
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                }
                else
                {
                    context.Result = new RedirectResult("/LoginNew/Login");
                }

                context.ExceptionHandled = true;
                return;
            }

            // Handle SQL exceptions with a friendly message and status 503
            if (context.Exception.GetType().Name == "SqlException") // use type name to avoid adding dependency
            {
                if (IsApiRequest(request))
                {
                    context.Result = new JsonResult(new { message = "Database error occurred. Please try again later." })
                    {
                        StatusCode = StatusCodes.Status503ServiceUnavailable
                    };
                }
                else
                {
                    // For web requests, show error page with friendly message
                    var errorViewModel = CreateErrorViewModel(context, "Database error occurred. Please try again later.");
                    context.Result = new ViewResult
                    {
                        ViewName = "Error",
                        ViewData = new ViewDataDictionary<ErrorViewModel>(
                            new EmptyModelMetadataProvider(), context.ModelState)
                        {
                            Model = errorViewModel
                        }
                    };
                }

                context.ExceptionHandled = true;
                return;
            }

            // Default error handling (render error view)
            var defaultErrorViewModel = CreateErrorViewModel(context);

            context.Result = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary<ErrorViewModel>(
                   new EmptyModelMetadataProvider(), context.ModelState)
                {
                    Model = defaultErrorViewModel
                }
            };
            context.ExceptionHandled = true;
        }

        private ErrorViewModel CreateErrorViewModel(ExceptionContext context, string customMessage = null)
        {
            return new ErrorViewModel
            {
                Message = customMessage ?? context.Exception.Message,
                Type = context.Exception.GetType().Name,
                Source = context.Exception.Source,
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                PageUrl = context.HttpContext.Request.Path,
                InnerException = context.Exception.InnerException?.Message,
                ShowAdminDetails = _env.IsDevelopment() || _env.EnvironmentName == "UAT"
            };
        }

        private bool IsApiRequest(HttpRequest request)
        {
            // Basic check to determine if the request expects JSON (API)
            return request.Path.StartsWithSegments("/api") ||
                   request.Headers["Accept"].Any(a => a.Contains("application/json"));
        }
    }
}
