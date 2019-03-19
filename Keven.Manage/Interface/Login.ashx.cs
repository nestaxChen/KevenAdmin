using Keven.Manage.Models;
using LinqKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Keven.Manage.Interface
{
    /// <summary>
    /// Login 的摘要说明
    /// </summary>
    public class Login : BaseInterface
    {
        JavaScriptSerializer js = new JavaScriptSerializer();

        public override void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
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
                case "list":
                    ListUser(context);
                    break;

                default:
                    context.Response.Write(js.Serialize(BaseModels.ErrorLogin("未指定具体接口！")));
                    return;
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ListUser(HttpContext context)
        {
            //context.Response.ContentType = "application/json";

            int pageCount = string.IsNullOrEmpty(Request("pageCount")) ? 10 : Request("pageCount").ToInt();
            int pageIndex = string.IsNullOrEmpty(Request("pageIndex")) ? 1 : Request("pageIndex").ToInt();
            int total = 0;

            string loginName = Request("loginName");
            string province = Request("province");


            var predicate = PredicateBuilder.New<Model.TT_User>(true);
            if (!string.IsNullOrEmpty(loginName))
            {
                predicate = predicate.And(t => t.LoginName.Contains(loginName) );
            }
            if (!string.IsNullOrEmpty(province))
            {
                predicate = predicate.And(t => t.Province == province);
            }

            BLL.TtUsersBll bll = new BLL.TtUsersBll();
            List<Model.TT_User> allUser = bll.QueryByPage(pageIndex, pageCount, out total, predicate, t => t.Id, false);

            List<Hashtable> ret = new List<Hashtable>();

            if (allUser != null)
            {
                Pager pager = new Pager();
                pager.total = total;
                pager.pageCount = pageCount;
                pager.pageIndex = pageIndex;

                foreach (var user in allUser)
                {
                    Hashtable ht = new Hashtable();
                    ht["id"] = user.Id;
                    ht["loginName"] = user.LoginName;
                    ht["password"] = user.Password;
                    ht["mobile"] = user.Mobile;
                    ht["province"] = user.Province;
                    ret.Add(ht);
                }

                context.Response.Write(js.Serialize(BaseModels.OK("查询成功！", ret, pager)));
                return;
            }

            context.Response.Write(js.Serialize(BaseModels.Error("查询失败！")));
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void AddUser(HttpContext context)
        {
            //context.Response.ContentType = "application/json";

            string loginName = Request("loginName");
            string password = Request("password");
            string mobile = Request("mobile");
            string province = Request("province");


            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
            {
                context.Response.Write(js.Serialize(BaseModels.Error("账号和密码不能为空！")));
                return;
            }

            BLL.TtUsersBll bll = new BLL.TtUsersBll();
            Model.TT_User user = new Model.TT_User();

            user.LoginName = loginName;
            user.Password = password;
            user.Mobile = mobile;
            user.Province = province;

            bll.Add(user);

                context.Response.Write(js.Serialize(BaseModels.OK("添加成功！")));
            return;
        }

        /// <summary>
        /// 登录系统
        /// </summary>
        /// <param name="context"></param>
        public void LoginSystem(HttpContext context)
        {
            //context.Response.ContentType = "application/json";

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