using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using FrutasVerdurasDeshidratadas.DAL;
using FrutasVerdurasDeshidratadas.Models;
using System.Configuration;
using System.Net.Mail;

namespace FrutasVerdurasDeshidratadas.Controllers
{
    public class HomeController : Controller
    {
        FrutasyVegetalesDeshidratadosEntities objCon = new FrutasyVegetalesDeshidratadosEntities();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult QuienesSomos()
        {
            return View();
        }

        public ActionResult PoliticasPrivacidad()
        {
            return View();
        }

        public ActionResult Contacto()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contacto(Contacto _contacto)
        {
            try
            {
                Tbl_Contacto _cont = new Tbl_Contacto();
                _cont.FechaContacto = DateTime.Now;
                _cont.eMail = _contacto.eMail;
                _cont.TextoMensaje = _contacto.TextoMensaje;

                objCon.Tbl_Contacto.Add(_cont);
                objCon.SaveChanges();

                TempData["msg"] = "<script>alert('Mensaje envíado con éxito, en breve estaremos contactandolo para atenderlo. Gracias.');</script>";

                //Armo los componentes del correo a enviar.
                string _FromEmailAddress = ConfigurationManager.AppSettings["Sender"].ToString();
                string _Subject = "Frutas y Vegetales Deshidratados, Mensaje enviado desde el formulario de contacto.";
                string _Body = "<br/> Se ha enviado con éxito su mensaje a nuestro departamento de atención a clientes, en breve estaremos poniéndonos en contacto con ested. Gracias  " +
               "<br/> Su mensaje fue el siguiente: " +
               "<br/> --------------------------------------------------------------" +
               "<br/> Correo: " + _cont.eMail +
               "<br/> Fecha/Hora: " + _cont.FechaContacto +
               "<br/> texto del Mensaje: " + _cont.TextoMensaje.Trim() +
               "<br/> Respetuosamente " +
               "<br/> Gerencia General de Ventas Online Frutas y Vegetales Deshidratados de México";

                //Envio correo al cliente
                var _enviar = SendEmailToCustomer(_cont.eMail.Trim(), _Subject, _Body);

                //Envio correo al vendedor
                var _enviarSeller = SendEmailToCustomer("ventas@salsasmexicanasdeshidratadas.com", _Subject, _Body);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {

                TempData["msg"] = "<script>alert('" + ex.Message + "');</script>";
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Distribuidores()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Distribuidores(Distribuidor _dist)
        {
            try
            {
                //Valido si ya hay un correo registrado anteriormente
                bool Isvalid = objCon.Tbl_Distribuidor.Any(x => x.eMail == _dist.eMail);

                if (Isvalid == false)//Si no existe el correo en la tabla de Distribuidores.
                {
                    var guid = Convert.ToString((new Random()).Next(100000));

                    Tbl_Distribuidor _distT = new Tbl_Distribuidor();
                    _distT.Fecha_Registro = DateTime.Now;
                    _distT.Id_EstatusDist = 2; //Este estatus es para candidatos
                    _distT.Estado = _dist.Estado;
                    _distT.Ciudad_Municipio = _dist.Ciudad_Municipio;
                    _distT.Nombres = _dist.Nombres;
                    _distT.CalleNumero = _dist.CalleNumero;
                    _distT.Colonia = _dist.Colonia;
                    _distT.CP = int.Parse(_dist.CP);                    
                    _distT.Telefono = _dist.Telefono;
                    _distT.eMail = _dist.eMail;
                    _distT.EsProspecto = true;
                    _distT.FolioSolicitud = "FVDH-" + guid;

                    objCon.Tbl_Distribuidor.Add(_distT);
                    objCon.SaveChanges();

                    TempData["msg"] = "<script>alert('Solicitud enviada con éxito su número de folio es: " + "FVDM-" + guid + " para darle seguimiento en un futuro, en breve nos pondremos en contacto con usted. Gracias.');</script>";

                    //Armo los componentes del correo a enviar.
                    string _FromEmailAddress = ConfigurationManager.AppSettings["Sender"].ToString();
                    string _Subject = "Frutas y Vegetales Deshidratados, Envio de Solicitud de Registro de Distribuidores FVDM-" + guid;
                    string _Body = "<br/> Estimado(a) solicitante, de parte de Frutas y vegetales Deshidratados gracias por su solicitud de registro de distribuidor. " +
                   "<br/> Revisaremos sus datos y en breve un representante se pondrá en contacto con usted. " +
                   "<br/> Respetuosamente " +
                   "<br/> Gerencia General de Ventas Online Frutas y Vegetales Deshidratados de México";

                    //Envio correo al cliente
                    var _enviar = SendEmailToCustomer(_dist.eMail.Trim(), _Subject, _Body);

                    //Envio correo al vendedor
                    var _enviarSeller = SendEmailToCustomer("ventas@salsasmexicanasdeshidratadas.com", _Subject, _Body);

                }
                else
                {
                    TempData["msg"] = "<script>alert('Ya existe un solicitante registrado con este correo, verifique o cambie de correo.');</script>";
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["msg"] = "<script>alert('" + ex.Message + "');</script>";
                return RedirectToAction("Index", "Home");
            }
        }

        public bool SendEmailToCustomer(string emailId, string strSubject, string strMsgBody)
        {
            try
            {
                string _FromEmailAddress = ConfigurationManager.AppSettings["Sender"].ToString();
                string _FromContresena = ConfigurationManager.AppSettings["PswSender"].ToString();
                string _Smtp = ConfigurationManager.AppSettings["SMTP"].ToString();
                int _Port = int.Parse(ConfigurationManager.AppSettings["PORT"].ToString());

                var fromMail = new MailAddress(_FromEmailAddress, "SaniFrut eShop Detalle de Compra"); // set your email  
                var fromEmailpassword = _FromContresena;
                var toEmail = new MailAddress(emailId);

                var smtp = new SmtpClient();
                smtp.Host = _Smtp;
                smtp.Port = _Port;
                //smtp.EnableSsl = true;
                smtp.EnableSsl = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword);

                var Message = new MailMessage(fromMail, toEmail);
                Message.Subject = strSubject;//"Bienvenido a SaniFrut eShop";

                Message.Body = strMsgBody;
                Message.IsBodyHtml = true;
                
                smtp.Send(Message);
                return true;
            }
            catch (Exception ex)
            {
                string _errm = ex.Message;
                return false;
            }
        }
    }
}