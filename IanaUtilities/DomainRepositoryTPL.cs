using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IanaUtilities
{
    public class DomainRepositoryTPL : IDomainRepository
    {
        private DataProvider _dataProvider = new DataProvider();

        public IEnumerable<DomainInformation> GetAll()
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(1, 0, 0);
                var domains = _dataProvider.GetAllDomains(client);
                var tasks = GetTasks(client, domains);
                Task.WaitAll(tasks.Values.ToArray());

                return GetResult(domains, tasks);
            }
        }

        private Dictionary<string, Task<string>> GetTasks(HttpClient client, IEnumerable<string> domains)
        {
            var tasks = new Dictionary<string, Task<string>>();
            foreach (var domain in domains)
            {
                tasks.Add(domain, Task.Run(() => _dataProvider.GetWhoisServerName(domain, client)));
            }

            return tasks;
        }

        private static IEnumerable<DomainInformation> GetResult(IEnumerable<string> domains, Dictionary<string, Task<string>> tasks)
        {
            var result = new List<DomainInformation>();
            foreach (var domain in domains)
            {
                result.Add(new DomainInformation
                {
                    Name = domain,
                    WhoisServerName = tasks[domain].Result
                });
            }

            return result;
        }
    }
}
