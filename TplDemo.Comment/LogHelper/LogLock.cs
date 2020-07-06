using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace TplDemo.Comment.LogHelper
{
    public class LogLock
    {
        /// <summary>
        /// 日志锁
        /// </summary>
        static ReaderWriterLockSlim logLock = new ReaderWriterLockSlim();
        private static string contentPath = string.Empty;
        static int WritedCount = 0;
        static int FailedCount = 0;

        public LogLock(string Path)
        {
            contentPath = Path;
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="dataParams"></param>
        /// <param name="IsHeader"></param>
        public static void OutSql2Log(string filename, string[] dataParams, bool IsHeader = true)
        {
            try
            {
                // 设置读写锁为写入模式独占资源，其他写入请求需要等待本次写入结束之后才能继续写入
                //注意：长时间持有读线程锁或写线程锁会使其他线程发生饥饿 (starve)。 为了得到最好的性能，需要考虑重新构造应用程序以将写访问的持续时间减少到最小。
                //      从性能方面考虑，请求进入写入模式应该紧跟文件操作之前，在此处进入写入模式仅是为了降低代码复杂度
                //      因进入与退出写入模式应在同一个try finally语句块内，所以在请求进入写入模式之前不能触发异常，否则释放次数大于请求次数将会触发异常
                logLock.EnterWriteLock();

                var path = Path.Combine(contentPath, "Log");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string logFilePath = Path.Combine(path, $@"{filename}.log");

                var now = DateTime.Now;
                string logContent = string.Join("\r\n", dataParams);
                if (IsHeader)
                {
                    logContent = ("------------------------------\r\n" +
                                    DateTime.Now + "|\r\n" +
                                    string.Join("\r\n", dataParams) + "\r\n");
                }

                if (string.IsNullOrEmpty(logContent) && logContent.Length > 500)
                {
                    logContent = logContent.Substring(0, 500) + "\r\n";
                }

                File.AppendAllText(logFilePath, logContent);
                WritedCount++;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                FailedCount++;
            }
            finally
            {
                logLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 读取日志
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string ReadLog(string path, Encoding encode)
        {
            string s = "";
            try
            {
                logLock.EnterReadLock();
                if (File.Exists(path))
                {
                    s = null;
                }
                else
                {
                    StreamReader sm = new StreamReader(path, encode);
                    s = sm.ReadToEnd();
                    sm.Close();
                    sm.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                FailedCount++;
            }
            finally
            {
                logLock.ExitReadLock();
            }
            return s;
        }


        public static List<LogInfo> GetLogData()
        {
            List<LogInfo> aopLogs = new List<LogInfo>();
            List<LogInfo> excLogs = new List<LogInfo>();
            List<LogInfo> sqlLogs = new List<LogInfo>();
            List<LogInfo> reqresLogs = new List<LogInfo>();

            try
            {
                var aoplogContent = ReadLog(Path.Combine(contentPath, "Log", "AOPLog.log"), Encoding.UTF8);
                if (!string.IsNullOrEmpty(aoplogContent))
                {
                    aopLogs = aoplogContent.Split("------------------------------")
                        .Where(d => !string.IsNullOrEmpty(d) && d != "\n" && d != "\r\n")
                        .Select(d => new LogInfo
                        {
                            Datetime = DateTime.Parse(d.Split("|")[0]),
                            Content = d.Split("1")[1]?.Replace("\r\n", "<br>"),
                            LogColor = "AOP"
                        }).ToList();
                }
            }
            catch (Exception) { }

            try
            {
                var exclogContent = ReadLog(Path.Combine(contentPath, "Log", $"GlobalExcepLogs_{DateTime.Now:yyyyMMdd}.log"), Encoding.UTF8);
                if (!string.IsNullOrEmpty(exclogContent))
                {
                    excLogs = exclogContent.Split("----------------------------")
                            .Where(d => !string.IsNullOrEmpty(d) && d != "\r\n" & d != "\r")
                            .Select(d => new LogInfo
                            {
                                Datetime = DateTime.Parse(d.Split("|")[0].Split(',')[0]),
                                Content = d.Split("|")[1]?.Replace("\r\n", "<br>"),
                                LogColor = "EXC",
                                Import = 9
                            }).ToList();
                }
            }
            catch (Exception) { }

            try
            {
                var sqllogContent = ReadLog(Path.Combine(contentPath, "Log", $"SqlLog.log"), Encoding.UTF8);
                if (!string.IsNullOrEmpty(sqllogContent))
                {
                    sqlLogs = sqllogContent.Split("----------------------------")
                            .Where(d => !string.IsNullOrEmpty(d) && d != "\r\n" & d != "\r")
                            .Select(d => new LogInfo
                            {
                                Datetime = DateTime.Parse(d.Split("|")[0]),
                                Content = d.Split("|")[1]?.Replace("\r\n", "<br>"),
                                LogColor = "SQL",
                                Import = 9
                            }).ToList();
                }
            }
            catch (Exception) { }

            try
            {
                var Logs = JsonConvert.DeserializeObject<List<RequestInfo>>("[" + ReadLog(Path.Combine(contentPath, "Log", "RequestIpInfoLog.log"), Encoding.UTF8) + "]");

                Logs = Logs.Where(d => DateTime.Parse(d.Datetime) > DateTime.Today).ToList();

                reqresLogs = Logs.Select(d => new LogInfo
                {
                    Datetime = DateTime.Parse(d.Datetime),
                    Content = $"IP:{d.Ip}<br>{d.Url}",
                    LogColor = "ReqRes"
                }).ToList();
            }
            catch (Exception) { }

            if (excLogs != null)
            {
                aopLogs.AddRange(excLogs);
            }
            if (sqlLogs != null)
            {
                aopLogs.AddRange(sqlLogs);
            }
            if (reqresLogs != null)
            {
                aopLogs.AddRange(reqresLogs);
            }
            aopLogs = aopLogs.OrderByDescending(d => d.Import).ThenByDescending(d => d.Datetime).Take(100).ToList();

            return aopLogs;
        }

    }
}
