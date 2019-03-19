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
    public class TrademarkController : Controller
    {


        [HttpGet]
        public ActionResult TmList()
        {

            return View();
        }

        [HttpGet]
        public ActionResult TmEdit()
        {

            return View();
        }


        /// <summary>
        /// 申请人列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <param name="keyword">关键字，名称或者模板</param>
        /// <param name="userId"></param>
        /// <param name="kefuId"></param>
        /// <param name="status"></param>
        /// <param name="nationality">国籍，</param>
        /// <param name="type">类型</param>
        /// <param name="begin">提交时间</param>
        /// <param name="end">提交时间</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ApplyerList(int? page, int? count, string keyword, string userId, string kefuId, string status, 
            string nationality, string type, string begin, string end)
        {
            page = page.ToInt() == 0 ? 1 : page;
            count = count.ToInt() == 0 ? 10 : count;

            var predicate = PredicateBuilder.New<TT_Trademark_Applyer>(true);
            if (!string.IsNullOrEmpty(keyword))
            {
                ViewBag.Keyword = keyword;
                predicate = predicate.And(t => t.Name.Contains(keyword) || t.TemplateName.Contains(keyword));
            }
            //if (!string.IsNullOrEmpty(parent))
            //{
            //    predicate = predicate.And(t => t.Parent == parent);
            //    ViewBag.Parent = parent;
            //}

            TtTrademarkApplyerBll bll = new TtTrademarkApplyerBll();

            int total = 0;
            List<TT_Trademark_Applyer> allAreas = bll.QueryByPage(page.ToInt(), count.ToInt(), out total, predicate, t => t.Id, false);

            ViewBag.Total = total;
            ViewBag.PageIndex = page;
            ViewBag.PageCount = count;
            var baseType = new PdTypeBll().Query(t => t.TypeId == 5 || t.TypeId == 6);
            ViewBag.DicNationality = baseType.Where(t=>t.TypeId==5).ToDictionary(t => t.Tid.ToInt(), t => t.Name);
            ViewBag.DicType = baseType.Where(t => t.TypeId == 6).ToDictionary(t => t.Tid.ToInt(), t => t.Name);

            return View(allAreas);
        }

        [HttpGet]
        public ActionResult ApplyerEdit(int? id)
        {
            TT_Trademark_Applyer applyer = new TT_Trademark_Applyer();
            if (id.ToInt() > 0)
            {
                TtTrademarkApplyerBll bll = new TtTrademarkApplyerBll();
                applyer = bll.Query(t => t.Id == id).FirstOrDefault();
            }
            var baseType = new PdTypeBll().Query(t => t.TypeId == 5 || t.TypeId == 6 || t.TypeId == 8);
            ViewBag.DicNationality = baseType.Where(t => t.TypeId == 5).ToDictionary(t => t.Tid.ToInt(), t => t.Name);
            ViewBag.DicType = baseType.Where(t => t.TypeId == 6).ToDictionary(t => t.Tid.ToInt(), t => t.Name);
            ViewBag.DicCerificateType = baseType.Where(t => t.TypeId == 8).ToDictionary(t => t.Tid.ToInt(), t => t.Name);

            ViewBag.allArea = new PdAreasBll().Query(t => true);
            //ViewBag.DicProvince = allArea.Where(t => t.Parent == "CN").ToDictionary(t => t.Id, t => t.Name);
            //ViewBag.DicCity = allArea.Where(t => t.Parent == (applyer.Province == null ? "110000" : applyer.Province )).ToDictionary(t => t.Id, t => t.Name);
            //ViewBag.DicDistrict = allArea.Where(t => t.Parent == (applyer.City == null ? "110100" : applyer.City)).ToDictionary(t => t.Id, t => t.Name);

            return View(applyer);
        }

        [HttpPost]
        public ActionResult ApplyerEdit(TT_Trademark_Applyer model)
        {
            TtTrademarkApplyerBll bll = new TtTrademarkApplyerBll();
            TT_Trademark_Applyer applyer = bll.Query(t => t.Id == model.Id).FirstOrDefault();

            var baseType = new PdTypeBll().Query(t => t.TypeId == 5 || t.TypeId == 6 || t.TypeId==8);
            ViewBag.DicNationality = baseType.Where(t => t.TypeId == 5).ToDictionary(t => t.Tid.ToInt(), t => t.Name);
            ViewBag.DicType = baseType.Where(t => t.TypeId == 6).ToDictionary(t => t.Tid.ToInt(), t => t.Name);
            ViewBag.DicCerificateType = baseType.Where(t => t.TypeId == 8).ToDictionary(t => t.Tid.ToInt(), t => t.Name);

            if (applyer == null)
            {
                applyer = model;
                bll.Add(applyer);
            }
            else
            {
                applyer.Name = model.Name;
                //applyer.Remark = model.Remark;
                bll.Update(applyer);

                return View(applyer);
            }

            ViewBag.Success = true;
            ViewBag.Message = "成功！";

            return View(applyer);
        }



        [HttpPost]
        public ActionResult GetCity(string province)
        {
            var allArea = new PdAreasBll().Query(t => t.Parent == (string.IsNullOrEmpty(province) ? "110000" : province));
            return Json(allArea.Select(t => new { t.Id, t.Name }));
        }
        [HttpPost]
        public ActionResult GetDistrict(string city)
        {
            var allArea = new PdAreasBll().Query(t => t.Parent == (string.IsNullOrEmpty(city) ? "110100" : city));
            return Json(allArea.Select(t => new { t.Id, t.Name }));
        }

    }
}