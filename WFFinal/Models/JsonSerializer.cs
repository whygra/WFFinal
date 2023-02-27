using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WFFinal.Models
{
    internal class JsonSerializer<T> : ILoader<T>, ISaver<T>
    {
        // чтение
        public T Load(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }


        // запись
        public void Save(T data, string path)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(path, json);
        }
    }
}
