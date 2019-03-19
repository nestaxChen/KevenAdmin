using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Keven.BLL;
using Keven.Manage.Models;
using Keven.Model;
using Keven.Common;
using System.Linq.Expressions;
using LinqKit;

namespace Keven.Manage.Controllers
{
    [AdminAuthorization(Level = (int)AdminModel.TmGroupEnum.商标经纪人)]
    public class ConfigController : Controller
    {
        /// <summary>
        /// 区域查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <param name="keyword"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public ActionResult AreaList(int? page,int? count,string keyword,string parent)
        {
             page = page.ToInt() == 0 ? 1 : page;
             count = count.ToInt() == 0 ? 10 : count;

            var predicate = PredicateBuilder.New<PD_Areas>(true);
            if (!string.IsNullOrEmpty(keyword))
            {
                ViewBag.Keyword = keyword;
                predicate = predicate.And(t => t.Name.Contains(keyword));
            }
            if (!string.IsNullOrEmpty(parent))
            {
                predicate = predicate.And(t => t.Parent == parent);
                ViewBag.Parent = parent;
            }

            PdAreasBll bll = new PdAreasBll();

            int total = 0;
            List<PD_Areas> allAreas = bll.QueryByPage(page.ToInt(), count.ToInt(), out total, predicate, t => t.Id, false);

            ViewBag.Total = total;
            ViewBag.PageIndex = page;
            ViewBag.PageCount = count;
            ViewBag.Province = bll.Query(t => t.Parent == "CN").ToDictionary(t => t.Id, t => t.Name);

            return View(allAreas);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult AreaEdit(string Id)
        {
            PdAreasBll bll = new PdAreasBll();
            PD_Areas model = new PD_Areas();
            if (!string.IsNullOrEmpty(Id))
                model = bll.Query(t => t.Id == Id).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public ActionResult AreaEdit(PD_Areas model)
        {
            PdAreasBll bll = new PdAreasBll();

            if (!string.IsNullOrEmpty(model.Id))
            {
                bll.Update(model);
            }

            ViewBag.Success = true;
            ViewBag.Message = "修改成功！";

            return View(model);
        }



        public ActionResult ClassList(int? page, int? count, string keyword,string tmCls)
        {
            page = page.ToInt() == 0 ? 1 : page;
            count = count.ToInt() == 0 ? 10 : count;
            Expression<Func<TT_Trademark_Class, bool>> predicate = t => true;

            if (!string.IsNullOrEmpty(keyword))
            {
                ViewBag.Keyword = keyword;
                predicate = t => t.ClassName.Contains(keyword);
            }
            if (!string.IsNullOrEmpty(tmCls))
            {
                predicate = t => t.ParentClass == tmCls;
                ViewBag.tmCls = tmCls;
            }

            TtTrademarkClassBll bll = new TtTrademarkClassBll();

            int total = 0;
            List<TT_Trademark_Class> allClass = bll.QueryByPage(page.ToInt(), count.ToInt(), out total, predicate, t => t.Id, false);

            ViewBag.Total = total;
            ViewBag.PageIndex = page;
            ViewBag.PageCount = count;
            ViewBag.BigClass = bll.Query(t => string.IsNullOrEmpty(t.ParentClass));

            return View(allClass);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult KefuList(int? page, int? count, string keyword)
        {
            page = page.ToInt() == 0 ? 1 : page;
            count = count.ToInt() == 0 ? 10 : count;

            var predicate = PredicateBuilder.New<TT_Kefu>(true);
            if (!string.IsNullOrEmpty(keyword))
            {
                ViewBag.Keyword = keyword;
                predicate = predicate.And(t => t.Name.Contains(keyword));
            }

            TtKefuBll bll = new TtKefuBll();

            int total = 0;
            List<TT_Kefu> allKefu = bll.QueryByPage(page.ToInt(), count.ToInt(), out total, predicate, t => t.Id, false);

            ViewBag.Total = total;
            ViewBag.PageIndex = page;
            ViewBag.PageCount = count;

            return View(allKefu);
        }

        [HttpGet]
        public ActionResult KefuEdit(int? id)
        {
            TT_Kefu kefu = new TT_Kefu();
            if (id.ToInt() > 0)
            {
                TtKefuBll bll = new TtKefuBll();
                kefu = bll.Query(t => t.Id == id).FirstOrDefault();
            }
            return View(kefu);
        }


        [HttpPost]
        public ActionResult KefuEdit(TT_Kefu model)
        {
            TtKefuBll bll = new TtKefuBll();
            TT_Kefu kefu = bll.Query(t => t.Id == model.Id).FirstOrDefault();

            if (kefu == null)
            {
                model.CreateDate = DateTime.Now;
                model.CreateId = BaseModels.CurrentUser().USER_ID;
                bll.Add(model);
            }
            else
            {
                kefu.Name = model.Name;
                kefu.RealName = model.RealName;
                kefu.Level = model.Level;
                kefu.Photo = model.Photo;
                kefu.QQ = model.QQ;
                kefu.Weixin = model.Weixin;
                kefu.WeixinUrl = model.WeixinUrl;
                kefu.Email = model.Email;
                kefu.Tel = model.Tel;
                kefu.Mobile = model.Mobile;
                kefu.OrderNum = model.OrderNum;
                kefu.WorkYear = model.WorkYear;
                kefu.IsOnline = model.IsOnline;
                kefu.IsChecked = model.IsChecked;

                kefu.UpdateDate = DateTime.Now;
                kefu.UpdateId = BaseModels.CurrentUser().USER_ID;
                bll.Update(kefu);
            }

            ViewBag.Success = true;
            ViewBag.Message = "修改成功！";

            return View(kefu);
        }


        [HttpPost]
        public ActionResult KefuDel(int id)
        {
            TtKefuBll bll = new TtKefuBll();
            TT_Kefu model = bll.Query(t => t.Id == id).FirstOrDefault();
            if (model != null)
            {
                bll.Delete(model, true);
            }
            return Json(BaseModels.OK("成功！"));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BaseTypeList(int? page, int? count, string keyword)
        {
            page = page.ToInt() == 0 ? 1 : page;
            count = count.ToInt() == 0 ? 10 : count;

            var predicate = PredicateBuilder.New<PD_BASETYPE>(true);
            if (!string.IsNullOrEmpty(keyword))
            {
                ViewBag.Keyword = keyword;
                predicate = predicate.And(t => t.Name.Contains(keyword));
            }

            PdBasetypeBll bll = new PdBasetypeBll();

            int total = 0;
            List<PD_BASETYPE> allKefu = bll.QueryByPage(page.ToInt(), count.ToInt(), out total, predicate, t => t.Id, false);

            ViewBag.Total = total;
            ViewBag.PageIndex = page;
            ViewBag.PageCount = count;

            return View(allKefu);
        }


        [HttpGet]
        public ActionResult BaseTypeEdit(int? id)
        {
            PD_BASETYPE bType = new PD_BASETYPE();
            if (id.ToInt() > 0)
            {
                PdBasetypeBll bll = new PdBasetypeBll();
                bType = bll.Query(t => t.Id == id).FirstOrDefault();
            }
            return View(bType);
        }


        [HttpPost]
        public ActionResult BaseTypeEdit(PD_BASETYPE model)
        {
            PdBasetypeBll bll = new PdBasetypeBll();
            PD_BASETYPE bType = bll.Query(t => t.Id == model.Id).FirstOrDefault();

            if (bType == null)
            {
                bType = model;
                bll.Add(bType);
            }
            else
            {
                bType.Name = model.Name;
                bType.Remark = model.Remark;
                bll.Update(bType);
            }

            ViewBag.Success = true;
            ViewBag.Message = "修改成功！";

            return View(bType);
        }

        [HttpPost]
        public ActionResult BaseTypeDel(int id)
        {
            PdBasetypeBll bll = new PdBasetypeBll();
            PD_BASETYPE model = bll.Query(t => t.Id == id).FirstOrDefault();
            if (model != null)
            {
                bll.Delete(model, true);
            }
            return Json(BaseModels.OK("成功！"));
        }



        [HttpGet]
        public ActionResult TypeList(int? page, int? count, string keyword, int? baseTypeId)
        {
            page = page.ToInt() == 0 ? 1 : page;
            count = count.ToInt() == 0 ? 10 : count;

            var predicate = PredicateBuilder.New<PD_TYPE>(true);
            if (!string.IsNullOrEmpty(keyword))
            {
                ViewBag.Keyword = keyword;
                predicate = predicate.And(t => t.Name.Contains(keyword));
            }
            if (baseTypeId.ToInt() > 0)
            {
                ViewBag.baseTypeId = baseTypeId;
                predicate = predicate.And(t => t.TypeId == baseTypeId);
            }

            PdTypeBll bll = new PdTypeBll();

            int total = 0;
            List<PD_TYPE> allKefu = bll.QueryByPage(page.ToInt(), count.ToInt(), out total, predicate, t => t.Id, false);

            ViewBag.Total = total;
            ViewBag.PageIndex = page;
            ViewBag.PageCount = count;
            ViewBag.BaseType = new PdBasetypeBll().Query(t => true).ToDictionary(t => t.Id, t => t.Name);

            return View(allKefu);
        }

        [HttpGet]
        public ActionResult TypeEdit(int? id)
        {
            PD_TYPE bType = new PD_TYPE();
            if (id.ToInt() > 0)
            {
                PdTypeBll bll = new PdTypeBll();
                bType = bll.Query(t => t.Id == id).FirstOrDefault();
            }
            ViewBag.BaseType = new PdBasetypeBll().Query(t => true).ToDictionary(t => t.Id, t => t.Name);

            return View(bType);
        }


        [HttpPost]
        public ActionResult TypeEdit(PD_TYPE model)
        {
            PdTypeBll bll = new PdTypeBll();
            PD_TYPE bType = bll.Query(t => t.Id == model.Id).FirstOrDefault();

            if (bType == null)
            {
                bType = model;
                bll.Add(bType);
            }
            else
            {
                bType.Tid = model.Tid;
                bType.TypeId = model.TypeId;
                bType.Name = model.Name;
                bType.Remark = model.Remark;
                bll.Update(bType);
            }

            ViewBag.Success = true;
            ViewBag.Message = "修改成功！";
            ViewBag.BaseType = new PdBasetypeBll().Query(t => true).ToDictionary(t => t.Id, t => t.Name);

            return View(bType);
        }

        [HttpPost]
        public ActionResult TypeDel(int id)
        {
            PdTypeBll bll = new PdTypeBll();
            PD_TYPE model = bll.Query(t => t.Id == id).FirstOrDefault();
            if (model != null)
            {
                bll.Delete(model, true);
            }
            return Json(BaseModels.OK("成功！"));
        }

    }
}