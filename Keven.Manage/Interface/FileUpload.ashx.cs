using Keven.Manage.Models;
using Keven.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Keven.Manage.Interface
{
    /// <summary>
    /// FileUpload 的摘要说明
    /// </summary>
    public class FileUpload : BaseInterface
    {

        public override void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/Json";
            HttpRequest request = context.Request;
            JavaScriptSerializer js = new JavaScriptSerializer();

            ReturnData rd = new ReturnData();

            string token = Request("token");
            UR_USERS user = GetUser(token);

            int userId = user.USER_ID.ToInt();

            if (userId == 0)
            {
                context.Response.Write(js.Serialize(BaseModels.ErrorLogin("请先登录！")));
                return;
            }

            string timestamp = Request("timestamp").Trim();
            if (string.IsNullOrEmpty(timestamp) || timestamp == "0")
            {
                timestamp = DateTime.Now.ToTimeStamp().ToString();
            }

            string isfile = Request("isfile"); // 如果是上传文件或者没有上传base64字符串
            if (isfile == "file" || Request("base64") == "")
            {

                rd = UploadFile(context, timestamp);
                context.Response.Write(js.Serialize(rd));
                return;
            }
            string filetype = Request("filetype");
            if (string.IsNullOrEmpty(filetype))
            {
                filetype = "image/jpeg";
            }
            string base64 = Request("base64");
            if (string.IsNullOrEmpty(base64))
            {
                rd = BaseModels.Error("请传入base64格式的图片字符串");
                context.Response.Write(js.Serialize(rd));
                return;
            }
            else
            {
                if (base64.IndexOf(',') >= 0)
                {
                    filetype = base64.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ElementAt(0);
                    if (filetype.IndexOf("jpg") >= 0 || filetype.IndexOf("jpeg") >= 0)
                        filetype = "image/jpeg";
                    base64 = base64.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ElementAt(1);
                }
            }

            string msg = "";
            bool result = StoreFile(base64, userId, filetype, out msg, "trademark", timestamp);

            rd = new Models.ReturnData()
            {
                data = new { filename = result ? msg : "" },
                message = result ? "" : msg,
                status_code = result ? "0" : "1"
            };

            context.Response.Write(js.Serialize(rd));
            return;
        }



        /// <summary>
        /// file形式的文件上传
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userid"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public ReturnData UploadFile(HttpContext context, string timestamp)
        {
            HttpRequest request = context.Request;
            HttpPostedFile file1 = context.Request.Files["file1"];
            
            if (file1 == null)
            {
                return BaseModels.Error("上传文件不存在！");
            }
            if (file1.ContentLength > 1024 * 1024 * 10)
            {
                return BaseModels.Error("上传文件不可大于10M！");
            }

            System.IO.BinaryReader reader = new System.IO.BinaryReader(file1.InputStream);
            string fileclass = "";
            for (int i = 0; i < 2; i++)
            {
                fileclass += reader.ReadByte().ToString();
            }
            if (fileclass != "255216" && fileclass != "13780" && fileclass != "6677" && fileclass != "3780" && fileclass != "8297" && fileclass != "8075")
            {
                return BaseModels.Error("文件格式不正确！");
            }
            string ext = ".jpg";
            if (fileclass == "3780")
            {
                ext = ".pdf";
            }
            if (fileclass == "8297")
            {
                ext = ".rar";
            }
            if (fileclass == "8075")
            {
                ext = ".zip";
            }

            string dpath = "/userfile/trademark/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            string filename = timestamp + ext;

            string msg = "";
            bool storeImg = StoreImg(context, file1, dpath, filename, out msg);

            if (storeImg)
            {
                return BaseModels.OK(msg, new { filename = dpath + filename });
            }
            else
            {
                return BaseModels.Error(msg);
            }
        }

        public bool StoreImg(HttpContext context, HttpPostedFile file1, string dpath, string filename, out string msg)
        {
            msg = "上传成功";
            string imagePath = dpath + filename;
            try
            {
                if (!System.IO.Directory.Exists(context.Request.MapPath(dpath)))
                {
                    System.IO.Directory.CreateDirectory(context.Request.MapPath(dpath));
                }
                string savefilepath2 = context.Request.MapPath(imagePath);
                file1.SaveAs(savefilepath2);
                return true;
            }
            catch (Exception ex)
            {
                Keven.Common.Log.Error("Exception/Upload", ex.ToString());
                msg = "上传失败:" + ex.Message;
                return false;
            }
        }


        /// <summary>
        /// 储存base64
        /// </summary>
        /// <param name="base64"></param>
        /// <param name="userId"></param>
        /// <param name="suffix"></param>
        /// <param name="msg"></param>
        /// <param name="folder"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public bool StoreFile(string base64, int userId, string suffix, out string msg, string folder = "trademark", string timestamp = "")
        {
            msg = "";
            byte[] arr = Convert.FromBase64String(base64.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+"));//
            try
            {
                string ext = ".jpg";
                if (suffix == "pdf")
                    ext = ".pdf";
                timestamp = string.IsNullOrEmpty(timestamp) ? DateTime.Now.ToTimeStamp().ToInt().ToString() : timestamp;
                msg = timestamp + ext;

                string save_folder = string.Format("/userfile/trademark/{0}/", userId.ToString());
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(save_folder)))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(save_folder));
                }

                if (suffix == "pdf")
                {
                    FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(save_folder + timestamp + ext), FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(arr);
                    bw.Close();
                    fs.Close();

                    return true;
                }
                else
                {
                    //图片格式
                    using (MemoryStream ms = new MemoryStream(arr))
                    {
                        Bitmap bmp = new Bitmap(ms);
                        bmp.Save(HttpContext.Current.Server.MapPath(save_folder + timestamp + ext), System.Drawing.Imaging.ImageFormat.Jpeg);
                        bmp.Dispose();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Keven.Common.Log.Error("Exception/Upload", ex.ToString());
                msg = ex.Message;
                return false;
            }
        }


    }
}