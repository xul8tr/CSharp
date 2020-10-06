using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JQAjax.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData(string alma)
        {

            return Json(new valasz{Bekuldott = alma, Fixadat="Hello", Id=5}, JsonRequestBehavior.AllowGet);
        }
    }

    public class valasz
    {
        public int Id { get; set; }
        public string Fixadat { get; set; }
        public string Bekuldott { get; set; }

    }
}
