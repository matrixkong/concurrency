using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Threading
{
    public class PassingParametersToThreadsExample
    {
        public static void RunSimpleSolution()
        {
            const string url ="http://test.com.au";
            var simpleSolution = new SimpleSolution(url);
            new Thread(new ThreadStart(simpleSolution.Fetch)).Start();
        }

        public static void RunAnonymousemehtodSolution()
        {
            ThreadStart start = () => AnonymousMehtodSolution.Fetch("http://test.com.au");
            new Thread(start).Start();
        }

        public static void RunThreadPoolSolution()
        {
            const string url = "http://test.com.au";
            WaitCallback callback = state => AnonymousMehtodSolution.Fetch((string) state);
            ThreadPool.QueueUserWorkItem(callback, url);
        }

        public static void RunParameterizedThreadStart()
        {
            const string url = "http://test.com.au";
            var t = new Thread(new ParameterizedThreadStart(AnonymousMehtodSolution.Fetch));
            t.Start(url);
        }
    }

    public class SimpleSolution
    {
        private readonly string _url;

        public SimpleSolution(string url)
        {
            this._url = url;
        }

        public void Fetch()
        {
            Console.WriteLine(_url);
        }
    }

    public class AnonymousMehtodSolution
    {
        public static void Fetch(string url)
        {
            Console.WriteLine(url);
        }
        public static void Fetch(object url)
        {
            Console.WriteLine(url);
        } 
    }
}
