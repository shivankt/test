using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
namespace Common.Util
{
    /*Author: Ankit*/

    /// <summary>Attribute to keep track for the url state</summary>
    public class UtilUrlStateAttribute : ActionFilterAttribute
    {
        public string ControlerName { get; set; }
        public string ActionName { get; set; }
        public UtilUrlStateAttribute(string controlerName, string actionName)
        {
            ControlerName = controlerName;
            ActionName = actionName;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string url = filterContext.HttpContext.Request.Url.ToString();

            UrlStateSessionHelper.AddUrlState(ControlerName, ActionName, url);
        }
    }

    public static class UrlStateExtensions
    {
        public static string UtilGetUrlState(this System.Web.HttpRequest request, string controlerName, string actionName)
        {
            return UrlStateSessionHelper.GetUrlState(controlerName, actionName);
        }

        public static string UtilGetUrlState(this System.Web.HttpRequestBase request, string controlerName, string actionName)
        {
            return UrlStateSessionHelper.GetUrlState(controlerName, actionName);
        }
    }

    class UrlStateSessionHelper
    {
        const string sessionKey = "UrlStateSessions";

        static Dictionary<string, string> CollectionUrlState
        {
            get
            {
                if (HttpContext.Current.Session[sessionKey] == null)
                    HttpContext.Current.Session[sessionKey] = new Dictionary<string, string>();

                return (Dictionary<string, string>)HttpContext.Current.Session[sessionKey];

            }
        }

        public static void AddUrlState(string controlerName, string actionName, string url)
        {
            string key = string.Format("/{0}/{1}", controlerName, actionName);
            if (CollectionUrlState.ContainsKey(key))
            {
                CollectionUrlState[key] = url;
            }
            else
            {
                CollectionUrlState.Add(key, url);
            }
        }

        public static string GetUrlState(string controlerName, string actionName)
        {
            string key = string.Format("/{0}/{1}", controlerName, actionName);
            if (CollectionUrlState.ContainsKey(key))
            {
                return CollectionUrlState[key];
            }
            return key;
        }
    }
}
