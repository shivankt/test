using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Common.Util
{
    public static class MVCExtensions
    {
        /// <summary>Get the ModelState errors</summary>
        public static string GetErrors(this ModelStateDictionary modelState, string seprator = "<br/>")
        {
            return string.Join(seprator, modelState.Where(p => p.Value.Errors.Count > 0).Select(p => p.Value.Errors[0].ErrorMessage).ToArray());
        }

        ///<summary>Render view as string</summary>
        ///http://stackoverflow.com/questions/609772/rendering-a-view-on-the-fly
        public static string UtilRenderView(this ViewResult vr, ControllerContext cc)
        {
            string viewName = string.IsNullOrEmpty(vr.ViewName) ? cc.RouteData.Values["action"].ToString() : vr.ViewName;
            string master = vr.MasterName;

            ViewDataDictionary viewData = cc.Controller.ViewData;

            if (vr.ViewData.Model != null)
            {
                viewData.Model = vr.ViewData.Model;
            }

            System.IO.StringWriter output = new System.IO.StringWriter();

            ViewEngineResult result = ViewEngines.Engines.FindView(cc, viewName, master);
            ViewContext viewContext = new ViewContext(cc, result.View, viewData, cc.Controller.TempData, output);

            result.View.Render(viewContext, output);
            result.ViewEngine.ReleaseView(cc, result.View);

            return output.ToString();
        }

        public static string UtilRenderView(this ActionResult actionResult, ControllerContext cc)
        {
            return ((ViewResult)actionResult).UtilRenderView(cc);
        }

        public static string UtilRenderView(this JsonResult jr)
        {
            System.Text.StringBuilder output = new System.Text.StringBuilder();

            if (jr.Data != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.Serialize(jr.Data, output);
            }

            return output.ToString();
        }

        public static void CleanAllBut(this ModelStateDictionary modelState, params string[] includes)
        {
            modelState
              .Where(x => !includes
                 .Any(i => String.Compare(i, x.Key, true) == 0))
              .ToList()
              .ForEach(k => modelState.Remove(k));
        }

        public static void CleanAll(this ModelStateDictionary modelState, params string[] includes)
        {
            modelState
              .Where(x => includes
                 .Any(i => String.Compare(i, x.Key, true) == 0))
              .ToList()
              .ForEach(k => modelState.Remove(k));
        }
    }
}