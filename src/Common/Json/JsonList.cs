using System;
using System.Collections;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace AstralKeks.Workbench.Common.Json
{
    public class JsonList : IList, IJsonObject
    {
        private readonly IJsonObject _impl;
        private readonly object _syncRoot;

        public JsonList(IJsonObject impl)
        {
            if (impl == null)
                throw new ArgumentNullException(nameof(impl));

            _impl = impl;
            _syncRoot = new object();
        }
        
        private int AddValue(object value)
        {
            var innerObject = Read();
            innerObject.Add(value.ToJson());
            _impl.Write(innerObject);
            return innerObject.Count - 1;
        }

        private void SetValue(int index, object value)
        {
            var innerObject = Read();
            innerObject[index] = value.ToJson();
            Write(innerObject);
        }

        private void InsertValue(int index, object value)
        {
            var innerObject = Read();
            innerObject.Insert(index, value.ToJson());
            Write(innerObject);
        }

        private bool ContainsValue(object value)
        {
            var innerObject = Read();
            return innerObject.Contains(value.ToJson());
        }

        private ICollection AllValues()
        {
            var innerObject = Read();
            return innerObject.Select(json => json.ToObject(_impl)).ToArray();
        }

        private IEnumerable EnumerateValues()
        {
            var innerObject = Read();
            return innerObject.Select(json => json.ToObject(_impl));
        }

        private object GetValue(int index)
        {
            var innerObject = Read();
            return innerObject[index].ToObject(_impl);
        }

        private int GetCount()
        {
            var innerObject = Read();
            return innerObject.Count;
        }

        private void RemoveValueAt(int index)
        {
            var innerObject = Read();
            innerObject.RemoveAt(index);
            Write(innerObject);
        }

        #region JsonObject

        private JArray Read()
        {
            return Read<JArray>();
        }

        private void Write(JArray obj)
        {
            Write<JArray>(obj);
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

        #region List
        public int Count => GetCount();
        public object SyncRoot => _syncRoot;
        public bool IsSynchronized => false;
        public bool IsReadOnly => false;
        public bool IsFixedSize => false;
        public object this[int index]
        {
            get { return GetValue(index); }
            set { SetValue(index, value); }
        }

        public void CopyTo(Array array, int index)
        {
            AllValues().CopyTo(array, index);
        }

        public int Add(object value)
        {
            return AddValue(value);
        }

        public bool Contains(object value)
        {
            return ContainsValue(value);
        }

        public void Clear()
        {
            Remove();
        }

        public int IndexOf(object value)
        {
            throw new NotSupportedException();
        }

        public void Insert(int index, object value)
        {
            InsertValue(index, value);
        }

        public void Remove(object value)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            RemoveValueAt(index);
        }

        public IEnumerator GetEnumerator()
        {
            return EnumerateValues().GetEnumerator();
        }
        #endregion
    }
}
