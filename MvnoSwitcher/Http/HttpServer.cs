using MvnoSwitcher.MobileConfig;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace MvnoSwitcher.Http
{
    public class HttpServer : IDisposable
    {
        HttpContentHandler _contentHandler;

        public string Token { get; private set; }

        public HttpServer()
        {
            _contentHandler = new HttpContentHandler("http://127.0.0.1:18080/");
            Token = GenHash();
        }

        private string GenHash()
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var bs = sha1.ComputeHash(Guid.NewGuid().ToByteArray());
            var sb = new StringBuilder();
            foreach(var b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        public void Dispose()
        {

        }

        public void Start()
        {
            //_contentHandler.Get("/config", (req, res) =>
            //{
            //    res.Send("<h1>Hello, World</h1>");
            //});
            _contentHandler.Get("/ondemand", (req, res) =>
            {
                // generate mobileconfig
                var args = req.Url.Query.Substring(1).Split('&').Select(s => s.Split('=')).ToDictionary(s => s[0], s => WebUtility.UrlDecode(s[1]));
                try
                {
                    var config = new ConfigGenerator();
                    config.Name = args["name"];
                    config.AuthenticationType = args["type"];
                    config.Username = args["username"];
                    config.Password = args["password"];
                    config.Apn = args["apn"];
                    // gen hash
                    var key = AppDelegate.Current.HttpServer.Token;
                    var digest = config.GetDigest(key);
                    if(digest != args["token"])
                    {
                        res.Send("error");
                        return;
                    }
                    // send it
                    res.Send(config.Generate(), "application/x-apple-aspen-config");
                }
                catch (Exception e)
                {
                    res.Send("error");
                }
            });
            _contentHandler.Start();
        }

        private bool IsValidAuthorization(string v)
        {
            var parts = v.Split(' ');
            if (parts.Length != 2) return false;
            if (parts[0] != "Basic") return false;
            var user = Encoding.UTF8.GetString(Convert.FromBase64String(parts[1]));
            return user == $"{Token}:";
        }
    }
}