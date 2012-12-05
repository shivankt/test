
namespace Common.Util.RegEx
{
    public class RegExPattren
    {
        public const string EMAIL = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public const string EMAIL_ALLOW_EMPTY = @"(^$)|(^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$)";
        public const string US_ZIP = @"\d{5}";
        public const string US_PHONE = @"(\d{10})|(((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4})";
        public const string US_PHONE_ALLOW_EMPTY = @"(^$)|(\d{10})|(((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4})";
        public const string DROP_DOWN_SELECTED = @"^[a-zA-Z1-9]";//Check if value is selected, 0 mean no value selected.
        public const string DYNAMIC_FIELD = @"(\{{2}).*?(\}{2})";
        public const string CSV_LINE = @"(?m)^((?("")""(?<quote>)|(?<=(^|,)))(?<field>(?(quote)(""""|[^""]|""(?!,))|[^,])*?)(?(quote)""(?<-quote>))(?($)|,))+";
        public const string WEB_URL = "^(https?://)"
       + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" //user@
       + @"(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP- 199.194.52.184
       + "|" // allows either IP or domain
       + @"([0-9a-z_!~*'()-]+\.)*" // tertiary domain(s)- www.
       + @"([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\." // second level domain
       + "[a-z]{2,6})" // first level domain- .com or .museum
       + "(:[0-9]{1,4})?" // port number- :80
       + "((/?)|" // a slash isn't required if there is no file name
       + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
    }

}
