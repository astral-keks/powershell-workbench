using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AstralKeks.Workbench.Common.Json
{
    public interface IJsonObject
    {
        TObject Read<TObject>();
        void Write<TObject>(TObject obj);
        void Remove();
    }

    public class FileJsonObject : IJsonObject
    {
        private readonly string _filePath;
        
        public FileJsonObject(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            _filePath = filePath;

            var directory = Path.GetDirectoryName(_filePath);
            if (Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        public virtual TObject Read<TObject>()
        {
            return File.Exists(_filePath)
                ? ((JToken) JsonConvert.DeserializeObject(File.ReadAllText(_filePath))).ToObject<TObject>()
                : default(TObject);
        }

        public virtual void Write<TObject>(TObject obj)
        {
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
        
        public virtual void Remove()
        {
            if (File.Exists(_filePath))
                File.Delete(_filePath);
        }
    }

    public class StringJsonObject :  IJsonObject
    {
        private JToken _obj;

        public StringJsonObject(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException(nameof(json));

            _obj = (JToken)JsonConvert.DeserializeObject(json);
        }

        public virtual TObject Read<TObject>()
        {
            if (typeof(TObject) == typeof(JToken))
                return (TObject)(object)_obj;

            return _obj != null ? _obj.ToObject<TObject>() : default(TObject);
        }

        public virtual void Write<TObject>(TObject obj)
        {
            _obj = JToken.FromObject(obj);
        }

        public virtual void Remove()
        {
            _obj = null;
        }
    }

}
