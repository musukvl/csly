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
            switch (value)
            {
                case string s:
                    return $"\"{s}\"";
                case int i:
                    return i.ToString();
                case double d:
                    return d.ToString();
                case bool b:
                    return b.ToString().ToLower();
                case Decimal d:
                    return d.ToString();
                default:
                    return "unknown";
                
            } 
        }
    }
}