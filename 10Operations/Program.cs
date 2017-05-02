using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _10Operations
{
    class Program
    {
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(2, 2);
        private static Thread[] threads = new Thread[10];

        public static void Main(string[] arg)
        {
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() => Operation());
                threads[i].Start();
            }

            foreach (var item in threads)
            {
                item.Join();
            }

            Console.WriteLine("Done");
        }

        private static void Operation()
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} is waiting");
            _semaphore.Wait();
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} started");
            Thread.Sleep(3000);
            _semaphore.Release();
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} is finished");
        }
    }
}
