using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository;
using System.Reflection;
using System.Configuration;

namespace Keven.Common.Log4Net
{
    public class Log4NetHelper
    {
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

        public static void Debug(string filename, string txt)
        {
            ILog log = GetLogger(filename);
            log.Debug(txt);
        }
        public static void Info(string filename, string txt)
        {
            ILog log = GetLogger(filename);
            log.Info(txt);
        }
        public static void Error(string filename, string txt)
        {
            ILog log = GetLogger(filename);
            log.Error(txt);
        }

        //public static void Info<T>(T model)
        //{
        //    try
        //    {
        //        ILog i = GetSQLLogger(typeof(T));
        //        if (i != null)
        //            i.Info(model);
        //    }
        //    catch { }
        //}
        //public static void Info<T>(T model, string tablename)
        //{
        //    try
        //    {
        //        ILog i = GetSQLLogger(tablename, typeof(T));
        //        if (i != null)
        //            i.Info(model);
        //    }
        //    catch { }
        //}


        /// <summary>
        /// 记录文本
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ILog GetLogger(string name)
        {
            if (string.IsNullOrEmpty(name))
                return LogManager.GetLogger("Defalut");

            name = System.Text.RegularExpressions.Regex.Replace(name, @"[^\w]", "\\");
            string name2 = name.ToUpper()
                .Replace("\"", "_")
                .Replace("\r\n", "\\r\\n")
                .Replace("\n", "\\n")
                .Replace("\r", "\\r")
                .Replace("\t", "\\t")
                .Replace("\\", "_")
                .Replace("/", "_");
            string repositoryName = "R_" + name2;
            string newname = "LOG_" + repositoryName;
            ILoggerRepository repository = null;
            try
            {
                repository = LogManager.GetRepository(repositoryName);
            }
            catch (Exception) { }

            if (repository != null)
                return LogManager.GetLogger(repositoryName, newname);

            try
            {
                repository = LogManager.CreateRepository(repositoryName);
            }
            catch (Exception)
            {
                repository = LogManager.GetRepository(repositoryName);
            }

            repository.Threshold = Level.All;

            RollingFileAppender rollingFileAppender = new RollingFileAppender();
            rollingFileAppender.AppendToFile = true;
            rollingFileAppender.DatePattern = "yyyy-MM-dd'.log'";
            rollingFileAppender.File = "log\\" + name + "\\";
            rollingFileAppender.ImmediateFlush = true;
            rollingFileAppender.Name = newname;
            rollingFileAppender.RollingStyle = RollingFileAppender.RollingMode.Date;
            rollingFileAppender.StaticLogFileName = false;

            var lockmodel = new FileAppender.InterProcessLock();
            lockmodel.CurrentAppender = rollingFileAppender;
            lockmodel.ActivateOptions();            
            rollingFileAppender.LockingModel = lockmodel;

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date [%thread] %-5level - %message%newline";
            patternLayout.ActivateOptions();
            rollingFileAppender.Layout = patternLayout;

            rollingFileAppender.LockingModel.ActivateOptions();
            rollingFileAppender.ActivateOptions();
            BasicConfigurator.Configure(repository, rollingFileAppender);

            return log4net.LogManager.GetLogger(repository.Name, newname);
        }


        //public static ILog GetSQLLogger(Type model)
        //{
        //    string tablename = "Log4Net_" + model.Name;
        //    return GetSQLLogger(tablename, model);
        //}
        /// <summary>
        /// 记录数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public static ILog GetSQLLogger(string tablename, Type model)
        //{
        //    if (model == null)
        //        return null;
        //    string repositoryName = System.Text.RegularExpressions.Regex.Replace(tablename, @"[^\w]", "");
        //    repositoryName = tablename.ToUpper();
        //    repositoryName = "SQLR_" + repositoryName;
        //    string appenderName = "SQLLOG_" + repositoryName;
        //    ILoggerRepository repository = null;
        //    try
        //    {
        //        ILoggerRepository[] repositories = LogManager.GetAllRepositories();
        //        if (repositories != null && repositories.Length > 0)
        //        {
        //            repository = repositories.FirstOrDefault(r => r.Name == repositoryName);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //    }

        //    if (repository != null)
        //        return LogManager.GetLogger(repositoryName, appenderName);

        //    try
        //    {
        //        repository = LogManager.CreateRepository(repositoryName);
        //    }
        //    catch (Exception)
        //    {
        //        repository = LogManager.GetRepository(repositoryName);
        //    }

        //    repository.Threshold = Level.All;

        //    AdoNetAppender appender = new AdoNetAppender();
        //    appender.BufferSize = 1;
        //    appender.Name = appenderName;
        //    appender.ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
        //    var connection = System.Configuration.ConfigurationManager.ConnectionStrings["Log4netSQLConnection"];
        //    if (connection == null)
        //    {
        //        return null;
        //    }
        //    appender.ConnectionString = connection.ConnectionString;

        //    List<string> columns = new List<string>();
        //    foreach (PropertyInfo property in model.GetProperties())
        //    {
        //        columns.Add(property.Name);
        //    }
        //    if (columns.Count == 0)
        //        return null;
        //    string commandText = "insert into " + tablename + " ([" + string.Join("],[", columns) + "]) values (@log_" + string.Join(",@log_", columns) + ")";

        //    appender.CommandText = commandText;

        //    columns.ForEach((column) =>
        //    {
        //        AdoNetAppenderParameter parameter = new AdoNetAppenderParameter();
        //        parameter.ParameterName = "@log_" + column;
        //        PatternLayout patternLayout = new CustomLayout();
        //        patternLayout.ConversionPattern = "%property{" + column + "}";
        //        patternLayout.ActivateOptions();
        //        parameter.Layout = new Layout2RawLayoutAdapter(patternLayout);
        //        appender.AddParameter(parameter);
        //    });

        //    appender.ActivateOptions();
        //    BasicConfigurator.Configure(repository, appender);

        //    return log4net.LogManager.GetLogger(repository.Name, appenderName);

        //}

    }
}
