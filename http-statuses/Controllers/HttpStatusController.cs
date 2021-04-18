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
            return View();
        }

        [HttpGet]
        [Route("{statusCode:int}")]
        public ActionResult GetStatus(int statusCode)
        {
            return Enum.IsDefined(typeof(HttpStatusCode), statusCode)
                ? new StatusCodeResult(statusCode)
                : new StatusCodeResult(404);
        }

        [HttpGet]
        [Route("about/{statusCode:int?}")]
        public ActionResult About(int? statusCode)
        {
            var isValidStatusCode = statusCode != null && Enum.IsDefined(typeof(HttpStatusCode), statusCode);
            ViewBag.Title = isValidStatusCode ? $"About the {statusCode} Status Code" : "About";
            ViewBag.isValidStatusCode = isValidStatusCode;
            ViewBag.StatusCode = statusCode;
            return View();
        }

        [HttpGet]
        [Route("{text}")]
        public ActionResult GetStatus(string text)
        {
            return new JsonResult(text);
        }

        public static string GetStatusCodeInfo(int statusCode)
        {
            return statusCode.ToString();
        }
    }
}