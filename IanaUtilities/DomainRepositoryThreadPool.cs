using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace IanaUtilities
{
    public class DomainRepositoryThreadPool : IDomainRepository
    {
        private DataProvider _dataProvider = new DataProvider();

        public IEnumerable<DomainInformation> GetAll()
        {
            var result = new List<DomainInformation>();
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(1, 0, 0);
                var domains = _dataProvider.GetAllDomains(client);
                var doneEvents = new Dictionary<string, ManualResetEvent>();
                foreach (var domain in domains)
                {
                    ManualResetEvent resetEvent = new ManualResetEvent(false);
                    doneEvents.Add(domain, resetEvent);
                    var context = new ThreadContext
                    {
                        Domain = domain,
                        HttpClient = client,
                        ResetEvent = resetEvent,
                        Result = result
                    };

                    ThreadPool.QueueUserWorkItem(ThreadPoolCallback, context);
                }

                doneEvents.Values.ToList().ForEach((element) => element.WaitOne());
            }

            return result;
        }

        private void ThreadPoolCallback(Object threadContext)
        {
            ThreadContext context = (ThreadContext)threadContext;
            try
            {
                context.Result.Add(new DomainInformation
                {
                    Name = context.Domain,
                    WhoisServerName = _dataProvider.GetWhoisServerName(context.Domain, context.HttpClient)
                });
            }
            finally 
            {
                context.ResetEvent.Set();
            }
        }

        private class ThreadContext
        {
            public ManualResetEvent ResetEvent { get; set; }

            public HttpClient HttpClient { get; set; }

            public string Domain { get; set; }

            public List<DomainInformation> Result { get; set; }
        }
    }
}
