using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ambiel.Utility.Thread
{
    /// <summary>
    /// 处理任务的返回值
    /// </summary>
    public class AsynResult
    {
        public static AsynResult CreateUnsuccress(Exception ex)
        {
            var result = new AsynResult();
            result.Exception = ex;
            result.Message = ex.Message;
            result.Success = false;
            return result;
        }

        public static AsynResult CreateSuccess(string msg = "")
        {
            var result = new AsynResult();
            result.Message = msg;
            result.Success = true;
            return result;
        }

        public bool Success { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }
        
    }
    public class ThreadWorker
    {
        private static Task mtTask;

        static ThreadWorker()
        {
            mtTask = new Task(Start);
            mtTask.Start();
            mtTask.ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    Console.WriteLine("异步线程管理器主线程出现异常");
                }
            }, TaskContinuationOptions.NotOnRanToCompletion);
        }
        static AutoResetEvent sync = new AutoResetEvent(true);
        static void Start()
        {
            while (true)
            {
                if (allTasks.Count == 0)
                    sync.WaitOne();
                else
                {
                    Task curTask;
                    if (allTasks.TryDequeue(out curTask))
                    {
                        curTask.Start();
                        curTask = null;
                    }
                }
            }
        }
        static  ConcurrentQueue<Task> allTasks = new ConcurrentQueue<Task>();

        public static void QueueTask(Action action, Action<AsynResult> callback)
        {
            var task = new Task(action);
            task.ContinueWith(t => callback(AsynResult.CreateUnsuccress(t.Exception)),
                TaskContinuationOptions.NotOnRanToCompletion);
            task.ContinueWith(t => callback(AsynResult.CreateSuccess()), TaskContinuationOptions.OnlyOnRanToCompletion);
            allTasks.Enqueue(task);
            sync.Set();
        }

        public static void BuildExceptionMessage(Exception ex, StringBuilder sb)
        {
            if(ex==null) return;
            if (string.IsNullOrWhiteSpace(ex.Message) == false) sb.AppendLine(ex.Message);
            BuildExceptionMessage(ex.InnerException,sb);
        }
    }
}