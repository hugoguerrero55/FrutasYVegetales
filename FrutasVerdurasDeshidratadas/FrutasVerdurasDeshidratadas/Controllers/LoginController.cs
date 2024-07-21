using FrutasVerdurasDeshidratadas.Models;
using FrutasVerdurasDeshidratadas.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Data.Entity;

namespace FrutasVerdurasDeshidratadas.Controllers
{
    public class LoginController : Controller
    {
        FrutasyVegetalesDeshidratadosEntities objCon = new FrutasyVegetalesDeshidratadosEntities();

        // GET: Login
        [OutputCache(Duration = 60, Location =OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public ActionResult Index(Login LgnUsr, string _ubicacion)
        {
            try
            {
                if (LgnUsr._password != null)
                {
                    var _passWord = FrutasVerdurasDeshidratadas.Models.EncriptaPassword.EncriptaTexto(LgnUsr._password);
                    bool Isvalid = objCon.Tbl_Members.Any(x => x.EmailId == LgnUsr._emailId && x.Password == _passWord);

                    if (Isvalid)
                    {
                        int timeout = LgnUsr._rememberme ? 60 : 30; // Timeout in minutes, 60 = 1 hour.  
                        var ticket = new FormsAuthenticationTicket(LgnUsr._emailId, false, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = System.DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        //Checo desde que producto esta llenando el carrito.
                        switch (_ubicacion)
                        {
                            case "salsas":
                                return RedirectToAction("Salsas", "Store", new { _idCategory = 1 });
                            case "frutas":
                                return RedirectToAction("Frutas", "Store", new { _idCategory = 2 });
                            case "tisanas":
                                return RedirectToAction("Tisanas", "Store", new { _idCategory = 3 });
                            default:
                                return RedirectToAction("Index", "Home");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Correo o Contraseña Inválidos, Verifique que su cuenta no haya caducado... Intente de nuevo!");
                    }

                }

                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View();
            }
        }

        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }

        public bool IsEmailExists(string eMail)
        {
            var IsCheck = objCon.Tbl_Members.Where(email => email.EmailId == eMail).FirstOrDefault();
            return IsCheck != null;
        }


        public string GeneratePassword()
        {
            string OTPLength = "4";
            string OTP = string.Empty;

            string Chars = string.Empty;
            Chars = "1,2,3,4,5,6,7,8,9,0";

            char[] seplitChar = { ',' };
            string[] arr = Chars.Split(seplitChar);
            string NewOTP = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i < Convert.ToInt32(OTPLength); i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                NewOTP += temp;
                OTP = NewOTP;
            }
            return OTP;
        }

        private DateTime _getServerDateTime()
        {
            try
            {
                DateTime dateTimeServer = objCon.Database.SqlQuery<DateTime>("SELECT GETDATE() Fecha").SingleOrDefault();

                return dateTimeServer;
            }
            catch (Exception)
            {
                return Convert.ToDateTime("1900-01-01 00:00:00.000"); 
            }
        }
    }
}