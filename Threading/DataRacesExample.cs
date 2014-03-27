using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Threading
{
    public class DataRacesExample
    {
        static int count = 0;

        public static void Run()
        {
            var job = new ThreadStart(ThreadJob);
            var thread = new Thread(job);
            thread.Start();

            for (int i = 0; i < 5; i++)
            {
                count++;
            }

            thread.Join();
            Console.WriteLine("Final count: {0}", count);
        }

        static void ThreadJob()
        {
            for (int i = 0; i < 5; i++)
            {
                count++;
            }
        }
    }
}
