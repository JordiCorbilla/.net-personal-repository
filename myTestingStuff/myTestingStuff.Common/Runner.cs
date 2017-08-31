using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace myTestingStuff.Common
{
    public class Runner
    {
        //Just adding some CPU load
        public void Run()
        {
            int i = 1000;
            while (i > 0)
            {
                Thread.Sleep(1);
                i--;
            }
        }

        public async Task<int> RunAsync() // we assume we return something after this operation 
        {
            await Task.Run(() =>
            {
                int i = 1000;
                while (i > 0)
                {
                    Thread.Sleep(1);
                    i--;
                }
            }); //1 seconds delay
            return 1;
        }
    }
}
