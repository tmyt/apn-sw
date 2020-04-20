using System;
using System.Collections.Generic;
using System.Text;

namespace MvnoSwitcher.Plist
{
    abstract class PlistXml
    {
        public abstract string Generate();
    }

    class PlistRoot : PlistXml
    {
        public PlistXml Content { get; set; }

        public override string Generate()
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            sb.AppendLine(@"<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">");
            sb.AppendLine(@"<plist version=""1.0"">");
            sb.AppendLine(Content.Generate());
            sb.AppendLine(@"</plist>");
            return sb.ToString();
        }
    }

    class PlistDict : PlistXml
    {
        private Dictionary<string, object> _values;

        public PlistDict()
        {
            _values = new Dictionary<string, object>();
        }

        public PlistDict Append(string key, object value)
        {
            _values[key] = value;
            return this;
        }

        public override string Generate()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<dict>");
            foreach (var kv in _values)
            {
                sb.AppendLine($"<key>{kv.Key}</key>");
                if (kv.Value is string)
                {
                    sb.AppendLine($"<string>{kv.Value}</string>");
                }
                else if (kv.Value is int)
                {
                    sb.AppendLine($"<integer>{kv.Value}</integer>");
                }
                else if (kv.Value is PlistXml)
                {
                    sb.AppendLine(((PlistXml)kv.Value).Generate());
                }
            }
            sb.AppendLine("</dict>");
            return sb.ToString();
        }
    }

    class PlistArray : PlistXml
    {
        private List<object> _values;

        public PlistArray()
        {
            _values = new List<object>();
        }

        public PlistArray Append(object value)
        {
            _values.Add(value);
            return this;
        }

        public override string Generate()
        {

            var sb = new StringBuilder();
            sb.AppendLine("<array>");
            foreach (var v in _values)
            {
                if (v is string)
                {
                    sb.AppendLine($"<string>{v}</string>");
                }
                else if (v is int)
                {
                    sb.AppendLine($"<integer>{v}</integer>");
                }
                else if (v is PlistXml)
                {
                    sb.AppendLine(((PlistXml)v).Generate());
                }
            }
            sb.AppendLine("</array>");
            return sb.ToString();
        }
    }
}
