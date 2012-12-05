using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Util.Models
{
    public class Pager
    {
        #region Fields and Properties

        public int PageIndex { get; set; }

        /// <summary>If set as zero, mean pull all records</summary>
        public int PageSize { get; set; }

        public string OrderBy { get; set; }

        public bool IsAsc { get; set; }

        string _urlRef;
        public string PagingUrl
        {
            get
            {
                return _urlRef.Replace("&amp;", "&");
            }
            set
            {
                if (System.Web.HttpContext.Current != null)
                    _urlRef = System.Web.HttpContext.Current.Server.HtmlEncode(value);
            }
        }

        public string RenderToID { get; set; }

        public int TotalItems { get; set; }

        /// <summary>Get property</summary>
        public int TotalPages
        {
            get
            {
                if (this.PageSize > 0)
                    return (int)Math.Ceiling((decimal)this.TotalItems / this.PageSize);
                return 1;
            }
        }

        public int FirstResult
        {
            get
            {
                if (PageSize == 0)
                    return 0;

                return ((PageIndex - 1) * PageSize);
            }
        }

        #endregion

        public Pager()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = string.Empty;
            IsAsc = true;
            PagingUrl = string.Empty;
        }

        public string PagerHtml(bool showItemCountDll = true, bool isPagerAjaxEnabled = true, string renderContentToID = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(PagerHtml(showItemCountDll, string.Empty));
            sb.Append(RenderPagerJSScript(this, showItemCountDll, isPagerAjaxEnabled, renderContentToID));
            return sb.ToString();
        }

        string PagerHtml(bool showItemCountDll, string parentID)
        {
            RenderToID = parentID;

            string linkFormat = "<li><a href=\"{0}\" onclick=\"return Navigate(this,'" + RenderToID + "');\" class=\"{2}\"> {1} </a></li>";

            if (this.TotalPages <= 0)
                return string.Empty;

            bool isVisiblePrev = false;
            bool isVisibleNext = false;
            List<string> numbers = new List<string>();
            if (this.TotalItems > 0 && this.PageIndex > 0)
            {
                int iTotalPages = this.TotalPages;
                int iCurrentPage = this.PageIndex;
                if (iTotalPages > 0)
                {
                    // manage 'Previous' lable
                    if (iCurrentPage == 1)
                        isVisiblePrev = false;
                    else
                        isVisiblePrev = true;
                    // manage 'Next' label
                    if (iCurrentPage == iTotalPages)
                        isVisibleNext = false;
                    else
                        isVisibleNext = true;

                    int iPageStart, iPageEnd;
                    if (iCurrentPage % 5 == 0)
                    {
                        iPageStart = iCurrentPage - 4;
                        iPageEnd = iCurrentPage;
                    }
                    else
                    {
                        iPageStart = iCurrentPage - (iCurrentPage % 5) + 1;
                        iPageEnd = iPageStart + 4;
                    }
                    if (iPageEnd > iTotalPages)
                    {
                        iPageEnd = iTotalPages;
                    }
                    while (iPageStart <= iPageEnd)
                    {
                        if (iPageStart == iCurrentPage)
                            numbers.Add(string.Format(linkFormat, this.GetUrl(iPageStart, this.PageSize, this.OrderBy), "<b>" + iPageStart + "</b>", "Active"));
                        else
                            numbers.Add(string.Format(linkFormat, this.GetUrl(iPageStart, this.PageSize, this.OrderBy), iPageStart, string.Empty));
                        iPageStart++;
                    }

                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"PagerWrapper\">");
            sb.Append("<div  class=\"PagerLeft\">");
            if (showItemCountDll)
            {
                sb.Append("Show Items:");
                sb.Append("<select onchange=\"ChangePageSize(this,'" + GetUrl("1", "{{0}}", OrderBy, IsAsc) + "','" + RenderToID + "')\">");
                sb.Append(this.GetPageSize_DDLOptions());
                sb.Append("</select>");
            }
            sb.Append("</div>");
            sb.Append("<div class=\"PagerRight\">");
            sb.Append("<ul class=\"LiFL\">");
            //sb.Append("<li>Page: </li>");

            if (isVisiblePrev)
                //sb.AppendFormat(linkFormat, this.GetUrl(this.PageIndex - 1, this.PageSize, this.OrderBy), "&lt; Previous ", string.Empty);
                sb.AppendFormat(linkFormat, this.GetUrl(this.PageIndex - 1, this.PageSize, this.OrderBy), "&lt; ", string.Empty);

            foreach (var item in numbers)
                sb.Append(item);

            if (isVisibleNext)
               // sb.AppendFormat(linkFormat, this.GetUrl(this.PageIndex + 1, this.PageSize, this.OrderBy), " Next &gt;", string.Empty);
               sb.AppendFormat(linkFormat, this.GetUrl(this.PageIndex + 1, this.PageSize, this.OrderBy), " &gt;", string.Empty);

            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("</div>");
            return sb.ToString();
        }

        public string GetUrl()
        {
            return GetUrl(this.PageIndex.ToString(), this.PageSize.ToString(), this.OrderBy.ToString(), this.IsAsc);
        }

        public string GetUrl(string orderBy)
        {
            return GetUrl(this.PageIndex.ToString(), this.PageSize.ToString(), orderBy, !this.IsAsc);
        }

        public string SortByLink(string linkText, string dbColumnName, string renderContentToID = "")
        {
            return string.Format("<a href=\"{0}\" onclick=\"return SortData(this,'{2}')\" >{1}</a>"
                                , GetUrl(this.PageIndex.ToString(), this.PageSize.ToString(), dbColumnName, !this.IsAsc)
                                , linkText
                                , renderContentToID
                                );
        }

        public string GetUrl(int pageIndex, int pageSize, string orderBy)
        {
            return GetUrl(pageIndex.ToString(), pageSize.ToString(), orderBy, this.IsAsc);
        }

        public string GetUrl(string pageIndex, string pageSize, string orderBy, bool isAsc)
        {
            return this.PagingUrl + (this.PagingUrl.Contains("?") ? "&" : "?")
                + "PageIndex=" + pageIndex
                + "&PageSize=" + pageSize
                + (!string.IsNullOrEmpty(orderBy) ? "&OrderBy=" + orderBy + "&IsAsc=" + isAsc.ToString() : "");
        }

        string GetPageSize_DDLOptions()
        {
            int[] pageSize = new int[] { 0, 5, 10, 15, 20, 25, 30, 40, 50 };
            string format = "<option value=\"{0}\">{1}</option>";
            string format2 = "<option value=\"{0}\" selected=\"selected\">{1}</option>";
            StringBuilder sb = new StringBuilder();
            foreach (var item in pageSize)
            {
                if (this.PageSize == item)
                {
                    if (item == 0)
                        sb.AppendFormat(format2, item, "--All--");
                    else
                        sb.AppendFormat(format2, item, item);
                }
                else
                {
                    if (item == 0)
                        sb.AppendFormat(format, item, "--All--");
                    else
                        sb.AppendFormat(format, item, item);
                }
            }
            return sb.ToString();
        }

        string RenderPagerJSScript(Pager pager, bool showItemCountDll, bool isPagerAjaxEnabled, string renderContentToID)
        {
            StringBuilder sbAjax = new StringBuilder();

            if (isPagerAjaxEnabled)
            {
                sbAjax.Append("$.get(url, function(data) {");
                sbAjax.Append("if(parent){");
                sbAjax.Append("$('#'+ parent).html(data);");
                sbAjax.Append("}");
                sbAjax.Append("else{");

                if (!string.IsNullOrEmpty(renderContentToID))
                    sbAjax.Append("$(\"#" + renderContentToID + "\").html(data);");
                else
                    sbAjax.Append("$(ele).parents('.clsTblWrapper').html(data);");

                sbAjax.Append("}");
                sbAjax.Append("});");//end of get
                sbAjax.Append("return false;");
            }
            else
            {
                sbAjax.Append("window.location = url;");
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<script type=\"text/javascript\">");

            if (showItemCountDll && pager != null) //Script to handel page size change
            {
                //Page size ddl fn
                sb.Append("function ChangePageSize(ele,url,parent){");
                sb.Append("url = url.replace(\"{{0}}\", $(ele).val());");
                sb.Append(sbAjax.ToString());
                sb.Append("}");
            }

            //Navigate fn
            sb.Append("function Navigate(ele,parent){");
            sb.Append("var url = $(ele).attr(\"href\");");
            sb.Append(sbAjax.ToString());
            sb.Append("}");

            //Sort the columns
            sb.Append("function SortData(ele, parent){");
            sb.Append("var url = $(ele).attr(\"href\");");
            if (!string.IsNullOrEmpty(renderContentToID))
                sb.AppendFormat("var parent = '{0}';", renderContentToID);
            sb.Append(sbAjax.ToString());
            sb.Append("}");


            sb.Append("</script>");
            sbAjax = null;
            return sb.ToString();
        }
    }

}
