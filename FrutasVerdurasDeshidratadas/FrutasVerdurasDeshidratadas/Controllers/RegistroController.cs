using FrutasVerdurasDeshidratadas.Models;
using FrutasVerdurasDeshidratadas.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.UI;

namespace FrutasVerdurasDeshidratadas.Controllers
{
    public class RegistroController : Controller
    {
        public string key = "b14ca5898a4e4133bbce2ea2315a1916";

        FrutasyVegetalesDeshidratadosEntities objCon = new FrutasyVegetalesDeshidratadosEntities();

        #region Check Email Exists or not in DB  
        public bool IsEmailExists(string eMail)
        {
            var IsCheck = objCon.Tbl_Members.Where(email => email.EmailId == eMail).FirstOrDefault();
            return IsCheck != null;
        }
        #endregion

        public void SendEmailToSistemOperator(string _eMail, string _nombreCompleto, string _fechaReg)
        {
            try
            {
                string _FromEmailAddress = ConfigurationManager.AppSettings["Sender"].ToString();
                string _FromContresena = ConfigurationManager.AppSettings["PswSender"].ToString();
                string _Smtp = ConfigurationManager.AppSettings["SMTP"].ToString();
                string _emailId = _FromEmailAddress = ConfigurationManager.AppSettings["SOReceiver"].ToString();
                int _Port = int.Parse(ConfigurationManager.AppSettings["PORT"].ToString());

                var fromMail = new MailAddress(_FromEmailAddress, "Sanifrut Registro de Usuarios Nuevos"); 
                var fromEmailpassword = _FromContresena;
                var toEmail = new MailAddress(_emailId);
                
                var smtp = new SmtpClient();
                smtp.Host = _Smtp;
                smtp.Port = _Port;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword);

                var Message = new MailMessage(fromMail, toEmail);
                Message.Subject = "Un nuevo usuario se ha registrado en la plataforma Cinema 86";
                Message.Body = "<br/> Por favor mantengase atento a esta cuenta para proporcionarle el mejor servicio" +
                               "<br/> Nombre: " + _nombreCompleto +
                               "<br/> Correo: " + _eMail +
                               "<br/> Fecha de Registro: " + _fechaReg  +
                               "<br/>< br/>";
                Message.IsBodyHtml = true;

                smtp.Send(Message);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void SendEmailToUser(string emailId)
        {
            try
            {
                string _FromEmailAddress = CryptoEncription.DecryptString(key, ConfigurationManager.AppSettings["Sender"].ToString());//ConfigurationManager.AppSettings["Sender"].ToString();
                string _FromContresena = CryptoEncription.DecryptString(key, ConfigurationManager.AppSettings["PswSender"].ToString());
                string _Smtp = CryptoEncription.DecryptString(key, ConfigurationManager.AppSettings["SMTP"].ToString());
                int _Port = int.Parse(ConfigurationManager.AppSettings["PORT"].ToString());

                var fromMail = new MailAddress(_FromEmailAddress, "SaniFrut eShop Bienvenido"); // set your email  
                var fromEmailpassword = _FromContresena;   
                var toEmail = new MailAddress(emailId);

                var smtp = new SmtpClient();               
                smtp.Host = _Smtp;
                smtp.Port = _Port;
                smtp.EnableSsl = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword);

                var Message = new MailMessage(fromMail, toEmail);
                Message.Subject = "Bienvenido a SaniFrut eShop";
                Message.Body = "<br/> Gracias por registrarse en su Tienda SaniFrut." +
                               "<br/> Le damos una cordial Bienvenida y no olvide enviarnos sus sugerencias." +
                               "<br/> felices compras.";
                Message.IsBodyHtml = true;
                smtp.Send(Message);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // GET: Registro
        [OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public ActionResult Index()
        {

            return View();
        }

        #region Registration post method for data save  
        [HttpPost]
        [OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public ActionResult Index(Member _usr)
        {
            try
            {
                Tbl_Members objUsr = new Tbl_Members();

                if (!ModelState.IsValid)//Valida si los campos cuentan con el estandar correcto.
                {
                    return View();
                }
                    //Primero valido que exista un usuario con el mismo correo.
                    string _emailSearch = _usr._emailId.ToLower().Trim();
                var IsExist = IsEmailExists(_emailSearch);

                if(IsExist)
                {
                    string _MsgRegistration = "Ya existe un usuario registrado con el correo " + _usr._emailId.ToLower().Trim() + " Intente con otro.";
                    ModelState.AddModelError("EmailExist", _MsgRegistration);
                    return View();
                }

                // email not verified on registration time  

                objUsr.FirstName = _usr._firstName.ToUpper().Trim();
                objUsr.LastName = _usr._lastName.ToUpper().Trim();
                objUsr.EmailId= _usr._emailId.ToLower().Trim();
                objUsr.IsActive = true;
                objUsr.IsDelete = false;

                //password convert  
                objUsr.Password = FrutasVerdurasDeshidratadas.Models.EncriptaPassword.EncriptaTexto(_usr._password);
                objUsr.CreatedOn = DateTime.Now;
                objUsr.ModifiedOn = DateTime.Now;
                objCon.Tbl_Members.Add(objUsr);
                objCon.SaveChanges();

                string _fechaRegistro = objUsr.CreatedOn.ToString();

                #region Send eMail to User and Store Manager
                    //SendEmailToUser(objUsr.EmailId); COMENTADO TEMPORALMENTE
                   // SendEmailToSistemOperator(objUsr.EmailId, objUsr.FirstName + "-" + 
                   //     objUsr.LastName, objUsr.CreatedOn.ToString());
                    ViewBag.Message = "Registro completado, Gracias!.";
                TempData["msg"] = "<script>alert('Registro completado, Gracias!.');</script>";

                #endregion

                return RedirectToAction("Index", "Login");
                //return View("Registration");
            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View("Registration");
            }
        }
        #endregion
    }
}