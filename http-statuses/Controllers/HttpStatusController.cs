using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using System.Net.Http;
using System.Net;
using AngleSharp;
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
            if (IsValidStatusCode(statusCode))
            {
                return new StatusCodeResult(statusCode);
            }
            return new RedirectResult("/");
        }

        [HttpGet]
        [Route("about")]
        public ActionResult About()
        {
            ViewBag.Title = "About";
            ViewBag.isValidStatusCode = false;
            return View();
        }

        [HttpGet]
        [Route("about/{statusCode:int}")]
        public ActionResult About(int statusCode)
        {
            if (!IsValidStatusCode(statusCode))
            {
                Response.Redirect("/about/");
            }

            ViewBag.isValidStatusCode = true;
            ViewBag.StatusCode = statusCode;
            return View();
        }


        [HttpGet]
        [Route("{text}")]
        public ActionResult GetStatus(string text)
        {
            return new JsonResult(text);
        }

        public bool IsValidStatusCode(int statusCode)
        {
            return Enum.IsDefined(typeof(HttpStatusCode), statusCode);
        }

        public static async Task<string> GetStatusCodeInfo(int statusCode)
        {
            var client = new HttpClient();
            var url = $"https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/{statusCode}";
            var response = await client.GetAsync(url);
            var contents = response.Content;
            var result = await contents.ReadAsStringAsync();

            var context = BrowsingContext.New(Configuration.Default);
            var document = await context.OpenAsync(req => req.Content(result));

            var html = await context.OpenNewAsync();
            var statusCodeHtml = document.QuerySelector(".main-page-content");

            if (statusCodeHtml == null)
            {
                return @$"
                    <h1>Sorry, it looks like that status page is broken.</h1>
                    <a href='{url}' target='_blank'>{url}</a>
                ";
            }

            var selectors = new []
            {
                "h1",
                "#status",
                "#examples",
                "#specifications"
            };

            foreach (var selector in selectors)
            {
                var node = statusCodeHtml.QuerySelector(selector);
                var sibling = statusCodeHtml.QuerySelector($"{selector} + div");
                if (node != null)
                {
                    html.Body.AppendChild(node);
                }

                if (sibling != null)
                {
                    html.Body.AppendChild(sibling);
                }
            }

            foreach(IHtmlAnchorElement anchor in html.Body.QuerySelectorAll("a"))
            {
                if (anchor.Href.Contains("localhost"))
                {
                    anchor.OuterHtml = $"<span>{anchor.InnerHtml}</span>";
                }
            }

            return html.Body.InnerHtml;
        }
    }
}