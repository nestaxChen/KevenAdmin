using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Keven.BLL;
using Keven.Model;
using Keven.Manage.Models;

namespace Keven.Manage.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        { 
            if (!BaseModels.IsLogin())
            {
                return Redirect("~/Login/");
            }
            UR_USERS user = BaseModels.CurrentUser();

            return View(user);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        public ActionResult Test()
        {
            BLL.UrUsersBll bll = new BLL.UrUsersBll();

            //新增测试：
            Model.UR_USERS model = new Model.UR_USERS()
            {
                USER_NAME = "测试1111111"
            };
            bll.Add(model);

            //修改
            //Company model1 = new Company()
            //{
            //    cID = 8,
            //    CName = "测试22221"
            //};

            //cbll.Edit(model1, new string[] { "CName" });


            //删除
            var model2 = bll.Query(c => c.USER_ID == 8).FirstOrDefault();
            bll.Delete(model2, true);//执行sql语句 打开关闭了一次ado.net链接

            //统一将上面的新增，编辑，删除分别生成insert,update,delete语句一次性发送给数据库执行
            //bll.SaveChanges();



            return View(bll.Query(c => true));

        }

    }
}