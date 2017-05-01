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
        private static SemaphoreSlim semaphore;

        public static void Main(string[] arg)
        {
            semaphore = new SemaphoreSlim(0, 2);
            Thread[] threads = new Thread[5];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() => {
                    Console.WriteLine($"Operation #{Thread.CurrentThread.ManagedThreadId} is waiting for the semaphore.");

                    semaphore.Wait();

                    Console.WriteLine($"Operation #{Thread.CurrentThread.ManagedThreadId} enters the semaphore.");

                    Thread.Sleep(2000);

                    Console.WriteLine($"Operation #{Thread.CurrentThread.ManagedThreadId} releases the semaphore; previous count: {semaphore.Release()}.");
                });

                threads[i].Start();
            }

            Thread.Sleep(1000);

            Console.Write("Main thread calls Release(2) --> ");
            semaphore.Release(2);
            Console.WriteLine($"{semaphore.CurrentCount} tasks can enter the semaphore.");

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }

            Console.WriteLine("Main thread exits.");
            Console.ReadLine();
        }
    }
}
