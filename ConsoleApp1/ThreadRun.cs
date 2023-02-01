using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class ThreadRun
    {
        static readonly object Identity = new object();
        public void output()
        {
            // Enter the lock to the thread
            lock (Identity)
            {
                // initialize the integer to be used in the for loop
                string strArray = "Hi programmer";

                int y;
                y = 0;

                for (y = y; y < strArray.Length; y++)
                {
                    Console.Write($"{strArray[y]}");
                    Thread.Sleep(TimeSpan.FromSeconds(0.1));
                }

                Console.Write(" ");
            }
        }
    }
}
