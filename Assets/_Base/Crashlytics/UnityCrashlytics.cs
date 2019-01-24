using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BaseFramework
{
    public class UnityCrashlytics
    {
#if !UNITY_EDITOR
        public static Action<string, string> onPostException;

        private static bool initFinished = false;
#endif

        public static void Init()
        {
#if !UNITY_EDITOR
            if(!initFinished)
            {
                initFinished = true;

                Application.logMessageReceived += OnLogMessageReceived;
                AppDomain.CurrentDomain.UnhandledException += OnUncaughtExceptionHandler;
            }
#endif
        }

        private static void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
#if !UNITY_EDITOR
            if(type == LogType.Exception || type == LogType.Error)
            { 
                if (string.IsNullOrEmpty(stackTrace))
                {
                    stackTrace = "Empty";
                }
                else
                {

                    try
                    {
                        string[] frames = stackTrace.Split('\n');

                        if (frames != null && frames.Length > 0)
                        {

                            StringBuilder trimFrameBuilder = new StringBuilder();

                            string frame = null;
                            int count = frames.Length;
                            for (int i = 0; i < count; i++)
                            {
                                frame = frames[i];

                                if (string.IsNullOrEmpty(frame) || string.IsNullOrEmpty(frame.Trim()))
                                {
                                    continue;
                                }

                                frame = frame.Trim();

                                // System.Collections.Generic
                                if (frame.StartsWith("System.Collections.Generic.") || frame.StartsWith("ShimEnumerator"))
                                {
                                    continue;
                                }
                                if (frame.StartsWith("Bugly"))
                                {
                                    continue;
                                }
                                if (frame.Contains("..ctor"))
                                {
                                    continue;
                                }

                                int start = frame.ToLower().IndexOf("(at");
                                int end = frame.ToLower().IndexOf("/assets/");

                                if (start > 0 && end > 0)
                                {
                                    trimFrameBuilder.AppendFormat("{0}(at {1}", frame.Substring(0, start).Replace(":", "."), frame.Substring(end));
                                }
                                else
                                {
                                    trimFrameBuilder.Append(frame.Replace(":", "."));
                                }

                                trimFrameBuilder.AppendLine();
                            }

                            stackTrace = trimFrameBuilder.ToString();
                        }
                    }
                    catch
                    {
                    }
                }
                onPostException.InvokeGracefully(condition, stackTrace);
            }
#endif
        }

        private static void OnUncaughtExceptionHandler(object sender, System.UnhandledExceptionEventArgs args)
        {
#if !UNITY_EDITOR
            if (args == null || args.ExceptionObject == null)
            {
                return;
            }

            try
            {
                if (args.ExceptionObject.GetType() == typeof(System.Exception))
                {
                    HandleException((System.Exception)args.ExceptionObject);
                }
            }
            catch
            {

            }
#endif
        }

        private static void HandleException(System.Exception e)
        {
#if !UNITY_EDITOR
            if (e == null)
            {
                return;
            }

            string name = e.GetType().Name;
            string reason = e.Message;

            StringBuilder stackTraceBuilder = new StringBuilder("");

            StackTrace stackTrace = new StackTrace(e, true);
            int count = stackTrace.FrameCount;
            for (int i = 0; i < count; i++)
            {
                StackFrame frame = stackTrace.GetFrame(i);

                stackTraceBuilder.AppendFormat("{0}.{1}", frame.GetMethod().DeclaringType.Name, frame.GetMethod().Name);

                ParameterInfo[] parameters = frame.GetMethod().GetParameters();
                if (parameters == null || parameters.Length == 0)
                {
                    stackTraceBuilder.Append(" () ");
                }
                else
                {
                    stackTraceBuilder.Append(" (");

                    int pcount = parameters.Length;

                    ParameterInfo param = null;
                    for (int p = 0; p < pcount; p++)
                    {
                        param = parameters[p];
                        stackTraceBuilder.AppendFormat("{0} {1}", param.ParameterType.Name, param.Name);

                        if (p != pcount - 1)
                        {
                            stackTraceBuilder.Append(", ");
                        }
                    }
                    param = null;

                    stackTraceBuilder.Append(") ");
                }

                string fileName = frame.GetFileName();
                if (!string.IsNullOrEmpty(fileName) && !fileName.ToLower().Equals("unknown"))
                {
                    fileName = fileName.Replace("\\", "/");

                    int loc = fileName.ToLower().IndexOf("/assets/");
                    if (loc < 0)
                    {
                        loc = fileName.ToLower().IndexOf("assets/");
                    }

                    if (loc > 0)
                    {
                        fileName = fileName.Substring(loc);
                    }

                    stackTraceBuilder.AppendFormat("(at {0}:{1})", fileName, frame.GetFileLineNumber());
                }
                stackTraceBuilder.AppendLine();
            }

            onPostException.InvokeGracefully(name + "\n" + reason, stackTraceBuilder.ToString());
#endif
        }
    }
}
