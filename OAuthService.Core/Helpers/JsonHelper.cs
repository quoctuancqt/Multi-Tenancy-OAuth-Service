using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OAuthService.Core.Helpers
{
    public static class JsonHelper
    {
        public static string ObjToJson<TModel>(this TModel model)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(model, jsonSettings);
        }

        public static TModel JsonToObj<TModel>(this string json)
        {
            return JsonConvert.DeserializeObject<TModel>(json);
        }
    }
}
