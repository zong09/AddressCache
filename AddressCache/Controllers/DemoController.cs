using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;

namespace AddressCache.Controllers
{
    public class DemoController : Controller
    {
        // GET: Demo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddressAdd(string ipAddress,string hostName)
        {
            InetAddress objiNet = new InetAddress();
            objiNet.HostName = hostName;
            objiNet.IPaddress = ipAddress;

            AddressCache objAddress = new AddressCache(10, TimeUnit.MINUTES);
            objAddress.Add(objiNet);

            OrderedDictionary dicAddress = objAddress.GetAll();
            return JsonMessage(true, "000", RenderView(ControllerContext, "~/Views/Demo/_gridCache.cshtml", true, dicAddress));
        }

        public ActionResult AddressRemove(string ipAddress, string hostName)
        {
            InetAddress objiNet = new InetAddress();
            objiNet.HostName = hostName;
            objiNet.IPaddress = ipAddress;

            AddressCache objAddress = new AddressCache(10, TimeUnit.MINUTES);
            objAddress.Remove(objiNet);

            OrderedDictionary dicAddress = objAddress.GetAll();
            return JsonMessage(true, "000", RenderView(ControllerContext, "~/Views/Demo/_gridCache.cshtml", true, dicAddress));
        }

        public ActionResult AddressPeek()
        {
            AddressCache objAddress = new AddressCache(10, TimeUnit.MINUTES);
            OrderedDictionary peek = objAddress.Peek();
            return JsonMessage(true, "000", RenderView(ControllerContext, "~/Views/Demo/_gridCache.cshtml", true, peek));
        }

        public ActionResult AddressTake()
        {
            AddressCache objAddress = new AddressCache(10, TimeUnit.MINUTES);
            OrderedDictionary peek = objAddress.Take();
            return JsonMessage(true, "000", RenderView(ControllerContext, "~/Views/Demo/_gridCache.cshtml", true, peek));
        }

        public ActionResult AddressGetAll()
        {
            AddressCache objAddress = new AddressCache(10, TimeUnit.MINUTES);
            OrderedDictionary dicAddress = objAddress.GetAll();
            return JsonMessage(true, "000", RenderView(ControllerContext, "~/Views/Demo/_gridCache.cshtml", true, dicAddress));
        }

        public static string RenderView(ControllerContext context, string viewPath, bool isPartial, object model = null)
        {
            context.Controller.ViewData.Model = model;
            using (System.IO.StringWriter writer = new System.IO.StringWriter())
            {
                ViewEngineResult viewResult = null;
                if (isPartial)
                    viewResult = ViewEngines.Engines.FindPartialView(context, viewPath);
                else
                    viewResult = ViewEngines.Engines.FindView(context, viewPath, null);

                ViewContext viewContext = new ViewContext(context, viewResult.View, context.Controller.ViewData, context.Controller.TempData, writer);
                viewResult.View.Render(viewContext, writer);

                return writer.GetStringBuilder().ToString();
            }
        }

        public static JsonResult JsonMessage(bool success, string code, string message)
        {
            JsonResult json = new JsonResult();
            json.Data = new { Success = success, Code = code, Message = message };
            return json;
        }
    }
}