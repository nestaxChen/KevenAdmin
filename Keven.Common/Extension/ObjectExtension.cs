using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace System
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 将Object类型转换为Int类型，为空或者转换失败返回0
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">转换失败返回默认值</param>
        /// <returns></returns>
        public static int ToInt(this object obj, int defaultValue = 0)
        {
            int result = defaultValue;
            if (obj == null)
                return result;
            if (int.TryParse(obj.ToString(), out result))
                return result;
            else
            {
                try
                {
                    return Convert.ToInt32(obj);
                }
                catch
                {
                    return defaultValue;
                }
            }
        }

        /// <summary>
        /// 将Object类型转换为DateTime类型，为空或者转换失败返回最小时间
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object obj)
        {
            DateTime result = new DateTime(1753, 1, 1);
            if (obj == null)
                return result;
            if (DateTime.TryParse(obj.ToString(), out result))
                return result;
            else
            {
                try
                {
                    return Convert.ToDateTime(obj);
                }
                catch (Exception)
                {
                    return new DateTime(1753, 1, 1);
                }
            }
        }

        /// <summary>
        /// 将类型转换为Double类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(this object obj, double defaultValue = 0)
        {
            double result = defaultValue;
            if (obj == null)
                return result;
            if (double.TryParse(obj.ToString(), out result))
                return result;
            else
            {
                try
                {
                    return Convert.ToDouble(obj.ToString());
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }
        }

        /// <summary>
        /// 将类型转换为decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object obj, decimal defaultValue = 0)
        {
            decimal result = defaultValue;
            if (obj == null)
                return result;
            if (decimal.TryParse(obj.ToString(), out result))
                return result;
            else
            {
                try
                {
                    return Convert.ToDecimal(obj);
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }
        }


        /// <summary>
        /// 将类型转换为long
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ToLong(this object obj, long defaultValue = 0)
        {
            long result = defaultValue;
            if (obj == null)
                return result;
            if (long.TryParse(obj.ToString(), out result))
                return result;
            else
            {
                try
                {
                    return Convert.ToInt64(obj);
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }
        }
        /// <summary>
        /// 将类型转换为Booolean
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool ToBoolean(this object obj, bool defaultValue = false)
        {
            bool result = defaultValue;
            if (obj == null)
                return result;

            if (typeof(int) == obj.GetType())
            {
                if ((int)obj > 0)
                    return true;
                else
                    return false;
            }
            if (bool.TryParse(obj.ToString(), out result))
                return result;
            else
            {
                try
                {
                    return Convert.ToBoolean(obj);
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }
        }

        /// <summary>
        /// xml序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filepath"></param>
        public static void XmlSerialize(this object obj, string filepath)
        {
            if (filepath.IsEmpty())
                return;
            try
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite))
                {
                    XmlSerializer formatter = new XmlSerializer(obj.GetType());
                    formatter.Serialize(fs, obj);
                }
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// 判断是否为数字 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool isNumeric(this object obj)
        {
            Type t = obj.GetType();
            if (t == typeof(int) || t == typeof(Int32) || t == typeof(Int16) || t == typeof(Int64))
            {
                return true;
            }
            string o = obj.ToString();
            if (Regex.IsMatch(o, @"^[1-9]\d*$"))
            {
                return true;
            }

            return false;
        }


        public static string ToJson(this object obj, Newtonsoft.Json.Formatting formatting = Newtonsoft.Json.Formatting.None)
        {
            if (obj == null)
                return "";
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, formatting);
        }
    }
}
