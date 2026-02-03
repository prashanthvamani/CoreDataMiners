using DataminersDAL.Repositories;
using DataminersModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DataMinersWeb.Controllers
{
    public class ITDBLoginController : Controller
    {
        private readonly ITDBLoginRepository _ITDBrepo;
        public ITDBLoginController(ITDBLoginRepository ITDBrep)
        {
            _ITDBrepo = ITDBrep;
        }
        public IActionResult ITDBLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ITDBLogin(string Username, string password)
        {
            try
            {
                var empid = HttpContext.Session.GetString("empid");
                var userID = HttpContext.Session.GetString("userid");

                LoginModel login = new LoginModel
                {
                    Username = "BFil\\" + Username,
                    Password = password
                };

                string Ntsanto = @"bfil\santosh.aute";
                string sanpassword = "qazxsw123";

                if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(password))
                {

                    DataSet ds = _ITDBrepo.ITDBLogin(login.Username, login.Password);

                    if (ds.Tables.Count > 0 &&  ds.Tables[0].Rows.Count > 0)
                    {
                        if (Ntsanto == Username && sanpassword == "qazxsw123")
                        {
                            string Ntlogid = ds.Tables[0].Rows[0]["NTloginid"].ToString();
                            string Leveluser = ds.Tables[0].Rows[0]["Level"].ToString();
                            //Session["Level"] = Leveluser;

                            HttpContext.Session.SetString("Level", Leveluser);

                            return RedirectToAction("Index", "ITDBCheckerDetails");

                        }
                        else if (Ntsanto != Username)
                        {
                            string Ntlogid = ds.Tables[0].Rows[0]["NTloginid"].ToString();
                            string Leveluser = ds.Tables[0].Rows[0]["Level"].ToString();

                            HttpContext.Session.SetString("Level", Leveluser);

                            return RedirectToAction("Index", "ITDBCheckerDetails");
                        }
                        else
                        {
                            // Invalid username or password
                            ViewBag.StatusMessage = "Invalid Username or Password!";
                            return View();
                        }
                    }
                    else
                    {
                        // Invalid username or password
                        ViewBag.StatusMessage = "Invalid Username or Password!";
                        return View();
                    }
                }
                else
                {
                    // Invalid username or password
                    ViewBag.StatusMessage = "Invalid Username or Password!";
                    return View();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
