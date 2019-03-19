using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public static class MvcHtmlExtension
    {
        public static void RouteTo(this RouteCollection r, string rule, object to, string[] namespaces)
        {
            r.MapRoute(Guid.NewGuid().ToString(), rule, to, namespaces);
        }
        public static void RouteTo(this RouteCollection r, string rule, string to, string[] namespaces)
        {
            string controller = "Home", action = "Index", query = "";
            int index = to.IndexOf("?");
            if (index >= 0)
            {
                query = to.Substring(index + 1);
                to = to.Substring(0, index);
            }
            if (!string.IsNullOrEmpty(to))
            {
                index = to.IndexOf("/");
                if (index > 0)
                {
                    action = to.Substring(index + 1);
                    controller = to.Substring(0, index);
                }
                else
                {
                    controller = to;
                }
            }

            r.MapRoute(Guid.NewGuid().ToString(), rule, new { controller = controller, action = action }, namespaces);
        }
        public static MvcHtmlString PageBar(this HtmlHelper htmlHelper, string urlFormate, string pageFormat,
         int currentPage, int pageCount, int total)
        {
            urlFormate = HttpContext.Current.Request.RawUrl;
            if (string.IsNullOrEmpty(urlFormate))
            {
                urlFormate = "?page=" + pageFormat;
            }
            else {
                urlFormate = System.Text.RegularExpressions.Regex.Replace(urlFormate, @"page\=(\d+)","page=" + pageFormat);
                if (urlFormate.IndexOf("=" + pageFormat) < 0) {
                    if (urlFormate.IndexOf("?") < 0)
                    {
                        urlFormate = urlFormate + "?page=" + pageFormat;
                    }
                    else
                    {
                        urlFormate = urlFormate + "&page=" + pageFormat;
                    }
                }
            }
            //获取总页数
            int totalPage = total / pageCount;
            totalPage = total % pageCount == 0 ? totalPage : (totalPage + 1);

            StringBuilder htmlString = new StringBuilder();
            htmlString.Append("<div class=\"pagination form-inline\">");
            htmlString.Append("<ul style=\"box-shadow:none;\">");
            htmlString.Append("<li><span class=\"lbl\">共有[ " + total + " ]条，[ " + totalPage + " ]页， 每页显示：[ " + pageCount + " ]条</span></li>");
            if (currentPage != 1)
            {
                htmlString.Append("<li><a href=\"" + urlFormate.Replace(pageFormat, "1") + "\">首页</a></li>");
            }
            if (currentPage > 1)
            {
                htmlString.Append("<li title='上一页'><a href=\"" + urlFormate.Replace(pageFormat, (currentPage - 1).ToString()) + "\"><i class=\"fa fa-fw fa-angle-double-left\"></i></a></li>");
            }
            else
            {
                htmlString.Append("<li class=\"disabled\"><a href=\"javascript:void(0)\"><i class=\"fa fa-fw fa-angle-double-left\"></i></a></li>");
            }

            int begin = currentPage - 2;
            begin = begin <= 0 ? 1 : begin;
            int end = begin + 5 - 1;
            end = end > totalPage ? totalPage : end;
            begin = end - 5 + 1;
            begin = begin <= 0 ? 1 : begin;
            for (int i = begin; i <= end && i <= totalPage; i++)
            {
                htmlString.Append("<li " + (i == currentPage ? " class=\"active\"" : "") + "  ><a href=\"" + (i == currentPage ? "javascript:void(0)" : urlFormate.Replace(pageFormat, (i).ToString())) + "\">" + i + "</a></li>");
            }
            if (currentPage < totalPage)
            {
                htmlString.Append("<li title='下一页'><a href=\"" + urlFormate.Replace(pageFormat, (currentPage + 1).ToString()) + "\"><i class=\"fa fa-fw fa-angle-double-right\"></i></a></li>");
            }
            else
            {
                htmlString.Append("<li class=\"disabled\"><a href=\"javascript:void(0)\"><i class=\"fa fa-fw fa-angle-double-right\"></i></a></li>");
            }
            if (currentPage != totalPage)
            {
                htmlString.Append("<li><a href=\"" + urlFormate.Replace(pageFormat, totalPage.ToString()) + "\">尾页</a></li>");
            }
            htmlString.Append("<li style=\"padding-left: 5px; padding-right: 5px;\"><input name=\"pagination_input\" style=\"width:50px;\" type=\"text\" placeholder=\"页码\"></li>");
            htmlString.Append("<li><button onclick=\"GoPage();return false;\" class=\"btn-sm btn-info\">GO</button></li>");
            htmlString.Append("</ul>");
            htmlString.Append("</div>");
            htmlString.Append("<script type=\"text/javascript\">");
            htmlString.Append("function GoPage()");
            htmlString.Append("{");
            htmlString.Append("        var pageInput = document.getElementsByName(\"pagination_input\");");
            htmlString.Append("        if (pageInput) {");
            htmlString.Append("            var page = pageInput[0].value;");
            htmlString.Append("            var p = parseInt(page);");
            htmlString.Append("            if (!isNaN(p) && p>=1 && p<=" + totalPage + ") {");
            htmlString.Append("                location.href = \"" + urlFormate.Replace(pageFormat, "\"+p+\"") + "\";");
            htmlString.Append("            }");
            htmlString.Append("        }");
            htmlString.Append("    }");
            htmlString.Append("</script>");
            MvcHtmlString str = new MvcHtmlString(htmlString.ToString());
            return str;
        }
    }
}