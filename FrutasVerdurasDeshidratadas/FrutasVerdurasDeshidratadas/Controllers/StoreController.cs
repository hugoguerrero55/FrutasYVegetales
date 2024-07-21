using FrutasVerdurasDeshidratadas.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrutasVerdurasDeshidratadas.Repository;
using FrutasVerdurasDeshidratadas.Models;
using PagedList;
using System.Data;
using PayPal.Api;
using System.Configuration;
using System.Web.Routing;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Data.Entity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mail;
using System.Text;
using System.Net;


namespace FrutasVerdurasDeshidratadas.Controllers
{
    public class StoreController : Controller
    {
        public string key = "b14ca5898a4e4133bbce2ea2315a1916";
        public enum _Cart_Status : ushort
        {
            Agregado = 1,
            Eliminado = 2,
            Pagado = 3
        }
        
        FrutasyVegetalesDeshidratadosEntities objCon = new FrutasyVegetalesDeshidratadosEntities();
        #region Other Class references ...         
        // Instance on Unit of Work         
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        #endregion
        protected static int _cveMiembro = 0;
        protected static string Divisa = ConfigurationManager.AppSettings["divisa"].ToString();


        // GET: Store
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tisanas(int _idCategory, int? page)
        {
            ViewBag.CategoriaId = _idCategory;
            List<Tbl_Product> _listProducto = new List<Tbl_Product>();

            _listProducto = objCon.Tbl_Product.Where(x => x.CategoryId == _idCategory).OrderBy(x => x.ProductName).ToList();



            //Obtengo el Id del usuario
            if (User.Identity.Name.Length != 0)
            {
                string _eml = User.Identity.Name;
                var _lstIdUSR = objCon.Tbl_Members.Where(x => x.EmailId == _eml).ToList();
                int _memberid = _lstIdUSR[0].MemberId;
                string _nomCompletoUsuario = _lstIdUSR[0].FirstName + " " + _lstIdUSR[0].LastName;
                var _lstenCarrito = objCon.Tbl_ShoppingCart.Where(i => i.MemberId == _memberid && i.CartStatusId == 1).ToList();
                ViewBag.ElementosCarritoUSR = _lstenCarrito.Count();
                ViewBag.NombreCompletoUSR = _nomCompletoUsuario;
                ViewBag.MemberUSRID = _memberid;

            }
            else
            {
                ViewBag.ElementosCarritoUSR = 0;
                ViewBag.NombreCompletoUSR = string.Empty;
            }

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(_listProducto.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Frutas(int _idCategory, int? page)
        {
            ViewBag.CategoriaId = _idCategory;
            List<Tbl_Product> _listProducto = new List<Tbl_Product>();

            _listProducto = objCon.Tbl_Product.Where(x => x.CategoryId == _idCategory).OrderBy(x => x.ProductName).ToList();



            //Obtengo el Id del usuario
            if (User.Identity.Name.Length != 0)
            {
                string _eml = User.Identity.Name;
                var _lstIdUSR = objCon.Tbl_Members.Where(x => x.EmailId == _eml).ToList();
                int _memberid = _lstIdUSR[0].MemberId;
                string _nomCompletoUsuario = _lstIdUSR[0].FirstName + " " + _lstIdUSR[0].LastName;
                var _lstenCarrito = objCon.Tbl_ShoppingCart.Where(i => i.MemberId == _memberid && i.CartStatusId == 1).ToList();
                ViewBag.ElementosCarritoUSR = _lstenCarrito.Count();
                ViewBag.NombreCompletoUSR = _nomCompletoUsuario;
                ViewBag.MemberUSRID = _memberid;

            }
            else
            {
                ViewBag.ElementosCarritoUSR = 0;
                ViewBag.NombreCompletoUSR = string.Empty;
            }

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(_listProducto.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ProductoDetalle(int _productId, string _posicion)
        {
            return View();
        }

        public ActionResult Salsas(int _idCategory, int? page)
        {
            try
            {
                ViewBag.CategoriaId = _idCategory;
                List<Tbl_Product> _listProducto = new List<Tbl_Product>();

                _listProducto = objCon.Tbl_Product.Where(x => x.CategoryId == _idCategory).OrderBy(x => x.ProductName).ToList();



                //Obtengo el Id del usuario
                if (User.Identity.Name.Length != 0)
                {
                    string _eml = User.Identity.Name;
                    var _lstIdUSR = objCon.Tbl_Members.Where(x => x.EmailId == _eml).ToList();
                    int _memberid = _lstIdUSR[0].MemberId;
                    string _nomCompletoUsuario = _lstIdUSR[0].FirstName + " " + _lstIdUSR[0].LastName;
                    var _lstenCarrito = objCon.Tbl_ShoppingCart.Where(i => i.MemberId == _memberid && i.CartStatusId == 1).ToList();
                    ViewBag.ElementosCarritoUSR = _lstenCarrito.Count();
                    ViewBag.NombreCompletoUSR = _nomCompletoUsuario;
                    ViewBag.MemberUSRID = _memberid;

                }
                else
                {
                    ViewBag.ElementosCarritoUSR = 0;
                    ViewBag.NombreCompletoUSR = string.Empty;
                }

                int pageSize = 4;
                int pageNumber = (page ?? 1);
                return View(_listProducto.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return View();
            }

        }

        public ActionResult Carrito(int _memberId)
        {
            List<USP_Get_Elements_To_ShoppingCart_Result> lstMemberShoping = new List<USP_Get_Elements_To_ShoppingCart_Result>();

            using (var ctx = new FrutasyVegetalesDeshidratadosEntities())
            {
                lstMemberShoping = ctx.Database.SqlQuery<USP_Get_Elements_To_ShoppingCart_Result>("EXEC USP_Get_Elements_To_ShoppingCart @memberId"
                    , new SqlParameter("@memberId", _memberId)).ToList();
            }


            //var _lstenCarrito = objCon.Tbl_ShoppingCart.Where(i => i.MemberId == _memberId).ToList();
            ViewBag.ElementosCarritoUSR = lstMemberShoping.Count();//_lstenCarrito.Count();
            ViewBag.IdMember = _memberId;

            return View(lstMemberShoping);
        }

        public ActionResult Detalle(int _productId)
        {
            List<Tbl_Product> _lstproducto = new List<Tbl_Product>();

            _lstproducto = objCon.Tbl_Product.Where(x => x.ProductId == _productId).ToList();

            return View(_lstproducto);
        }

        public ActionResult Mas(int _memberID , int _productId)
        {
            try
            {
                //int _memberid;
                string _email = string.Empty;
                string[] _cant = Request.Form.GetValues(name: "cantidad");//int.Parse(ViewBag.Cant);

                _email = User.Identity.Name;

                if (User.Identity.Name.Length == 0)
                {
                    return RedirectToAction("Carrito", "Store");
                }

                //Trae los datos del producto mediante el <SP USP_Get_Elements_To_ShoppingCart>
                List<USP_Get_Elements_From_ShoppingCart_By_ID_Result> lstAddToCart = new List<USP_Get_Elements_From_ShoppingCart_By_ID_Result>();

                using (var ctx = new FrutasyVegetalesDeshidratadosEntities())
                {
                    lstAddToCart = ctx.Database.SqlQuery<USP_Get_Elements_From_ShoppingCart_By_ID_Result>("EXEC USP_Get_Elements_From_ShoppingCart_By_ID @p_ProductID"
                        , new SqlParameter("@p_ProductID", _productId)).ToList();
                }


                //Ahora agrego en la tabla Tbl_ShoppingCart cada uno de los valores.
                Tbl_ShoppingCart cartAdd = new Tbl_ShoppingCart();
                cartAdd.Price = Convert.ToDecimal(lstAddToCart[0].Price);
                cartAdd.Quantity = 1;
                cartAdd.Total = Convert.ToDecimal(lstAddToCart[0].SubTotal);
                cartAdd.AddedOn = DateTime.Now;
                cartAdd.CartStatusId = Convert.ToInt32(_Cart_Status.Agregado);
                cartAdd.MemberId = _memberID;
                cartAdd.ProductId = _productId;
                cartAdd.UpdatedOn = DateTime.Now;
                objCon.Tbl_ShoppingCart.Add(cartAdd);
                objCon.SaveChanges();
                
                var _lstenCarrito = objCon.Tbl_ShoppingCart.Where(i => i.MemberId == _memberID).ToList();


                ViewBag.ElementosCarritoUSR = _lstenCarrito.Count();
                
                return RedirectToAction("Carrito", "Store", new { _memberid = _memberID });
            }
            catch (Exception ex)
            {
                TempData["msg"] = "<script>alert('" + ex.Message + "');</script>";
                return null;
            }
        }
        public ActionResult Menos(int _memberID, int _productId)
        {
            try
            {

                int _id = _memberID;
                int _prodid = _productId;

                using (FrutasyVegetalesDeshidratadosEntities bd = new FrutasyVegetalesDeshidratadosEntities())
                {
                    //Veo primero si los que quedan son por lo menos 1.
                    var _lstCuantosHay = objCon.Tbl_ShoppingCart.Where(s => s.MemberId == _id && s.ProductId == _prodid && s.CartStatusId == 1).ToList();
                    
                    if (_lstCuantosHay.Count == 0)
                    {
                        return RedirectToAction("Carrito", "Store", new { _memberId = _id });
                    }

                    var query = (from p in bd.Tbl_ShoppingCart where p.MemberId == _id && p.ProductId == _prodid && p.CartStatusId == 1 select p).FirstOrDefault();

                    bd.Tbl_ShoppingCart.Remove(query);
                    bd.SaveChanges();

                    var _lstCuantosquedan = objCon.Tbl_ShoppingCart.Where(s => s.MemberId == _id && s.CartStatusId == 1).ToList();

                    if (_lstCuantosquedan.Count == 0)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Carrito", "Store", new { _memberId = _id });
                    }


                }
            }
            catch (Exception ex)
            {

                TempData["msg"] = "<script>alert('" + ex.Message + "');</script>";
                return null;
            }
        }

        public ActionResult AgregaAlCarrito(int _productId, string _posicion)
        {
            try
            {                
                int _memberid;                
                string _email = string.Empty;
                string[] _cant = Request.Form.GetValues(name:"cantidad");//int.Parse(ViewBag.Cant);

                _email = User.Identity.Name;

                if (User.Identity.Name.Length == 0)
                {
                    return RedirectToAction("Index", "Login", new { _ubicacion = _posicion });
                }
                else
                {
                    List<Tbl_Members> _lstmiembros = new List<Tbl_Members>();

                    _lstmiembros = objCon.Tbl_Members.Where(x => x.EmailId == _email).ToList();
                    _memberid = _lstmiembros[0].MemberId;
                }

                //Trae los datos del producto mediante el <SP USP_Get_Elements_To_ShoppingCart>
                List<USP_Get_Elements_From_ShoppingCart_By_ID_Result> lstAddToCart = new List<USP_Get_Elements_From_ShoppingCart_By_ID_Result>();

                using (var ctx = new FrutasyVegetalesDeshidratadosEntities())
                {
                    lstAddToCart = ctx.Database.SqlQuery<USP_Get_Elements_From_ShoppingCart_By_ID_Result>("EXEC USP_Get_Elements_From_ShoppingCart_By_ID @p_ProductID"
                        , new SqlParameter("@p_ProductID", _productId)).ToList();
                }


                //Ahora agrego en la tabla Tbl_ShoppingCart cada uno de los valores.
                Tbl_ShoppingCart cartAdd = new Tbl_ShoppingCart();
                cartAdd.Price = Convert.ToDecimal(lstAddToCart[0].Price);
                cartAdd.Quantity = 1;
                cartAdd.Total = Convert.ToDecimal(lstAddToCart[0].SubTotal);
                cartAdd.AddedOn = DateTime.Now;
                cartAdd.CartStatusId = Convert.ToInt32(_Cart_Status.Agregado);
                cartAdd.MemberId = _memberid;
                cartAdd.ProductId = _productId;
                cartAdd.UpdatedOn = DateTime.Now;
                objCon.Tbl_ShoppingCart.Add(cartAdd);
                objCon.SaveChanges();

                TempData["msg"] = "<script>alert('Se ha Agregado [ 1 ] Producto al carrito de compras.');</script>";

                var _lstenCarrito = objCon.Tbl_ShoppingCart.Where(i => i.MemberId == _memberid).ToList();


                ViewBag.ElementosCarritoUSR = _lstenCarrito.Count();

                //Checo desde que producto esta llenando el carrito.
                switch (_posicion)
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
            catch (Exception ex)
            {
                TempData["msg"] = "<script>alert('" + ex.Message + "');</script>";
                return null;
            }
        }

        public ActionResult EliminaDelCarrito(int _memberID, int _productId)
        {
            try
            {

                int _id = _memberID;
                int _prodid = _productId;

                using (FrutasyVegetalesDeshidratadosEntities bd = new FrutasyVegetalesDeshidratadosEntities())
                {
                    bd.Tbl_ShoppingCart.RemoveRange(bd.Tbl_ShoppingCart.Where(c => c.MemberId == _id && c.ProductId == _prodid && c.CartStatusId == 1));
                    bd.SaveChanges();

                    var _lstCuantosquedan = objCon.Tbl_ShoppingCart.Where(s => s.MemberId == _id && s.CartStatusId == 1).ToList();

                    if (_lstCuantosquedan.Count == 0)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["msg"] = "<script>alert('Producto eliminado de su carrito de compras.');</script>";

                        return RedirectToAction("Carrito", "Store", new { _memberId = _id });
                    }


                }
            }
            catch (Exception ex)
            {

                //return ViewBag.Message = ex.Message;
                TempData["msg"] = "<script>alert('" + ex.Message + "');</script>";
                return null;
            }
        }

        //[ChildActionOnly]
        //public ActionResult _DesgloceCarrito(int _idMember)
        //{
        //    List<USP_Proceed_To_Pay_Result> lstMemberShoping = new List<USP_Proceed_To_Pay_Result>();

        //    using (var ctx = new FrutasyVegetalesDeshidratadosEntities())
        //    {
        //        lstMemberShoping = ctx.Database.SqlQuery<USP_Proceed_To_Pay_Result>("EXEC USP_Proceed_To_Pay @memberId"
        //            , new SqlParameter("@memberId", _idMember)).ToList();
        //    }

        //    return View(lstMemberShoping);
        //}

        //[ChildActionOnly]
        //public ActionResult _Member(int _idMember)
        //{            
        //    try
        //    {
        //        List<Tbl_Members> _lsttblMembers = objCon.Tbl_Members.Where(s => s.MemberId == _idMember).ToList();

        //        return View(_lsttblMembers);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }            

        //}

        public ActionResult CheckOut(int _idMember)
        {
            _cveMiembro = _idMember;
            List<USP_Proceed_To_Pay_Result> lstCheckOut = new List<USP_Proceed_To_Pay_Result>();

            using (var ctx = new FrutasyVegetalesDeshidratadosEntities())
            {
                lstCheckOut = ctx.Database.SqlQuery<USP_Proceed_To_Pay_Result>("EXEC USP_Proceed_To_Pay @p_MemberID"
                    , new SqlParameter("@p_MemberID", _idMember)).ToList();
            }

            ViewBag.IdMember = _cveMiembro;

            return View(lstCheckOut);
        }
        public ActionResult Envio(string _idTrans, string _orderID, string _amountPaid)
        {
            Envios _envios = new Envios();
            _envios.TransactionID = _idTrans;
            _envios.OrderId = _orderID;
            _envios.AmountPaid = Convert.ToDecimal(_amountPaid);

            return View(_envios);
        }

        [HttpPost]
        public ActionResult Envio(Envios _env)
        {
            try
            {
                string _sqlCmd = string.Empty;

                if (_cveMiembro > 0)
                {
                    _sqlCmd = "INSERT INTO  Tbl_ShippingDetails (MemberId, AddressLine, City, State, Municipio, ZipCode, TransactionID, OrderId, AmountPaid) " +
                              "VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8})";
                    using (var ctx = new FrutasyVegetalesDeshidratadosEntities())
                    {
                        int _noOfRowInserted = ctx.Database.ExecuteSqlCommand(_sqlCmd, _cveMiembro, _env.AddressLine.ToUpper()
                            , _env.City.ToUpper(), _env.State.ToUpper(), _env.Municipio.ToUpper(), _env.ZipCode
                            , _env.TransactionID, _env.OrderId, _env.AmountPaid);
                    }
                }

                //Actualiza en el carrito el shipping ID
                var _idshippingROW = objCon.Tbl_ShippingDetails.Where(s => s.MemberId == _cveMiembro && s.TransactionID == _env.TransactionID).FirstOrDefault();

                int _idShipping = _idshippingROW.ShippingDetailId;

                //Le pongo el Id del Shipping a los elementos del carrito
                CambiaShipingIdDelCart(_cveMiembro, _idShipping);

                //Llamo ala función para que cambie de estatus el carrito de compras del usuario poniendolo con el estatus de Pagado.
                CambiaEstatusCart(_cveMiembro);


                ViewBag.MiembroId = _cveMiembro;
                ViewBag.TransaccionNumber = _env.TransactionID;

                return RedirectToAction("Comprobante", "Store", new { _idMember = ViewBag.MiembroId, _transaccion = @ViewBag.TransaccionNumber });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult Comprobante(int _idMember, string _transaccion)
        {

            List<USP_ShowOrderCompleted_Result> lstComprobante = new List<USP_ShowOrderCompleted_Result>();
            using (var ctx = new FrutasyVegetalesDeshidratadosEntities())
            {
                lstComprobante = ctx.Database.SqlQuery<USP_ShowOrderCompleted_Result>("EXEC USP_ShowOrderCompleted @p_MemberID , @p_Transaccion",
                     new SqlParameter("@p_MemberID", _idMember),
                     new SqlParameter("@p_Transaccion", _transaccion)).ToList();
            }

            ViewBag.MiembroId = _idMember;
            ViewBag.TransaccionNumber = _transaccion;

            return View(lstComprobante);
        }

        public ActionResult FinCompra(int _idMember, string _transaccion)
        {

            List<USP_ShowOrderCompleted_Result> _listTicket = new List<USP_ShowOrderCompleted_Result>();
            using (var ctx = new FrutasyVegetalesDeshidratadosEntities())
            {
                _listTicket = ctx.Database.SqlQuery<USP_ShowOrderCompleted_Result>("EXEC USP_ShowOrderCompleted @p_MemberID , @p_Transaccion",
                     new SqlParameter("@p_MemberID", _idMember),
                     new SqlParameter("@p_Transaccion", _transaccion)).ToList();
            }

            //Genero el ticket
            var _generadoPDF = GeneraPDFTicket(_listTicket);

            if (_generadoPDF != string.Empty)
            {
                //Armo los componentes del correo a enviar.
                string _FromEmailAddress = CryptoEncription.DecryptString(key, ConfigurationManager.AppSettings["Sender"].ToString());
                string _ToEmailAddress = CryptoEncription.DecryptString(key, ConfigurationManager.AppSettings["SOReceiver"].ToString());
                string _Subject = "Detalle de Compra Folio " + _generadoPDF;
                string _Body = "<br/> Estimado(a) " + _listTicket[0].Nombres + " " + _listTicket[0].Apellidos + ", de parte de Frutas y Vegetales Deshidratados gracias por su compra. " +
               "<br/< br/>" +
               "<br/> Adjunto a este correo le enviámos el detalle de su compra, con fólio " + _listTicket[0].Folio +
               "<br/< br/>" +
               "<br/> Lo esperamos pronto de nueva cuenta con nosotros en su tienda online de Frutas y Vegetales Deshidratados 🛒" +
               "<br/< br/>" +
               "<br/< br/>" +
               "<br/> Respetuosamente " +
               "<br/> Gerencia de Ventas Online Frutas y Vegetales Deshidratados de México" +
               "<br/< br/>";

                //Envio correo al cliente
                var _enviar = SendEmailToCustomer(_listTicket[0].eMail.Trim(), _Subject, _Body, _generadoPDF + ".pdf");

                //Envio correo al vendedor
                var _enviarSeller = SendEmailToCustomer(_ToEmailAddress, _Subject, _Body, _generadoPDF + ".pdf");

                if (_enviar && _enviarSeller)
                {
                    TempData["msg"] = "<script>alert('Gracias por su compra. Se ha enviado el comprobante a su cuenta de correo.');</script>";
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            Envios _envio = new Envios();
            try
            {
                //Aqui valido que los campos del embarque capturados por el usuario esten completos.

                //Un recurso que representa a un pagador que financia un método de pago como paypal
                //La identificación del pagador se devolverá cuando proceda el pago o haga clic para pagar
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    // esta sección se ejecutará primero porque PayerID no existe
                    // es devuelto por la llamada a la función de creación de la clase de pago

                    // Creando un pago
                    // baseURL es la URL en la que PayPal envía los datos.
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Store/PaymentWithPayPal?";

                    // aquí estamos generando guid para almacenar el ID de pago recibido en la sesión
                    // que se utilizará en la ejecución del pago
                    var guid = Convert.ToString((new Random()).Next(100000));

                    // La función CreatePayment nos da la URL de aprobación del pago
                    // en qué pagador se redirige para el pago de la cuenta de PayPal
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    // obtener enlaces devueltos por paypal en respuesta a la llamada de función Create
                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            // guardar la URL payapalredirect a la que se redirigirá al usuario para el pago
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // guardar el ID de pago en la clave guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {

                    // Esta función se ejecuta después de recibir todos los parámetros para el pago
                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    // Si el pago ejecutado falló, mostraremos un mensaje de error de pago al usuario
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }

                    //Prelleno la clase Envio para que despues el usuario la llene.                    
                    _envio.TransactionID = executedPayment.id;
                    _envio.AmountPaid = Convert.ToDecimal(executedPayment.transactions[0].amount.total);
                    _envio.OrderId = guid;
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            //Actualizo el stock restandole la cantidad de productos adquiridos.
            bool _rest = RestadelProductStock(_cveMiembro);


            ////Llamo ala función para que cambie de estatus el carrito de compras del usuario.
            //CambiaEstatusCart(_cveMiembro);

            //Cambio el contenido de del cqarrito de compras del usuario a
            //Muestro la pantalla de operacion exitosa con los datos de la compra.
            //return View("SuccessView");
            return RedirectToAction("Pagado", "Store", new RouteValueDictionary(_envio));

        }

        private void CambiaEstatusCart(int _idmiembro)
        {
            try
            {
                List<Tbl_ShoppingCart> results = (from p in objCon.Tbl_ShoppingCart
                                          where p.MemberId == _idmiembro
                                          select p).ToList();

                foreach (Tbl_ShoppingCart p in results)
                {
                    p.CartStatusId = 3;
                }

                objCon.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private bool RestadelProductStock(int _idmiembro)
        {
            int _cantidadTotalProductos = 0;
            int _cveProducto = 0;
            List<Tbl_Product> _lstActuaStock = new List<Tbl_Product>();

            try
            {
                //Traigo las claves de producto del carrito de compras para los productos seleccionados por el usuario.
                List<USP_Proceed_To_Pay_Result> lstItemsPay = _unitOfWork.GetRepositoryInstance<USP_Proceed_To_Pay_Result>().GetResultBySqlProcedure("USP_Proceed_To_Pay @p_MemberID",
                new SqlParameter("p_MemberID", System.Data.SqlDbType.Int) { Value = _idmiembro }).ToList();

                foreach (var item in lstItemsPay)
                {
                    _cveProducto = (int)(item.ClaveProducto); //Capturo la clave del producto
                    _cantidadTotalProductos = (int)(item.Cantidad);//Cacho la cantidad total para ese producto.
                    _lstActuaStock = null;

                    //Lleno una lista con el valor de actualstock del producto.
                    _lstActuaStock = objCon.Tbl_Product.Where(x => x.ProductId == _cveProducto).ToList();

                    int _actualstock = Convert.ToInt32(_lstActuaStock[0].ActualStock); //Cacho el valor del actual stock del producto.

                    using (var ctx = new FrutasyVegetalesDeshidratadosEntities())
                    {
                        int noOfRowUpdated = 0;

                        if (_actualstock > _cantidadTotalProductos)//Si el stoc actual es mayor a la cantidad a restar de producto en el carrito.
                        {
                            noOfRowUpdated = ctx.Database.ExecuteSqlCommand("Update Tbl_Product Set ActualStock = " +
                            (_actualstock - _cantidadTotalProductos).ToString() + " Where ProductId = " + _cveProducto);//Le resto al stock.
                        }
                        else if (_actualstock == _cantidadTotalProductos)//Si la cantidad a restar es igual a la que hay en el stock actual.
                        {
                            noOfRowUpdated = ctx.Database.ExecuteSqlCommand("Update Tbl_Product Set ActualStock = 0 Where ProductId = " + _cveProducto);
                        }
                        else if (_cantidadTotalProductos > _actualstock)
                        {
                            noOfRowUpdated = ctx.Database.ExecuteSqlCommand("Update Tbl_Product Set ActualStock = " +
                            _lstActuaStock[0].ActualStock + " Where ProductId = " + _cveProducto);
                        }
                    }
                }

                return true;

            }
            catch (Exception ex)
            {

                return false;
            }
        }

        private void CambiaShipingIdDelCart(int _idmiembro, int _shipID)
        {
            try
            {
                List<Tbl_ShoppingCart> results = (from p in objCon.Tbl_ShoppingCart
                                          where p.MemberId == _idmiembro && p.CartStatusId == 1
                                          select p).ToList();

                foreach (Tbl_ShoppingCart p in results)
                {
                    p.ShippingDetailId = _shipID;
                }

                objCon.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult Pagado(Envios _env)
        {
            List<Envios> _lstEnvios = new List<Envios>();
            _lstEnvios.Add(new Envios
            {
                OrderId = _env.OrderId,
                AmountPaid = _env.AmountPaid,
                TransactionID = _env.TransactionID
            });

            ViewBag.TransID = _lstEnvios[0].TransactionID;
            ViewBag.OrderID = _lstEnvios[0].OrderId;
            ViewBag.AmountPaid = _lstEnvios[0].AmountPaid;

            //Aqui se hace un update
            return View(_lstEnvios);
        }
        private object CreatePayment(object apiContext, string v)
        {
            throw new NotImplementedException();
        }

        #region Metodo de pago PayPal
        private PayPal.Api.Payment payment;

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {

            //Traigo la transaccion procvesada
            List<USP_Proceed_To_Pay_Result> lstItemsPay = _unitOfWork.GetRepositoryInstance<USP_Proceed_To_Pay_Result>().GetResultBySqlProcedure("USP_Proceed_To_Pay @p_MemberID",
                new SqlParameter("p_MemberID", System.Data.SqlDbType.Int) { Value = _cveMiembro }).ToList();


            //create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };

            decimal _total = 0;

            foreach (var item in lstItemsPay)
            {
                //Adding Item Details like name, currency, price etc
                itemList.items.Add(new Item()
                {
                    name = item.NomProducto.ToString(),// "Item Name comes here",
                    currency = Divisa, //USD", //MXN
                    price = item.PrecioUnitario.ToString(),//"1",
                    quantity = item.Cantidad.ToString(),//"1",
                    sku = "sku"
                });

                _total = _total + Convert.ToDecimal(item.SubTotal);
            }

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            // Adding Tax, shipping and Subtotal details
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = _total.ToString()//"0"
            };

            //Final amount with details
            var amount = new Amount()
            {
                currency = Divisa,
                total = _total.ToString(),//"3", // Total must be equal to sum of tax, shipping and subtotal.
                details = details
            };

            var _invoiceID = Convert.ToString((new Random()).Next(100000));

            var transactionList = new List<Transaction>();
            // Adding description about the transaction
            transactionList.Add(new Transaction()
            {
                description = "Compra en e-Shop SaniFrut",
                invoice_number = "FVDM-" + _invoiceID + DateTime.Now.Year.ToString() +
                DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0'),//Guid.NewGuid().ToString(),//"your invoice number", //Generate an Invoice No
                amount = amount,
                item_list = itemList
            });


            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        #endregion

        #region Manejo de Ticket
        private string GeneraPDFTicket(List<USP_ShowOrderCompleted_Result> _lstTicket)
        {
            try
            {
                Document doc = new Document(PageSize.LETTER);
                // Indicamos donde vamos a guardar el documento
                PdfWriter writer = PdfWriter.GetInstance(doc,
                    new FileStream(Server.MapPath("/Comprobantes/" + _lstTicket[0].Folio.ToString() + ".pdf"), FileMode.Create));

                // Le colocamos el título y el autor
                // **Nota: Esto no será visible en el documento
                doc.AddTitle("Detalle de compra Folio: " + _lstTicket[0].Folio.ToString());
                doc.AddCreator("Frutas y Verduras Deshidratadas");

                // Abrimos el archivo
                doc.Open();

                // Creamos la imagen y le ajustamos el tamaño
                iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(Server.MapPath("/images/logo.jpg"));
                imagen.BorderWidth = 0;
                imagen.Alignment = Element.ALIGN_RIGHT;
                float percentage = 0.0f;
                percentage = 100 / imagen.Width;
                imagen.ScalePercent(percentage * 100);

                // Insertamos la imagen en el documento
                doc.Add(imagen);

                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                // Escribimos el encabezamiento en el documento
                doc.Add(new Paragraph("Detalle de entrega Folo- " + _lstTicket[0].Folio.ToString()));
                doc.Add(Chunk.NEWLINE);

                // Creamos una tabla que contendrá el nombre, apellido y país
                // de nuestros visitante.
                PdfPTable tblEnvio = new PdfPTable(7);
                tblEnvio.WidthPercentage = 100;

                // Configuramos el título de las columnas de la tabla
                PdfPCell clNombre = new PdfPCell(new Phrase("Nombre", _standardFont));
                clNombre.BorderWidth = 0;
                clNombre.BorderWidthBottom = 0.75f;

                PdfPCell cleMail = new PdfPCell(new Phrase("eMail", _standardFont));
                cleMail.BorderWidth = 0;
                cleMail.BorderWidthBottom = 0.75f;

                PdfPCell clDireccion = new PdfPCell(new Phrase("Dirección", _standardFont));
                clDireccion.BorderWidth = 0;
                clDireccion.BorderWidthBottom = 0.75f;

                PdfPCell clCiudad = new PdfPCell(new Phrase("Ciudad", _standardFont));
                clCiudad.BorderWidth = 0;
                clCiudad.BorderWidthBottom = 0.75f;

                PdfPCell clEstado = new PdfPCell(new Phrase("Estado", _standardFont));
                clEstado.BorderWidth = 0;
                clEstado.BorderWidthBottom = 0.75f;

                PdfPCell clMunicipio = new PdfPCell(new Phrase("Municipio", _standardFont));
                clMunicipio.BorderWidth = 0;
                clMunicipio.BorderWidthBottom = 0.75f;

                PdfPCell clCP = new PdfPCell(new Phrase("CP", _standardFont));
                clCP.BorderWidth = 0;
                clCP.BorderWidthBottom = 0.75f;

                // Añadimos las celdas a la tabla
                tblEnvio.AddCell(clNombre);
                tblEnvio.AddCell(cleMail);
                tblEnvio.AddCell(clDireccion);
                tblEnvio.AddCell(clCiudad);
                tblEnvio.AddCell(clEstado);
                tblEnvio.AddCell(clMunicipio);
                tblEnvio.AddCell(clCP);

                // Llenamos la tabla con información
                clNombre = new PdfPCell(new Phrase(_lstTicket[0].Nombres + " " + _lstTicket[0].Apellidos, _standardFont));
                clNombre.BorderWidth = 0;

                cleMail = new PdfPCell(new Phrase(_lstTicket[0].eMail, _standardFont));
                cleMail.BorderWidth = 0;

                clDireccion = new PdfPCell(new Phrase(_lstTicket[0].Direccion, _standardFont));
                clDireccion.BorderWidth = 0;

                clCiudad = new PdfPCell(new Phrase(_lstTicket[0].Ciudad, _standardFont));
                clCiudad.BorderWidth = 0;

                clEstado = new PdfPCell(new Phrase(_lstTicket[0].Estado, _standardFont));
                clEstado.BorderWidth = 0;

                clMunicipio = new PdfPCell(new Phrase(_lstTicket[0].Municipio, _standardFont));
                clMunicipio.BorderWidth = 0;

                clCP = new PdfPCell(new Phrase(_lstTicket[0].CP, _standardFont));
                clCP.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tblEnvio.AddCell(clNombre);
                tblEnvio.AddCell(cleMail);
                tblEnvio.AddCell(clDireccion);
                tblEnvio.AddCell(clCiudad);
                tblEnvio.AddCell(clEstado);
                tblEnvio.AddCell(clMunicipio);
                tblEnvio.AddCell(clCP);

                doc.Add(tblEnvio);
                doc.Add(Chunk.NEWLINE);

                // Escribimos el encabezamiento en el documento
                doc.Add(new Paragraph("Detalle de transacción # " + _lstTicket[0].Transaccion));
                //doc.Add(Chunk.NEWLINE);

                // Creamos una tabla que contendrá el nombre, apellido y país
                // de nuestros visitante.
                PdfPTable tblPago = new PdfPTable(3);
                tblPago.WidthPercentage = 100;

                // Configuramos el título de las columnas de la tabla
                PdfPCell clFolio = new PdfPCell(new Phrase("Fólio", _standardFont));
                clFolio.BorderWidth = 0;
                clFolio.BorderWidthBottom = 0.75f;

                PdfPCell cleImporteTot = new PdfPCell(new Phrase("Importe Total", _standardFont));
                cleImporteTot.BorderWidth = 0;
                cleImporteTot.BorderWidthBottom = 0.75f;

                PdfPCell clFechaHora = new PdfPCell(new Phrase("Fecha y Hora", _standardFont));
                clFechaHora.BorderWidth = 0;
                clFechaHora.BorderWidthBottom = 0.75f;

                // Añadimos las celdas a la tabla
                tblPago.AddCell(clFolio);
                tblPago.AddCell(cleImporteTot);
                tblPago.AddCell(clFechaHora);

                // Llenamos la tabla con información
                clFolio = new PdfPCell(new Phrase(_lstTicket[0].Folio.ToString(), _standardFont));
                clFolio.BorderWidth = 0;

                cleImporteTot = new PdfPCell(new Phrase(_lstTicket[0].Importe.ToString(), _standardFont));
                cleImporteTot.BorderWidth = 0;

                clFechaHora = new PdfPCell(new Phrase(_lstTicket[0].Fecha.ToString(), _standardFont));
                clFechaHora.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tblPago.AddCell(clFolio);
                tblPago.AddCell(cleImporteTot);
                tblPago.AddCell(clFechaHora);

                doc.Add(tblPago);

                doc.Close();
                writer.Close();

                return _lstTicket[0].Folio.ToString(); //Retorna el numero de folio que se usa como nombre del archivo PDF generado
            }
            catch (Exception ex)
            {

                return string.Empty;
            }
        }

        private bool SendMail(string strFrom, string strTo, string strSubject, string strMsg, string AttachFileName)
        {
            try
            {
                // Create the mail message
                MailMessage objMailMsg = new MailMessage(strFrom, strTo);

                objMailMsg.BodyEncoding = Encoding.UTF8;
                objMailMsg.Subject = strSubject;
                objMailMsg.Body = strMsg;
                //Attachment at = new Attachment(Server.MapPath("~/Uploaded/txt.doc"));
                Attachment at = new Attachment(Server.MapPath("~/Comprobantes/" + AttachFileName));
                objMailMsg.Attachments.Add(at);
                objMailMsg.Priority = MailPriority.High;
                objMailMsg.IsBodyHtml = true;

                //prepare to send mail via SMTP transport
                SmtpClient objSMTPClient = new SmtpClient();
                objSMTPClient.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;

                //smtp.Host = _Smtp;
                //smtp.Port = _Port;
                //smtp.EnableSsl = true;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword);

                objSMTPClient.Send(objMailMsg);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendEmailToCustomer(string emailId, string strSubject, string strMsgBody, string AttachFileName)
        {
            try
            {
                string _FromEmailAddress = CryptoEncription.DecryptString(key, ConfigurationManager.AppSettings["Sender"].ToString());
                string _FromContresena = CryptoEncription.DecryptString(key, ConfigurationManager.AppSettings["PswSender"].ToString());
                string _Smtp = CryptoEncription.DecryptString(key, ConfigurationManager.AppSettings["SMTP"].ToString());
                int _Port = int.Parse(ConfigurationManager.AppSettings["PORT"].ToString());

                var fromMail = new MailAddress(_FromEmailAddress, "Frutas y Vegetales Deshidratados Tienda online - Detalle de Compra"); // set your email  
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
                //Message.Body = "<br/> Gracias por registrarse en su Tienda SaniFrut." +
                //               "<br/> Le damos una cordial Bienvenida y no olvide enviarnos sus sugerencias." +
                //               "<br/> felices compras.";
                Message.Body = strMsgBody;
                Message.IsBodyHtml = true;

                //Attachment at = new Attachment(Server.MapPath("~/Uploaded/txt.doc"));
                Attachment at = new Attachment(Server.MapPath("~/Comprobantes/" + AttachFileName));
                Message.Attachments.Add(at);
                Message.Priority = MailPriority.High;

                smtp.Send(Message);
                return true;
            }
            catch (Exception ex)
            {
                string _errm = ex.Message;
                return false;
            }
        }
        #endregion
    }
}