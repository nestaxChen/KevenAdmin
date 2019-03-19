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
    [AdminAuthorization(Level = (int)AdminModel.TmGroupEnum.商标经纪人)]
    public class ManageController : Controller
    {
        // GET: Manage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminList()
        {
            string keyword = Request["keyword"];
            ViewBag.Keyword = keyword;
            UrUsersBll bll = new UrUsersBll();
            List<UR_USERS> allUser = bll.Query(t => string.IsNullOrEmpty(keyword) ? true : t.USER_NAME.Contains(keyword)).ToList();
            return View(allUser);
        }

        /// <summary>
        /// 管理员详情
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminDetail()
        {
            UR_USERS user = BaseModels.CurrentUser();

            return View(user);
        }


        /// <summary>
        /// 管理员修改
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminEdit(int? id)
        {
            UR_USERS user = new UR_USERS();
            if (id.ToInt() > 0)
            {
                UrUsersBll bll = new UrUsersBll();
                user = bll.Query(t => t.USER_ID == id).FirstOrDefault();
            }

            ViewBag.Organization = new SysOrganizationBll().Query(t => true).ToDictionary(x => x.ORG_ID, x => x.ORG_NAME);
            ViewBag.Part = new UrPartBll().Query(t => true).ToDictionary(x => x.PART_ID, x => x.PART_NAME);

            return View(user);
        }

        [HttpPost]
        public ActionResult AdminEdit(UR_USERS model)
        {
            UrUsersBll bll = new UrUsersBll();
            UR_USERS user = bll.Query(t => t.USER_ID == model.USER_ID).FirstOrDefault();

            ViewBag.Organization = new SysOrganizationBll().Query(t => true).ToDictionary(x => x.ORG_ID, x => x.ORG_NAME);
            ViewBag.Part = new UrPartBll().Query(t => true).ToDictionary(x => x.PART_ID, x => x.PART_NAME);

            if (user == null)
            {
                user = model;
                user.USER_CREATE_DATE = DateTime.Now;
                bll.Add(user);
            }
            else
            {
                user.USER_NAME = model.USER_NAME;
                user.USER_ORG_ID = model.USER_ORG_ID;
                user.USER_LOGIN_NAME = model.USER_LOGIN_NAME;
                user.USER_PHONE = model.USER_PHONE;
                user.USER_MOBILE = model.USER_MOBILE;
                user.USER_EMAIL = model.USER_EMAIL;
                user.USER_WORK_NO = model.USER_WORK_NO;
                user.USER_SEX = model.USER_SEX;
                user.USER_TITLE = model.USER_TITLE;
                user.USER_ACTIVE = model.USER_ACTIVE;
                user.USER_ADMIN = model.USER_ADMIN;

                user.USER_UPDATE_DATE = DateTime.Now;
                bll.Update(user);
            }

            ViewBag.Success = true;
            ViewBag.Message = "修改成功！";

            return View(user);
        }

        [HttpPost]
        public ActionResult AdminDel(int id)
        {
            UrUsersBll bll = new UrUsersBll();
            UR_USERS model = bll.Query(t => t.USER_ID == id).FirstOrDefault();
            if (model != null)
            {
                bll.Delete(model, true);
            }
            return Redirect("~/Manage/AdminList");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AdminPwd()
        {
            UrUsersBll bll = new UrUsersBll();
            UR_USERS user = BaseModels.CurrentUser();
            return View(user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AdminPwd(int userId,string oldPwd,string newPwd)
        {
            UrUsersBll bll = new UrUsersBll();
            UR_USERS user = bll.Query(t => t.USER_ID == userId).FirstOrDefault();
            if (user.USER_LOGIN_PASSWD != oldPwd.ToSHA1() && !string.IsNullOrEmpty(user.USER_LOGIN_PASSWD))
            {
                ViewBag.Success = false;
                ViewBag.Message = "当前密码不正确！";
                return View(user);
            }
            user.USER_LOGIN_PASSWD = newPwd.ToSHA1();

            bll.Update(user);

            ViewBag.Success = true;
            ViewBag.Message = "修改成功！";

            return View(user);
        }

        public ActionResult PartList()
        {
            string keyword = Request["keyword"];
            UrPartBll bll = new UrPartBll();
            List<UR_PART> allPart = bll.Query(t => string.IsNullOrEmpty(keyword) ? true : t.PART_NAME.Contains(keyword) ).ToList();

            ViewBag.Keyword = keyword;
            return View(allPart);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult PartEdit(int? id)
        {
            UrPartBll bll = new UrPartBll();
            UR_PART model = new UR_PART();
            if (id.HasValue && id > 0)
            {
                model = bll.Query(t => t.PART_ID == id).FirstOrDefault();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult PartEdit(UR_PART model)
        {
            UrPartBll bll = new UrPartBll();
            if (model.PART_ID > 0)
            {
                bll.Update(model);
            }
            else
            {
                bll.Add(model);
            }

            ViewBag.Success = true;
            ViewBag.Message = "修改成功！";

            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PartFunc(int? id)
        {
            ManageModels mm = new ManageModels();

            if (id.HasValue && id > 0)
            {
                mm.part = new UrPartBll().Query(t => t.PART_ID == id).FirstOrDefault();
                mm.allFunc = new SysFunctionBll().Query(t => true).ToList();
                mm.allPartFunc = new SysPartFuncBll().Query(t => t.PF_PART_ID == id).ToList();

            }
            return View(mm);
        }

        [HttpPost]
        public ActionResult PartDel(int id)
        {
            UrPartBll bll = new UrPartBll();
            UR_PART model = bll.Query(t => t.PART_ID == id).FirstOrDefault();

            if (model != null)
            {
                bll.Delete(model, true);
            }
            return Json(BaseModels.OK("成功！"));
        }

        /// <summary>
        /// 组织机构
        /// </summary>
        /// <returns></returns>
        public ActionResult OrganList()
        {
            string keyword = Request["keyword"];
            int page = string.IsNullOrEmpty(Request["page"]) ? 1 : Request["page"].ToInt();
            int count = string.IsNullOrEmpty(Request["count"]) ? 10 : Request["count"].ToInt();

            SysOrganizationBll bll = new SysOrganizationBll();
            List<SYS_ORGANIZATION> allOrgan = bll.Query(t => string.IsNullOrEmpty(keyword) ? true : t.ORG_NAME.Contains(keyword)).ToList();

            ViewBag.Total = allOrgan.Count;
            ViewBag.PageIndex = page;
            ViewBag.PageCount = count;
            ViewBag.Keyword = keyword;
            ViewBag.Parent = bll.Query(t => t.ORG_PARENT_ID == "-1").ToDictionary(t => t.ORG_ID, t => t.ORG_NAME);

            //分页
            allOrgan = allOrgan.Take(count * page).Skip(count * (page - 1)).ToList();

            return View(allOrgan);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult OrganEdit(int? Id)
        {
            SysOrganizationBll bll = new SysOrganizationBll();
            SYS_ORGANIZATION model = new SYS_ORGANIZATION();
            if (Id.ToInt() > 0)
                model = bll.Query(t => t.ORG_ID == Id).FirstOrDefault();

            ViewBag.Parent = bll.Query(t => t.ORG_PARENT_ID == "-1").ToDictionary(t => t.ORG_ID, t => t.ORG_NAME);

            return View(model);
        }

        [HttpPost]
        public ActionResult OrganEdit(SYS_ORGANIZATION model)
        {
            SysOrganizationBll bll = new SysOrganizationBll();
            ViewBag.Parent = bll.Query(t => t.ORG_PARENT_ID == "-1").ToDictionary(t => t.ORG_ID, t => t.ORG_NAME);

            if (model.ORG_ID > 0)
            {
                bll.Update(model);
            }
            else
            {
                bll.Add(model);
            }

            ViewBag.Success = true;
            ViewBag.Message = "修改成功！";

            return View(model);
        }


        [HttpPost]
        public ActionResult OrganDel(int id)
        {
            SysOrganizationBll bll = new SysOrganizationBll();
            SYS_ORGANIZATION model = bll.Query(t => t.ORG_ID == id).FirstOrDefault();

            if (model != null)
            {
                bll.Delete(model, true);
            }
            return Json(BaseModels.OK("成功！"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult FuncList()
        {
            string keyword = Request["keyword"];
            int page = string.IsNullOrEmpty(Request["page"]) ? 1 : Request["page"].ToInt();
            int count = string.IsNullOrEmpty(Request["count"]) ? 10 : Request["count"].ToInt();

            SysFunctionBll bll = new SysFunctionBll();
            List<SYS_FUNCTION> allFunc = bll.Query(t => string.IsNullOrEmpty(keyword) ? true : t.FN_NAME.Contains(keyword)).ToList();
            ViewBag.Parent = bll.Query(t => t.FN_PARENT_ID == -1).ToDictionary(t => t.FN_ID, t => t.FN_NAME);


            ViewBag.Total = allFunc.Count;
            ViewBag.PageIndex = page;
            ViewBag.PageCount = count;
            ViewBag.Keyword = keyword;
            //分页
            allFunc = allFunc.Take(count * page).Skip(count * (page - 1)).ToList();
            
            return View(allFunc);
        }

        /// <summary>
        /// 菜单
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult FuncEdit(int? Id)
        {
            SysFunctionBll bll = new SysFunctionBll();
            SYS_FUNCTION model = new SYS_FUNCTION();
            ViewBag.Parent = bll.Query(t => t.FN_PARENT_ID == -1).ToDictionary(t => t.FN_ID, t => t.FN_NAME);

            if (Id.ToInt() > 0)
                model = bll.Query(t => t.FN_ID == Id).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public ActionResult FuncEdit(SYS_FUNCTION model)
        {
            SysFunctionBll bll = new SysFunctionBll();
            ViewBag.Parent = bll.Query(t => t.FN_PARENT_ID == -1).ToDictionary(t => t.FN_ID, t => t.FN_NAME);

            model.FN_IS_LEAF = 0;
            if (model.FN_ID > 0)
            {
                bll.Update(model);
            }
            else
            {
                bll.Add(model);
            }

            ViewBag.Success = true;
            ViewBag.Message = "修改成功！";

            return View(model);
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FuncDel(int id)
        {
            SysFunctionBll bll = new SysFunctionBll();
            SYS_FUNCTION model = bll.Query(t => t.FN_ID == id).FirstOrDefault();

            if (model != null)
            {
                bll.Delete(model, true);
            }
            return Json(BaseModels.OK("成功！"));
        }

        [HttpPost]
        public ActionResult SavePartFunc(int id, string funcIds)
        {
            if (string.IsNullOrEmpty(funcIds))
            {
                return Json(BaseModels.Error("成功！"));
            }

            SysPartFuncBll bll = new SysPartFuncBll();

            List<SYS_PART_FUNC> allPF = bll.Query(t => t.PF_PART_ID == id).ToList();
            foreach (var pf in allPF)
            {
                bll.Delete(pf);
            }
            //bll.SaverChanges();

            string[] ids = funcIds.Split(',');
            foreach (var item in ids)
            {
                SYS_PART_FUNC model = new SYS_PART_FUNC();

                model.PF_PART_ID = id;
                model.PF_FN_ID = item.ToInt();

                bll.Add(model, false);
            }
            bll.SaverChanges();

            return Json(BaseModels.OK("成功！"));
        }

    }
}