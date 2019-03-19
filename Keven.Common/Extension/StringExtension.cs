using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace System
{
    public static class StringExtension
    {
        /// <summary>
        /// 判断字符串是否为空，null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string obj)
        {
            return string.IsNullOrEmpty(obj);
        }
        /// <summary>
        /// 对字符串进行SHA1加密
        /// </summary>
        /// <param name="obj">需要加密的字符串</param>
        /// <returns>密文</returns>
        public static string ToSHA1(this string obj)
        {
            byte[] StrRes = Encoding.Default.GetBytes(obj);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }

        /// <summary>
        /// SHA256加密，不可逆转
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToSHA256(this string obj)
        {
            System.Security.Cryptography.SHA256 s256 = new System.Security.Cryptography.SHA256Managed();
            byte[] byte1;
            byte1 = s256.ComputeHash(Encoding.Default.GetBytes(obj));
            s256.Clear();
            return BitConverter.ToString(byte1).Replace("-", "");
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //public static string ToMD5(this string obj)
        //{
        //    return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(obj, "MD5").ToLower();
        //}

        /// <summary>
        /// 字符串截断
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string ToSubString(this string obj, int startIndex, int length)
        {
            if (obj.IsEmpty())
                return "";
            if (obj.Length - 1 < startIndex)
            {
                return "";
            }
            string start = obj.Substring(0);
            if (start.Length < length)
                return start;
            else
                return start.Substring(0, length);
        }
        /// <summary>
        /// 截取从头开始，length长度字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ToSubString(this string obj, int length)
        {
            return obj.ToSubString(0, length);
        }

        /// <summary>
        /// 去除字符串中的html标记
        /// </summary>
        /// <param name="html"></param>
        /// <param name="length">保留多少长度</param>
        /// <returns></returns>
        public static string ReplaceHtmlTag(this string html, int length = 0)
        {
            if (string.IsNullOrEmpty(html))
                return "";
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");
            //去掉 \r\n\t
            if (!string.IsNullOrEmpty(strText))
                strText = strText.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }

        /// <summary>
        /// 在字符串头部添加字符
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="c"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ComplementStart(this string obj, char c, int length)
        {
            if (length <= 0)
                return obj;
            if (obj.IsEmpty())
                obj = "";
            StringBuilder sb = new StringBuilder(obj);
            for (int i = 0; i < length; i++)
            {
                sb.Insert(0, c);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 在字符串尾部添加字符
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="c"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ComplementEnd(this string obj, char c, int length)
        {
            if (length <= 0)
                return obj;
            if (obj.IsEmpty())
                obj = "";
            StringBuilder sb = new StringBuilder(obj);
            for (int i = 0; i < length; i++)
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 如果字符串为空，这返回为默认值，否则返回原本字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string EmptyOrNullToDefault(this string obj, string defaultValue = "")
        {
            if (!string.IsNullOrEmpty(obj))
                return obj;
            return defaultValue;
        }

    }
}
