using Keven.Manage.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Keven.Manage.Interface
{
    /// <summary>
    /// User 的摘要说明
    /// </summary>
    public class User : BaseInterface
    {

        JavaScriptSerializer js = new JavaScriptSerializer();

        public override void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/Json";
            HttpRequest request = context.Request;

            string act = Request("act").ToLower();

            switch (act.ToLower())
            {
                case "login":
                    LoginSystem(context);
                    break;
                case "add":
                    AddUser(context);
                    break;
                case "edit":
                    EditUser(context);
                    break;
                case "list":
                    ListUser(context);
                    break;
                case "get":
                    GetUser(context);
                    break;
                case "del":
                    DelUser(context);
                    break;
                default:
                    context.Response.Write(js.Serialize(BaseModels.ErrorLogin("未指定具体接口！")));
                    return;
            }
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="context"></param>
        public void ListUser(HttpContext context)
        {
            //context.Response.ContentType = "application/json";
            int pageCount = string.IsNullOrEmpty(Request("pageCount")) ? 10 : Request("pageCount").ToInt();
            int pageIndex = string.IsNullOrEmpty(Request("pageIndex")) ? 1 : Request("pageIndex").ToInt();
            int total = 0;

            string loginName = Request("loginName");

            List<Hashtable> ret = new List<Hashtable>();

            for (int i = 0; i < 10; i++)
            {
                Hashtable ht = new Hashtable();
                ht["id"] = (pageIndex - 1) * pageCount + i + 1;
                ht["loginName"] = string.IsNullOrEmpty(loginName) ? "张三" : loginName;
                ht["password"] = "123456";
                ht["mobile"] = "15888888888";
                ht["province"] = "杭州";
                ret.Add(ht);
            }

            Pager pager = new Pager();
            pager.total = 88;
            pager.pageCount = pageCount;
            pager.pageIndex = pageIndex;
            context.Response.Write(js.Serialize(BaseModels.OK("查询成功！", ret, pager)));
            return;
        }


        public void DelUser(HttpContext context)
        {
            //context.Response.ContentType = "application/json";
            string token = Request("token");

            string id = Request("id");


            if (string.IsNullOrEmpty(token))
            {
                context.Response.Write(js.Serialize(BaseModels.ErrorLogin("登录已失效！")));
                return;
            }

            if (string.IsNullOrEmpty(id))
            {
                context.Response.Write(js.Serialize(BaseModels.Error("编码不能为空！")));
                return;
            }

            context.Response.Write(js.Serialize(BaseModels.OK("删除成功！")));
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void GetUser(HttpContext context)
        {
            //context.Response.ContentType = "application/json";
            string token = Request("token");

            string id = Request("id");


            if (string.IsNullOrEmpty(token))
            {
                context.Response.Write(js.Serialize(BaseModels.ErrorLogin("登录已失效！")));
                return;
            }

            if (string.IsNullOrEmpty(id))
            {
                context.Response.Write(js.Serialize(BaseModels.Error("编码不能为空！")));
                return;
            }

            Hashtable ht = new Hashtable();
            ht["Id"] = id;
            ht["loginName"] = "张双";
            ht["password"] = "123456";
            ht["mobile"] = "15888888888";
            ht["province"] = "杭州";

            context.Response.Write(js.Serialize(BaseModels.OK("查询成功！", ht)));
            return;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        public void AddUser(HttpContext context)
        {
            //context.Response.ContentType = "application/json";
            string token = Request("token");

            string loginName = Request("loginName");
            string password = Request("password");
            string mobile = Request("mobile");
            string province = Request("province");

            if (string.IsNullOrEmpty(token))
            {
                context.Response.Write(js.Serialize(BaseModels.ErrorLogin("登录已失效！")));
                return;
            }

            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
            {
                context.Response.Write(js.Serialize(BaseModels.Error("账号和密码不能为空！")));
                return;
            }

            context.Response.Write(js.Serialize(BaseModels.OK("添加成功！")));
            return;
        }


        public void EditUser(HttpContext context)
        {
            //context.Response.ContentType = "application/json";
            string token = Request("token");

            string loginName = Request("loginName");
            string password = Request("password");
            string mobile = Request("mobile");
            string province = Request("province");

            if (string.IsNullOrEmpty(token))
            {
                context.Response.Write(js.Serialize(BaseModels.ErrorLogin("登录已失效！")));
                return;
            }

            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
            {
                context.Response.Write(js.Serialize(BaseModels.Error("账号和密码不能为空！")));
                return;
            }

            context.Response.Write(js.Serialize(BaseModels.OK("修改成功！")));
            return;
        }


        /// <summary>
        /// 登录系统
        /// </summary>
        /// <param name="context"></param>
        public void LoginSystem(HttpContext context)
        {
            string userName = Request("userName");
            string password = Request("password");

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                context.Response.Write(js.Serialize(BaseModels.Error("账号和密码不能为空！")));
                return;
            }
            Hashtable ht = new Hashtable();
            ht["name"] = "张三";
            ht["token"] = Guid.NewGuid().ToString("n");

            context.Response.Write(js.Serialize(BaseModels.OK("登录成功！", ht)));
            return;
        }
    }
}