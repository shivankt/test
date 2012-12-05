using System.Collections.Generic;
using System.Linq;

namespace Common.Util.Models
{
    public class QueryStatus
    {
        public enum Statuses
        {
            Success,
            Error
        }

        public Statuses Status { get; set; }

        public Dictionary<string, string> ListMessage { get; set; }

        public QueryStatus()
        {
            Status = Statuses.Error;
            ListMessage = new Dictionary<string, string>();
        }

        /// <summary>default seprator is html element br </summary>
        public override string ToString()
        {
            return GetErros("<br/>");
        }

        public string ToString(string seprator)
        {
            return GetErros(seprator);
        }

        string GetErros(string seprator)
        {
            return string.Join(seprator, ListMessage.Select(p => p.Value).ToArray<string>());
        }

    }
}
