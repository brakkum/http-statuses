using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;

namespace http_statuses.Controllers
{
    public class HttpStatusController : Controller
    {
        [HttpGet]
        [Route("/")]
        public ActionResult Index()
        {
            var codes = new Dictionary<int, string>();
            foreach (var code in Enum.GetValues(typeof(HttpStatusCode)))
            {
                if (codes.ContainsKey(code.GetHashCode()))
                {
                    continue;
                }
                codes.Add(code.GetHashCode(), code.ToString());
            }

            ViewBag.StatusCodes = codes;
            return View();
        }

        [HttpGet]
        [Route("{statusCode:int}")]
        public ActionResult GetStatus(int statusCode)
        {
            return System.Enum.IsDefined(typeof(HttpStatusCode), statusCode)
                ? new StatusCodeResult(statusCode)
                : new StatusCodeResult(404);
        }

        [HttpGet]
        [Route("{text}")]
        public ActionResult GetStatus(string text)
        {
            return new JsonResult(text);
        }
    }
}