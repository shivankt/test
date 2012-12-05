﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
namespace Common.Util
{
    public static class HttpRequestExtensions
    {
        public static string DomainName(this HttpRequestBase rq)
        {
            return DomainName(rq.ServerVariables["SERVER_PORT"], rq.ServerVariables["SERVER_PORT_SECURE"], rq.ServerVariables["SERVER_NAME"], rq.ApplicationPath);
        }

        public static string DomainName(this HttpRequest rq)
        {
            return DomainName(rq.ServerVariables["SERVER_PORT"], rq.ServerVariables["SERVER_PORT_SECURE"], rq.ServerVariables["SERVER_NAME"], rq.ApplicationPath);
        }
        // Get server name
        static string DomainName(string Port, string Protocol, string serverName, string appPath)
        {

            if (Port == null || Port == "80" || Port == "443")
                Port = "";
            else
                Port = ":" + Port;

            if (Protocol == null || Protocol == "0")
                Protocol = "http://";
            else
                Protocol = "https://";

            if (appPath == "/")
                appPath = "";

            string sOut = Protocol + serverName + Port;// +appPath;
            return sOut;
        }
    }
}
