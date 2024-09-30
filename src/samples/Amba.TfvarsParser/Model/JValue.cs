namespace Amba.TfvarsParser.Model
{
    public class JValue : JSon
    {
        private readonly object value;

        public JValue(object val)
        {
            value = val;
        }

        public override bool IsValue => true;

        public bool IsString => value is string;

        public bool IsInt => value is int;

        public bool IsDouble => value is double;

        public bool IsBool => value is bool;

        public T GetValue<T>()
        {
            return (T) value;
        }
        
        public override string ToString()
        {
            return value switch
            {
                string s => $"\"{s}\"",
                int i => i.ToString(),
                double d => d.ToString(),
                bool b => b.ToString().ToLower(),
                Decimal d => d.ToString(),
                _ => "unknown"
            };
        }
    }
}