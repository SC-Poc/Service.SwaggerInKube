using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Service.SwaggerInKube
{
    public static class SwagerInKubeInfo
    {
        public static SwaggerInfo[] GetAllWaggers(string @namespace, string kubeapihost, string token)
        {
            var url = $"{kubeapihost.Trim('/')}/api/v1/namespaces/{@namespace}/pods";

            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var json = client.GetStringAsync(url).GetAwaiter().GetResult();
            var data = JsonConvert.DeserializeObject<Data>(json);

            foreach (var item in data.items)
            {
                Console.WriteLine($"{item.metadata.labels["app"]}.{item.metadata.@namespace}");
            }

            return data.items.Select(i => new SwaggerInfo()
            {
                Name = $"{i.metadata.labels["app"]}.{i.metadata.@namespace}",
                Url = $"http://{i.metadata.labels["app"]}.{i.metadata.@namespace}.svc.cluster.local/swagger/v1/swagger.json"
            }).ToArray();
        }

        

        public class SwaggerInfo
        {
            public string Url { get; set; }
            public string Name { get; set; }

        }

        public class Data
        {
            public Item[] items { get; set; }
        }

        public class Item
        {
            public Metadata metadata { get; set; }
        }

        public class Metadata
        {
            public Dictionary<string, string> labels { get; set; }
            public string @namespace { get; set; }
        }
    }
}