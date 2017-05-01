using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContinuationTasks
{
    class Program
    {
        static void Main(string[] args)
        {
            Point1();
            Point2();
            Point3();
            Point4();
            Console.WriteLine("Done!");
        }

        private static void Point1()
        {
            Task.Run(() =>
            {
                Console.WriteLine("Task started");
                throw new Exception();
            })
            .ContinueWith((data) =>
            {
                Console.WriteLine("regardless of the result continuation");
            })
            .Wait();
        }

        private static void Point2()
        {
            Task.Run(() =>
            {
                Console.WriteLine("Task started");
                throw new Exception();
            })
            .ContinueWith((data) =>
            {
                Console.WriteLine("OnlyOnFaulted continuation");
            }, TaskContinuationOptions.OnlyOnFaulted)
            .Wait();
        }

        private static void Point3()
        {
            Task.Run(() =>
            {
                Console.WriteLine("Task started");
                throw new Exception();
            })
            .ContinueWith((data) =>
            {
                Console.WriteLine("AttachedToParent continuation");
            }, TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnFaulted)
            .Wait();
        }

        private static void Point4()
        {
            Task.Run(() =>
            {
                Console.WriteLine("Task started");
                throw new TaskCanceledException();
            })
            .ContinueWith((data) =>
            {
                Console.WriteLine("ExecuteSynchronously continuation");
            }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.ExecuteSynchronously )
            .Wait();
        }
    }
}
