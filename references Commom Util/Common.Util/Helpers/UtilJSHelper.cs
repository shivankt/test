
namespace Common.Util.Helpers
{
    public class UtilJSHelper
    {
        static string scriptFormat = " <script type=\"text/javascript\">{0}</script>";

        public static string ScriptWindowLocation(string location)
        {
            location = System.Web.HttpContext.Current.Server.HtmlEncode(location);
            return string.Format(scriptFormat, string.Format("window.location='{0}'", location));
        }
    }
}
