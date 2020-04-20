using System;
using MvnoSwitcher.Plist;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MvnoSwitcher.MobileConfig
{
    public class ConfigGenerator
    {
        public string Name { get; set; }
        public string AuthenticationType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Apn { get; set; }
		public int ProtocolMask { get; set; }

        public ConfigGenerator()
        {
            AuthenticationType = "CHAP";
            ProtocolMask = 3;
        }

        public string Generate()
        {
            var plist = new PlistRoot();
            var root = (PlistDict)(plist.Content = new PlistDict());
            root.Append("PayloadContent",
                new PlistArray().Append(
                    new PlistDict().Append("AttachAPN",
                        new PlistDict().Append("Name", Apn)
                        .Append("AuthenticationType", AuthenticationType)
                        .Append("Username", Username)
                        .Append("Password", Password)
                        .Append("DefaultProtocolMask", ProtocolMask)
                        .Append("AllowedProtocolMask", ProtocolMask)
                    )
                    .Append("APNs",
                        new PlistArray().Append(
                            new PlistDict().Append("Name", Apn)
                            .Append("AuthenticationType", AuthenticationType)
                            .Append("Username", Username)
                            .Append("Password", Password)
                            .Append("DefaultProtocolMask", ProtocolMask)
                            .Append("AllowedProtocolMask", ProtocolMask)
                        )
                    )
                    .Append("PayloadDescription", "Provides customization of carrier Access Point Name.")
                    .Append("PayloadDisplayName", "Advanced Settings")
                    .Append("PayloadIdentifier", $"mo.{Apn}.generated")
                    .Append("PayloadOrganizatoin", "MvnoSwitcher")
                    .Append("PayloadType", "com.apple.cellular")
                    .Append("PayloadUUID", Guid.NewGuid().ToString())
                    .Append("PayloadVersion", 1)
                )
            );
			root.Append("PayloadDescription", $"APN config for {Name}");
            root.Append("PayloadDisplayName", $"{Name} ({Apn})");
            root.Append("PayloadIdentifier", $"mo.{Apn}.generated");
            root.Append("PayloadOrganization", "MvnoSwitcher");
            root.Append("PayloadType", "Configuration");
            root.Append("PayloadUUID", Guid.NewGuid().ToString());
            root.Append("PayloadVersion", 1);
            return plist.Generate();
        }

        public string GetDigest(string key)
        {
            var hmac = System.Security.Cryptography.HMAC.Create();
            hmac.Key = Encoding.UTF8.GetBytes(key);
            var value = $"{Apn}{AuthenticationType}{Name}{Password}{Username}";
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
            var sb = new StringBuilder();
            foreach(var b in hash)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        public string GetQueryString(string key)
        {
            var args = new Dictionary<string, string>();
            args["name"] = Name;
            args["username"] = Username;
            args["password"] = Password;
            args["apn"] = Apn;
            args["type"] = AuthenticationType;
            args["token"] = GetDigest(key);
            return string.Join("&", args.Select(kv => $"{kv.Key}={WebUtility.UrlEncode(kv.Value)}"));
        }
    }
}