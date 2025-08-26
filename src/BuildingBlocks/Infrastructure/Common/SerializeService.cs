using Contracts.Common.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class SerializeService : ISerializeServices
    {

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string Serialize<T>(T json)
        {
            return JsonConvert.SerializeObject(json, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                }
            });
        }

        public string Serialize<T>(T obj, Type type)
        {
            return JsonConvert.SerializeObject(obj, type, new JsonSerializerSettings());
        }
    }
}
