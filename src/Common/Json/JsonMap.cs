using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace AstralKeks.Workbench.Common.Json
{
    public class JsonMap : IDictionary, IJsonObject
    {
        private readonly IJsonObject _impl;
        private readonly object _syncRoot;

        internal JsonMap(IJsonObject impl)
        {
            if (impl == null)
                throw new ArgumentNullException(nameof(impl));

            _impl = impl;
            _syncRoot = new object();
        }

        private void AddValue(string key, object value)
        {
            var innerObject = Read();
            innerObject.Add(key, value.ToJson());
            _impl.Write(innerObject);
        }

        private void SetValue(string key, object value)
        {
            var innerObject = Read();
            innerObject[key] = value.ToJson();
            Write(innerObject);
        }

        private bool ContainsValue(string key, object value, bool checkValue)
        {
            var innerObject = Read();
            return innerObject[key] != null && (!checkValue || innerObject[key] == value.ToJson());
        }

        private object GetValue(string key)
        {
            var innerObject = Read();
            return innerObject[key].ToObject(_impl);
        }

        private int GetCount()
        {
            var innerObject = Read();
            return innerObject.Count;
        }

        private void RemoveValue(string key)
        {
            var innerObject = Read();
            innerObject.Remove(key);
            Write(innerObject);
        }

        private string[] AllKeys()
        {
            var innerObj = Read();
            return innerObj.Properties().Select(p => p.Name).ToArray();
        }

        private object[] AllValues()
        {
            var innerObj = Read();
            return innerObj.Properties().Select(p => p.Value.ToObject(_impl)).ToArray();
        }

        private IEnumerable<KeyValuePair<string, object>> AllPairs()
        {
            var innerObj = Read();
            return innerObj.Properties().Select(p => new KeyValuePair<string, object>(p.Name, p.Value.ToObject(_impl)));
        }

        #region JsonObject
        private JObject Read()
        {
            return Read<JObject>();
        }

        private void Write(JObject obj)
        {
            Write<JObject>(obj);
        }

        public TObject Read<TObject>()
        {
            return _impl.Read<TObject>();
        }

        public void Write<TObject>(TObject obj)
        {
            _impl.Write(obj);
        }

        public void Remove()
        {
            _impl.Remove();
        }
        #endregion

        #region Dictionary
        public ICollection Keys => AllKeys();
        public ICollection Values => AllValues();
        public int Count => GetCount();
        public bool IsReadOnly => false;
        public bool IsFixedSize => false;
        public bool IsSynchronized => false;
        public object SyncRoot => _syncRoot;

        public object this[object key]
        {
            get { return GetValue((string)key); }
            set { SetValue((string)key, value); }
        }

        public void CopyTo(Array array, int index)
        {
            AllKeys().CopyTo(array, index);
        }

        public void Remove(object key)
        {
            RemoveValue((string)key);
        }

        public bool Contains(object key)
        {
            return ContainsValue((string)key, null, false);
        }

        public void Add(object key, object value)
        {
            AddValue((string)key, value);
        }

        public void Clear()
        {
            Remove();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            var innerObj = Read();
            return new JsonMapEnumerator(innerObj);
        }

        public IEnumerator GetEnumerator()
        {
            return AllPairs().GetEnumerator();
        }

        private class JsonMapEnumerator : IDictionaryEnumerator
        {
            private readonly IEnumerator<KeyValuePair<string, JToken>> _innerObjEnumerator;

            public JsonMapEnumerator(JObject innerObj)
            {
                _innerObjEnumerator = innerObj.GetEnumerator();
            }

            public bool MoveNext()
            {
                var success = _innerObjEnumerator.MoveNext();
                return success;
            }

            public void Reset()
            {
                _innerObjEnumerator.Reset();
            }

            public object Current => Entry;
            public object Key => _innerObjEnumerator.Current.Key;
            public object Value => _innerObjEnumerator.Current.Value;
            public DictionaryEntry Entry => new DictionaryEntry(Key, Value);
        }
        #endregion
    }
}
