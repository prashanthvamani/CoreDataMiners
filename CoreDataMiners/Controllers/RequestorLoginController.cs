using DataminersBAL;
using DataminersDAL.Repositories;
using DataminersModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Logging;
using System.Data;
using System.Net;
using System.Security.Claims;

namespace DataMinersWeb.Controllers
{
    public class RequestorLoginController : Controller
    {
        private readonly LoginBaL _loginBaL;
        private readonly LoginRepository _loginRepository;

        DataSet ds = new DataSet();
        LoginModel lm = new LoginModel();

        public RequestorLoginController(LoginBaL loginBaL, LoginRepository loginRepository)
        {
            _loginBaL = loginBaL;
            _loginRepository = loginRepository;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        public IActionResult Login(string Uname,string pwd)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            lm.Username = Uname;
            lm.Password = pwd;

            //DataSet ds = _loginBaL.BFILLogin(lm);
            DataSet ds = _loginBaL.ITDBLogin(lm.Username,lm.Password);

            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
               // _logService.WriteLog($"Login failed: No user found for Username={Username}");
                return View();
            }

            string eCode = ds.Tables[0].Rows[0]["Empid"].ToString();
            string eMsg = ds.Tables[0].Rows.Count > 1 ? ds.Tables[0].Rows[1]["Empid"].ToString() : "";
            // ✅ Valid user login
            if (!eCode.StartsWith("E20"))
            {
                string employeeId = eCode;
                var UserName = "BFIL\\" + Uname;
                //string designation = ds.Tables[0].Rows[0]["Designation"].ToString();
                //string employeeName = ds.Tables[0].Rows[0]["EmployeeName"].ToString();

                // Save session info
                HttpContext.Session.SetString("empid", employeeId);
                HttpContext.Session.SetString("userid", UserName);



                //HttpContext.Session.SetString("Designation", designation);
                //HttpContext.Session.SetString("username", employeeName);

               // bool logResult = _loginRepository.UserLogInsert("Forms", employeeName, employeeId, designation, remoteIp, userAgent);

                return RedirectToAction("Index", "RequestorForm");

                //return RedirectToAction("JwtTest", "Test");
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "RequestorLogin");
        }
    }
}
