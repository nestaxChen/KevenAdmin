using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Keven.Common
{
    public class Log
    {
        public static void Debug(string filename, string txt)
        {
            Log4Net.Log4NetHelper.Debug(filename, txt);
        }
        public static void Info(string filename, string txt)
        {
            Log4Net.Log4NetHelper.Info(filename, txt);
        }
        public static void Error(string filename, string txt)
        {
            Log4Net.Log4NetHelper.Error(filename, txt);
        }

        /// <summary>
        /// 写日志，每天一份日志,文件保存目录winform默认在Bin下的Log目录下，网站是在网站的根目录的Log下 
        /// </summary>
        /// <param name="saveFile">日志文件保存的文件夹</param>
        /// <param name="txt">写内容</param>
        /// <returns></returns>
        public static string WriteTxt(string saveFile, string txt)
        {
            try
            {
                InfoTxt(saveFile, txt);
                return "true";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public static void InfoTxt(string filename, string txt, bool isOnlyMessage = false)
        {
            //return;
            if (string.IsNullOrEmpty(txt))
                return;
            try
            {
                ILog log = GetLogger(filename, isOnlyMessage);
                log.Info(txt);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 记录文本
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ILog GetLogger(string name, bool isOnlyMessage = false)
        {
            if (string.IsNullOrEmpty(name))
                return LogManager.GetLogger("Defalut");

            name = System.Text.RegularExpressions.Regex.Replace(name, @"[^\w]", "\\");
            name = name.Trim('\\').Trim('/');
            string name2 = name.ToUpper()
                .Replace("\"", "_")
                .Replace("\r\n", "\\r\\n")
                .Replace("\n", "\\n")
                .Replace("\r", "\\r")
                .Replace("\t", "\\t")
                .Replace("\\", "_")
                .Replace("/", "_");
            string repositoryName = "R_LOG4HELPER_" + name2 + (isOnlyMessage ? "_ISOM_" : "");
            string newname = "LOG_" + repositoryName;
            ILoggerRepository repository = null;
            try
            {
                repository = LogManager.GetRepository(repositoryName);
            }
            catch (Exception) { }


            if (repository != null)
            {
                ILog ilog = LogManager.GetLogger(repositoryName, newname);
                if (ilog != null)
                    return ilog;
            }
            try
            {
                repository = LogManager.CreateRepository(repositoryName);
            }
            catch (Exception)
            {
                return null;
            }

            repository.Threshold = Level.All;

            RollingFileAppender rollingFileAppender = new RollingFileAppender();
            rollingFileAppender.AppendToFile = true;
            rollingFileAppender.DatePattern = (isOnlyMessage ? "'NoTime-'" : "") + "yyyy-MM-dd'.log'";
            rollingFileAppender.File = "log\\" + name + "\\";
            rollingFileAppender.ImmediateFlush = true;
            rollingFileAppender.Name = newname;
            //rollingFileAppender.LockingModel = new FileAppender.InterProcessLock();
            rollingFileAppender.RollingStyle = RollingFileAppender.RollingMode.Date;
            rollingFileAppender.StaticLogFileName = false;
            if (!isOnlyMessage)
                rollingFileAppender.Layout = new PatternLayout("%date [%thread] %-5level - %message%newline");
            else
                rollingFileAppender.Layout = new PatternLayout("%message%newline");
            rollingFileAppender.ActivateOptions();
            BasicConfigurator.Configure(repository, rollingFileAppender);

            return log4net.LogManager.GetLogger(repository.Name, newname);
        }


        public static string WriteTxtNoTime(string saveFile, string txt)
        {
            try
            {
                Log4Net.Log4NetHelper.InfoTxt(saveFile, txt, true);
                return "true";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

    }
}
