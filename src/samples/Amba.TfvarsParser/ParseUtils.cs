using System.Text;
using Amba.TfvarsParser.Model;

namespace Amba.TfvarsParser;

public static class ParseUtils
{
    public static StringBuilder Traverse(JSon? json, StringBuilder? sb = null, int level = 1)
    {
        sb ??= new StringBuilder();
        if (json == null)
        {
            return sb;
        }
        if (json is JObject jObject) 
        {
            sb.Append("{\n");
            foreach (var pair in jObject.Values)
            {
                sb.Append(new string(' ', level * 4));
                sb.Append(pair.Key);
                sb.Append(" = ");
                Traverse(pair.Value, sb, level + 1);
                sb.Append(",\n");
            }

            sb.Append(new string(' ', (level - 1) * 4));
            sb.Append("}");
        }
        else if (json is JList list)
        {
            sb.Append("[");
            foreach (var item in list.Items)
            {
                Traverse(item, sb, level + 1);
                sb.Append(", ");
            } 
            sb.Append("]");
        }
        else if (json is JValue value)
        {
            sb.Append(value.ToString());
        }
        else if (json is JNull)
        {
            sb.Append("null");
        }

        return sb;
    } 
}