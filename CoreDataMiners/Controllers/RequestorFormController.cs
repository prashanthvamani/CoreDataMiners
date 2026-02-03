using DataminersDAL.Repositories;
using DataminersModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Reflection;

namespace DataMinersWeb.Controllers
{
    public class RequestorFormController : Controller
    {
        private readonly UserCredentialsRepository _userCredentialsRepository;
        private readonly RequestFormRepository _requestFormRepository;

        public RequestorFormController(UserCredentialsRepository userCredentialsRepository,RequestFormRepository formRepository)
        {
            _userCredentialsRepository = userCredentialsRepository;
            _requestFormRepository = formRepository;

        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var empid = HttpContext.Session.GetString("empid");
                var userID = HttpContext.Session.GetString("userid");
                DataSet ds = _userCredentialsRepository.GetUserCredentialDetails(empid);

                ViewData["empid"] = ds.Tables[0].Rows[0]["EMP_STAFFID"].ToString();
                ViewData["DM_DEPT_NAME"] = ds.Tables[0].Rows[0]["DM_DEPT_NAME"].ToString();
                ViewData["LOCATION_NAME"] = ds.Tables[0].Rows[0]["LOCATION_NAME"].ToString();
                ViewData["DESIGNATION_NAME"] = ds.Tables[0].Rows[0]["DESIGNATION_NAME"].ToString();
                ViewData["EMP_MAILID"] = ds.Tables[0].Rows[0]["EMP_MAILID"].ToString();
                ViewData["EMP_MOBILE_NO"] = ds.Tables[0].Rows[0]["EMP_MOBILE_NO"].ToString();

                
                

                var open = _userCredentialsRepository.OpenCount();
                var wip = _userCredentialsRepository.WIPCount();
                var close = _userCredentialsRepository.CloseCount();
                var reject = _userCredentialsRepository.RejectCount();
                var selfcount = _userCredentialsRepository.SelfCount(userID);

                ViewData["OpenCount"] = open;
                ViewData["WIPCount"] = wip;
                ViewData["CloseCount"] = close;
                ViewData["RejectCount"] = reject;
                ViewData["SelfCount"] = selfcount;


                return View();
            }
            catch(Exception ex) 
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult Index(CustomerReqModel.customerReq customerReq,IFormFile fileToUpload)
        {
            try
            {
                // Fetch user/session details and set ViewData
                var empid = HttpContext.Session.GetString("empid");
                var userID = HttpContext.Session.GetString("userid");
                DataSet ds = _userCredentialsRepository.GetUserCredentialDetails(empid);

                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    ModelState.AddModelError("", "User credential details not found.");
                    return View(customerReq);
                }

                ViewData["empid"] = ds.Tables[0].Rows[0]["EMP_STAFFID"].ToString();
                ViewData["DM_DEPT_NAME"] = ds.Tables[0].Rows[0]["DM_DEPT_NAME"].ToString();
                ViewData["LOCATION_NAME"] = ds.Tables[0].Rows[0]["LOCATION_NAME"].ToString();
                ViewData["DESIGNATION_NAME"] = ds.Tables[0].Rows[0]["DESIGNATION_NAME"].ToString();
                ViewData["EMP_MAILID"] = ds.Tables[0].Rows[0]["EMP_MAILID"].ToString();
                ViewData["EMP_MOBILE_NO"] = ds.Tables[0].Rows[0]["EMP_MOBILE_NO"].ToString();

                // Dashboard counters
                ViewData["OpenCount"] = _userCredentialsRepository.OpenCount();
                ViewData["WIPCount"] = _userCredentialsRepository.WIPCount();
                ViewData["CloseCount"] = _userCredentialsRepository.CloseCount();
                ViewData["RejectCount"] = _userCredentialsRepository.RejectCount();
                ViewData["SelfCount"] = _userCredentialsRepository.SelfCount(userID);


                // Set details in customerReq before insert

                customerReq.empid = ViewData["empid"].ToString();
                customerReq.UserID = userID;
                customerReq.department = ViewData["DM_DEPT_NAME"].ToString();
                customerReq.customertype = ViewData["LOCATION_NAME"].ToString();
                customerReq.customerDesignation = ViewData["DESIGNATION_NAME"].ToString();
                customerReq.UserEMailID = ViewData["EMP_MAILID"].ToString();
                customerReq.mobilenumber = ViewData["EMP_MOBILE_NO"].ToString();
                customerReq.Contactnumber = ViewData["EMP_MOBILE_NO"].ToString();
                customerReq.InsertDate = DateTime.Now;

                // File upload validation
                if (fileToUpload != null && fileToUpload.Length > 0)
                {
                    var allowedExtensions = new[] { ".xls", ".xlsx" };
                    var extension = Path.GetExtension(fileToUpload.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("FileUpload", "Invalid file type. Only Excel files (.xls, .xlsx) are allowed.");
                        return View(customerReq);
                    }

                    if (fileToUpload.Length > 5 * 1024 * 1024) // 5MB max size
                    {
                        ModelState.AddModelError("FileUpload", "File size exceeds 5 MB limit.");
                        return View(customerReq);
                    }

                    // Save file
                    var fileName = Path.GetFileName(fileToUpload.FileName);
                    var timeStamp = DateTime.Now.ToString("MM-dd-yy_hh_mm_ss");
                    var savedFileName = $"{Path.GetFileNameWithoutExtension(fileName)}-{timeStamp}{Path.GetExtension(fileName)}";

                    var savePath = Path.Combine(@"\\dcprfs\dm\", savedFileName);

                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        fileToUpload.CopyTo(stream);
                        //customerReq.FileUpload.CopyTo(stream);
                    }
                    customerReq.FileUpload = savePath;
                }
                else
                {
                    ModelState.AddModelError("FileUpload", "Please select an Excel file to upload.");
                    return View(customerReq);
                }

                // Insert request form data
                bool isInsert = _requestFormRepository.RequestFormInsert(customerReq);
                bool UserInsert = _requestFormRepository.UpdateUserDetails(customerReq);

                // Get TAT and Request ID details
                DataSet dstat = _requestFormRepository.GetTatTime();
                DataSet dsID = _requestFormRepository.GetCustomerUniqID();


                if (dstat.Tables.Count == 0 || dsID.Tables.Count == 0 || dstat.Tables[0].Rows.Count == 0 || dsID.Tables[0].Rows.Count == 0)
                {
                    ModelState.AddModelError("", "Error retrieving request processing information.");
                    return View(customerReq);
                }

                string TAT = dstat.Tables[0].Rows[0]["Timeremaining"].ToString();
                string RequestID = dsID.Tables[0].Rows[0]["RequestID"].ToString();


                string Time = "Yours Is " + (Convert.ToInt32(dstat.Tables[0].Rows[0]["PendingRequests"]) + 1) + "Request In Queue Will Be Processed.";
                string ReqID = " Your RequestID Is - " + dsID.Tables[0].Rows[0]["RequestID"].ToString();


                // Insert task and allowtoWork entries
                CustomerReqModel.CustomerAddtask customerAddtask = new CustomerReqModel.CustomerAddtask
                {
                    ReqID = RequestID,
                    LoginDate = DateTime.Now,
                    AssignTO = "None",
                    InitialStatus = "open",
                    DeilveryTime = TAT
                };
                bool TaskInsert = _requestFormRepository.AddTask(customerAddtask);


                CustomerReqModel.Allowtowork allowtowork = new CustomerReqModel.Allowtowork
                {
                    ReqID = RequestID,
                    AssignDate = DateTime.Now,
                    Status = "WIP"
                };
                bool Allowtowork = _requestFormRepository.AllowtoWork(allowtowork);

                // Send email notification

                try
                {
                    string Email = "dataminers@bfil.co.in";
                    string TOEmail = customerReq.UserEMailID;
                    string CCmail = customerReq.AddCustomerEMailID;


                    if (CCmail == "shalabh.saxena@bfil.co.in" || CCmail == "ashish.damani @bfil.co.in")
                    {
                        CCmail = "";
                    }


                    string Subject = $"{RequestID} - {customerReq.Reportname}";
                    string Body = $"Hello All,\n\nYour request is {(Convert.ToInt32(dstat.Tables[0].Rows[0]["PendingRequests"]) + 1)} th in queue and will be processed.\n\nThanks & Regards,\nIT DataMiners.";
                    string password = "bfil@123";


                    using (MailMessage mm = new MailMessage(Email, TOEmail))
                    {
                        mm.Subject = Subject;
                        mm.Body = Body;

                        if (!string.IsNullOrWhiteSpace(CCmail))
                        {
                            mm.CC.Add(CCmail);
                        }

                        mm.Attachments.Add(new Attachment(customerReq.FileUpload));

                        mm.IsBodyHtml = false;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "172.26.100.104";
                        smtp.EnableSsl = false;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        NetworkCredential networkCredential = new NetworkCredential(Email, password);
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = networkCredential;

                        smtp.Port = 516;
                        smtp.Send(mm);
                    }
                }
                catch (Exception ex)
                {
                    // Log error - replace with your logging mechanism
                    // _logger.LogError(ex, "Error sending email.");
                    ModelState.AddModelError("", "Failed to send email notification.");
                    return View(customerReq);
                }

                // Pass RequestID to ViewData for toast notification usage in view
                ViewData["SuccessMessage"] = $"Request submitted successfully! Your Request ID is {RequestID}.";

                // Return view with success
                return View(customerReq);
            }
            catch
            {
                // Log error - replace with your logging mechanism
                // _logger.LogError(ex, "An error occurred in Index POST.");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
                return View(customerReq);
            }
        }
    }
}
