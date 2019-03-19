using Keven.BLL;
using Keven.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Keven.Manage.Models
{
    public class BaseModels
    {
        public static string[] TrademarkClass = new string[] { "不限", "化学原料", "颜料油漆", "化妆品", "燃料油脂", "医药", "五金金属", "机械设备", "手工器械", "电子电脑", "医疗器械", "家用电器", "运输工具", "军火烟火", "珠宝钟表", "乐器", "办公文具", "橡胶制品", "皮革箱包", "建筑材料", "家具", "厨房洁具", "绳网袋篷", "纺织纱线", "布料床单", "服装鞋帽", "花边配饰", "地毯席垫", "体育玩具", "食品", "小食配料", "水果花木", "啤酒饮料", "酒", "烟草烟具", "广告贸易", "金融物管", "建筑修理", "通讯传媒", "运输旅行", "材料加工", "教育娱乐", "技术服务", "餐饮酒店", "医疗园艺", "法律" };



        //public List<PD_Areas> GetCity(string province)
        //{
        //    var allArea = new PdAreasBll().Query(t => t.Parent == (string.IsNullOrEmpty(province) ? "110000" : province));
        //    return allArea.ToList();
        //}
        //public List<PD_Areas> GetDistrict(string city)
        //{
        //    var allArea = new PdAreasBll().Query(t => t.Parent == (string.IsNullOrEmpty(city) ? "110100" : city));
        //    return allArea.ToList();

        //}

        public class Result
        {
            public static JsonRequestBehavior Behavior = JsonRequestBehavior.DenyGet;

            public static JsonResult Success(object Data)
            {
                return new JsonResult() { Data = new { error = false, data = Data } };
            }
            public static JsonResult Error(object Data)
            {
                return new JsonResult() { Data = new { error = true, data = Data } };
            }
            public static RedirectResult ErrorPage(string message)
            {
                RedirectResult rr = new RedirectResult("~/Error?Message=" + message);
                return rr;
            }

            public static JsonResult Ok(string message)
            {
                return Ok(message, null);
            }

            public static JsonResult Ok(string message, object data)
            {
                JsonResult js = new JsonResult();
                js.Data = new { error = false, message = message, msg = message, data = data };
                js.JsonRequestBehavior = Behavior;
                return js;
            }
        }

        /// <summary>
        /// 验证是否登录
        /// </summary>
        /// <returns></returns>
        public static bool IsLogin()
        {
            try
            {
                UrUsersBll bll = new UrUsersBll();
                HttpCookie cookies = HttpContext.Current.Request.Cookies["UserToken"];
                if (cookies == null)
                    return false;

                string token = cookies["token"];
                UR_USERS user = bll.Query(t => t.USER_UNUSED1 == token).FirstOrDefault();

                if (user == null)
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 从cookie获取用户名
        /// </summary>
        /// <returns></returns>
        public static string GetUserName()
        {
            HttpCookie cookies = HttpContext.Current.Request.Cookies["UserToken"];
            if (cookies == null)
                return "";

            return HttpUtility.UrlDecode(cookies["userName"], System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// 获取当前登录user对象
        /// </summary>
        /// <returns></returns>
        public static UR_USERS CurrentUser()
        {
            try
            {
                UrUsersBll bll = new UrUsersBll();
                HttpCookie cookies = HttpContext.Current.Request.Cookies["UserToken"];
                if (cookies == null)
                    return null;
                string token = cookies["token"];
                return bll.Query(t => t.USER_UNUSED1 == token).FirstOrDefault();
            }
            catch
            {

                return null;
            }
        }


        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ReturnData Error(string message)
        {
            ReturnData rd = new ReturnData();
            rd.status_code = "1";
            rd.message = message;
            return rd;
        }

        /// <summary>
        /// 和登录有关的错误信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ReturnData ErrorLogin(string message)
        {
            ReturnData rd = new ReturnData();
            rd.status_code = "9";
            rd.message = message;
            return rd;
        }


        /// <summary>
        /// 成功状态
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ReturnData OK(string message)
        {
            ReturnData rd = new ReturnData();
            rd.status_code = "0";
            rd.message = message;
            return rd;
        }

        public static ReturnData OK(string message, object data)
        {
            ReturnData rd = new ReturnData();
            rd.status_code = "0";
            rd.message = message;
            rd.data = data;
            return rd;
        }

        public static ReturnData OK(string message, object data, object pager)
        {
            ReturnData rd = new ReturnData();
            rd.status_code = "0";
            rd.message = message;
            rd.data = data;
            rd.pager = pager;
            return rd;
        }


    }



    public class ReturnData
    {
        public String status_code;

        public String message;
        public Object data;
        public Object pager;
        public String notice;
    }

    public class Pager
    {
        public int pageIndex;
        public int pageCount;
        public int total;
    }


    /// <summary>
    /// 国籍
    /// </summary>
    public enum NationalityEnum
    {
        /// <summary>
        /// 中国大陆
        /// </summary>
        中国大陆 = 1,
        /// <summary>
        /// 国外
        /// </summary>
        国外 = 2,
        /// <summary>
        /// 中国台湾
        /// </summary>
        中国台湾 = 3,
        /// <summary>
        /// 中国香港
        /// </summary>
        中国香港 = 4,
        /// <summary>
        /// 中国澳门
        /// </summary>
        中国澳门 = 5

    }






}