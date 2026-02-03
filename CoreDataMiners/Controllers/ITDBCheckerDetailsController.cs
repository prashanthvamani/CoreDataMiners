using DataminersDAL.Repositories;
using DataminersModel.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DataMinersWeb.Controllers
{
    [Route("[controller]")]
    public class ITDBCheckerDetailsController : Controller
    {
        private readonly ITDBCheckerRepository _repository;
        public ITDBCheckerDetailsController(ITDBCheckerRepository repository)
        {
            _repository = repository;
        }

        
        public IActionResult Index()
        {
            var model = new ITDBCloseViewModel();
            return View(model);
        }

        [HttpGet("GetRequests")]
        public JsonResult GetRequests(string search = "", int page = 1, int pageSize = 10)
        {
            try
            {
                //var empid = HttpContext.Session.GetString("empid");
                //var userID = HttpContext.Session.GetString("userid");

                var model = new ITDBCloseViewModel
                {
                    RequestDetails = _repository.RequestDetailsOpenWIPs("BFIL\\prashanth.vamani")
                };

                var list = model.RequestDetails.AsQueryable();

                // If search term provided, filter the list (case-insensitive, contains)
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.ToLower();
                    list = list.Where(r =>
                        (r.RequestID != null && r.RequestID.ToLower().Contains(search)) ||
                        (r.RequestSource != null && r.RequestSource.ToLower().Contains(search)) ||
                        (r.CustomerType != null && r.CustomerType.ToLower().Contains(search)) ||
                        (r.Requestor != null && r.Requestor.ToLower().Contains(search)) ||
                        (r.FinalStatus != null && r.FinalStatus.ToLower().Contains(search))
                    );
                }

                int totalRecords = list.Count();

                var reportPage = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return Json(new { totalRecords, data = reportPage });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Index(string RequestID)
        {

            var model = new ITDBCloseViewModel();

            if(!string.IsNullOrWhiteSpace(RequestID))
            {
                model.RequestIDSearch = _repository.requestsearchViewModels(RequestID);
            }
            else
            {
                model.RequestIDSearch = new List<RequestsearchViewModel>();
            }

            return View(model);
        }


    }
}
