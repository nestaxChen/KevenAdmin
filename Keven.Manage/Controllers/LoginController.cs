using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Keven.BLL;
using Keven.Manage.Models;
using Keven.Model;
using Keven.Common;

namespace Keven.Manage.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            if (BaseModels.IsLogin())
                return Redirect("~/Home/");

            return View();
        }

        [HttpPost]
        public ActionResult Index(string userName,string userPwd)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userPwd))
            {
                ViewBag.Success = false;
                ViewBag.Message = "账号或密码不能为空！";
                return View();
            }
            userPwd = userPwd.ToSHA1();
            UrUsersBll bll = new UrUsersBll();
            UR_USERS user = bll.Query(t => t.USER_LOGIN_NAME == userName && t.USER_LOGIN_PASSWD == userPwd).FirstOrDefault();

            if (user == null)
            {
                ViewBag.Success = false;
                ViewBag.Message = "账号或密码错误！";
                return View();
            }
            
            user.USER_LOGIN_DATE = DateTime.Now;
            user.USER_UNUSED1 = Guid.NewGuid().ToString("n");
            bll.Update(user);
            //写入cookie
            HttpCookie cookies = new HttpCookie("UserToken");
            cookies["token"] = user.USER_UNUSED1;
            cookies["userName"] = HttpUtility.UrlEncode(user.USER_NAME, System.Text.Encoding.UTF8); 
            cookies.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(cookies);
            
            return Redirect("~/Home/");
        }


        /// <summary>
        /// 退出登录，清空cookie
        /// </summary>
        /// <returns></returns>
        public ActionResult Quit()
        {
            HttpCookie cookies = new HttpCookie("UserToken");
            cookies.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookies);

            return Json(BaseModels.OK("成功！"));
        }
    }
}