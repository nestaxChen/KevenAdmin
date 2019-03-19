using Keven.Manage.Models;
using Keven.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Keven.Manage
{
    public class AdminAuthorization : FilterAttribute, IAuthorizationFilter
    {
        public string Method { get; set; }
        /// <summary>
        /// 权限等级
        /// </summary>
        public int Level { get; set; }
        public string Admins { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!BaseModels.IsLogin())
            {
                if (Method == "json")
                {
                    filterContext.Result = BaseModels.Result.Error("请先登录！");
                    return;
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Login");
                    return;
                }
            }
            UR_USERS currentAdmin = BaseModels.CurrentUser();

            if (Level > 0)
            {
                //角色
                if (currentAdmin.USER_PART.ToInt() < Level)
                {
                    if (Method == "json")
                    {
                        filterContext.Result = BaseModels.Result.Error("您无权操作！");
                        return;
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("~/?message=" + filterContext.HttpContext.Server.UrlEncode("您无权操作"));
                        return;
                    }
                }
            }
            if (!string.IsNullOrEmpty(Admins))
            {
                Admins += "sa";
                List<string> adminlist = new List<string>(Admins.Split(','));
                if (!adminlist.Contains(currentAdmin.USER_NAME))
                {
                    if (Method == "json")
                    {
                        filterContext.Result = BaseModels.Result.Error("您无权操作！");
                        return;
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("~/?message=" + filterContext.HttpContext.Server.UrlEncode("您无权操作"));
                        return;
                    }
                }
            }
        }
    }
}