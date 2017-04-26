using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IanaUtilities
{
    public class DomainRepositoryAsync : IDomainRepository
    {
        private DataProvider _dataProvider = new DataProvider();

        public IEnumerable<DomainInformation> GetAll()
        {
            var result = new List<DomainInformation>();
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(1, 0, 0);
                var domains = _dataProvider.GetAllDomainsAsync(client).Result;
                var tasks = new Dictionary<string, Task<string>>();
                foreach (var domain in domains)
                {
                    tasks.Add(domain, _dataProvider.GetWHOISServerNameAsync(domain, client));
                }

                Task.WaitAll(tasks.Values.ToArray());

                foreach (var domain in domains)
                {
                    result.Add(new DomainInformation
                    {
                        Name = domain,
                        WHOISServerName = tasks[domain].Result
                    });
                }
            }

            return result;
        }
    }
}
