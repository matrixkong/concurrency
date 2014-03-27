using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threading
{
    public class MainEntry
    {
        public static void Main()
        {
            //DeadLockExample.Run();
            //MonitorClassExample.Run();
            PassingParametersToThreadsExample.Run();   
            Console.ReadKey();
        }
    }
}
