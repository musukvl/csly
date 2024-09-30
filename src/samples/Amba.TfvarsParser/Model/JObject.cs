using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Amba.TfvarsParser.Model
{
    public class JObject : JSon, IReadOnlyDictionary<string, JSon>
    {
        private IEnumerable<JSon> _values;

        public JObject(string key, JSon value)
        {
            Values = new Dictionary<string, JSon>();
            Values[key] = value;
        }

        public JObject()
        {
            Values = new Dictionary<string, JSon>();
        }

        public JObject(Dictionary<string, JSon> dic)
        {
            Values = dic;
        }

        public override bool IsObject => true;

        public override bool IsList => true;

        public IEnumerable<string> Keys { get; }

        IEnumerable<JSon> IReadOnlyDictionary<string, JSon>.Values => _values;

        public Dictionary<string, JSon> Values { get; }
        
        

        public int Count => Values.Count;

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out JSon value)
        {
            throw new NotImplementedException();
        }

        public JSon this[string key]
        {
            get => Values[key];
            set => Values[key] = value;
        }


        public void Merge(JObject obj)
        {
            foreach (var pair in obj.Values) this[pair.Key] = pair.Value;
        }
        

        public bool ContainsKey(string key)
        {
            return Values.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<string, JSon>> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Values.GetEnumerator();
        }
    }
}