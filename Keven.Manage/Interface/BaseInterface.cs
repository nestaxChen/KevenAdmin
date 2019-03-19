using Keven.BLL;
using Keven.Manage.Models;
using Keven.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;

namespace Keven.Manage.Interface
{
    public class BaseInterface : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "application/Json";
            HttpRequest request = context.Request;
            JavaScriptSerializer js = new JavaScriptSerializer();


            //context.Response.Write(js.Serialize(rd));
            //return;

        }

        /// <summary>
        /// 过滤关键字的危险支付
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string Request(string key, string defaultValue = "")
        {
            string s = System.Web.HttpContext.Current.Request[key];
            if (string.IsNullOrEmpty(s))
            {
                return defaultValue;
            }
            else
            {
                //if (s.ToLower() == "exec.com.cn")
                //    return s;
                string StrRegex = @"\b(alert|prompt)\b|^\+/v(8|9)|\b(and|or)\b.{1,6}?(=|>|<|\bin\b|\blike\b)|/\*.+?\*/|<\s*script\b|<\s*img\b|\bEXEC\b|UNION.+?SELECT|UPDATE.+?SET|INSERT\s+INTO.+?VALUES|(SELECT|DELETE).+?FROM|(CREATE|ALTER|DROP|TRUNCATE)\s+(TABLE|DATABASE)";
                Regex r = new Regex(StrRegex, RegexOptions.IgnoreCase);
                s = r.Replace(s, "");
                return s.Replace("'", "&#39;").Replace("alert(", "").Replace("<", "").Replace(">", "");

            }
        }


        //public static T GetRequestPrams(HttpContext context)
        //{
        //    Stream sream = context.Request.InputStream;
        //    StreamReader sr = new StreamReader(sream);
        //    string search = sr.ReadToEnd();
        //    sr.Close();
        //    var jSetting = new JsonSerializerSettings
        //    {
        //        NullValueHandling = NullValueHandling.Ignore
        //    };
        //    return JsonConvert.DeserializeObject<T>(search, jSetting);
        //}



        /// <summary>
        /// 接口验证登录
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UR_USERS GetUser(string token)
        {
            if (BaseModels.IsLogin())
            {
                //登录状态
                return BaseModels.CurrentUser();
            }

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            try
            {
                UrUsersBll bll = new UrUsersBll();
 
                return bll.Query(t => t.USER_UNUSED1 == token).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

    }
}