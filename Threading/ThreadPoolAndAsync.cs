using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Threading
{
    public class ThreadPoolAndAsync
    {
        public static void Run()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(PrintOut), "Hello");
            ThreadPool.QueueUserWorkItem(new WaitCallback(PrintOut), "Haha");

            int w;
            int c;
            ThreadPool.GetMinThreads(out w, out c);

            // Write the numbers of minimum threads
            Console.WriteLine("{0}, {1}",
                w,
                c);
            // Give the callback time to execute - otherwise the app
            // may terminate before it is called
            Thread.Sleep(1000);
        }

        static readonly object _listLock = new object();

        public static void PrintOut(object parameter)
        {
            //Monitor.Wait(_listLock);
            lock (_listLock)
            {
                Console.WriteLine(parameter);
                // Monitor.Pulse(_listLock);
                Monitor.Pulse(_listLock);
            }

        }


        public static void RunAutoResetEvent()
        {
            // Loop through number of min threads we use
            for (int c = 2; c <= 40; c++)
            {
                // Use AutoResetEvent for thread management
                AutoResetEvent[] arr = new AutoResetEvent[50];
                for (int i = 0; i < arr.Length; ++i)
                {
                    arr[i] = new AutoResetEvent(false);
                }

                // Set the number of minimum threads
                ThreadPool.SetMinThreads(c, 4);

                // Get current time
                long t1 = Environment.TickCount;

                // Enqueue 50 work items that run the code in this delegate function
                for (int i = 0; i < arr.Length; i++)
                {
                    ThreadPool.QueueUserWorkItem(delegate(object o)
                    {
                        //simulate some work;
                        Thread.Sleep(100);
                        arr[(int)o].Set(); // Signals completion 
                    }, i);
                }

                // Wait for all tasks to complete
                WaitHandle.WaitAll(arr);

                // Write benchmark results
                long t2 = Environment.TickCount;
                Console.WriteLine("{0},{1}",
                c,
                t2 - t1);
            }
        }

        static WaitHandle[] waitHandles =  
                {
                    new AutoResetEvent(false),
                    new AutoResetEvent(false)
                };
        public static void RunWaitHandles()
        {
            
            // Queue up two tasks on two different threads;  
            // wait until all tasks are completed.
            DateTime dt = DateTime.Now;
            Console.WriteLine("Main thread is waiting for BOTH tasks to complete.");
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoTask), waitHandles[0]);
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoTask), waitHandles[1]);
            WaitHandle.WaitAll(waitHandles);
            // The time shown below should match the longest task.
            Console.WriteLine("Both tasks are completed (time waited={0})",
                (DateTime.Now - dt).TotalMilliseconds);

            // Queue up two tasks on two different threads;  
            // wait until any tasks are completed.
            dt = DateTime.Now;
            Console.WriteLine();
            Console.WriteLine("The main thread is waiting for either task to complete.");
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoTask), waitHandles[0]);
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoTask), waitHandles[1]);
            int index = WaitHandle.WaitAny(waitHandles);
            // The time shown below should match the shortest task.
            Console.WriteLine("Task {0} finished first (time waited={1}).",
                index + 1, (DateTime.Now - dt).TotalMilliseconds);
        }
        static Random r = new Random();
        static void DoTask(Object state)
        {
            var are = (AutoResetEvent)state;
            int time = 1000 * r.Next(2, 10);
            Console.WriteLine("Performing a task for {0} milliseconds.", time);
            Thread.Sleep(time);
            are.Set();
        }
    }
}
