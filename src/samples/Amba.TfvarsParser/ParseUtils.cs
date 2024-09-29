using System.Text;
using Amba.TfvarsParser.Model;

namespace Amba.TfvarsParser;

public static class ParseUtils
{
    public static StringBuilder Traverse(JSon json, StringBuilder? sb = null, int level = 1)
    {
        sb ??= new StringBuilder();
        if (json.IsObject)
        {
            sb.Append("{\n");
            foreach (var pair in ((JObject) json).Values)
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
        else if (json.IsList)
        {
            sb.Append("[");
            foreach (var item in ((JList) json).Items)
            {
                Traverse(item, sb, level + 1);
                sb.Append(", ");
            } 
            sb.Append("]");
        }
        else if (json.IsValue)
        {
            sb.Append(((JValue)json).ToString());
        }

        return sb;
    } 
}