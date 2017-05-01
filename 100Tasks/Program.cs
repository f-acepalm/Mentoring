using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _100Tasks
{
    class Program
    {
        static void Main(string[] args)
        {
            var tasks = new List<Task>();
            for (int i = 0; i < 100; i++)
            {
                var taskNumber = i;
                tasks.Add(Task.Run(() => DoSomeWork(taskNumber)));
            }

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Finished");
        }

        public static void DoSomeWork(int number)
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine($"Task {number}: iteration number - {i}");
            }
        }
    }
}
