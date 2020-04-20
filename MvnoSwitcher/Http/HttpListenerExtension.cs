using System.Net;
using System.IO;

namespace MvnoSwitcher.Http
{
    static class HttpListenerExtension
    {
        /* for HttpListenerResponse */
        public static void Send(this HttpListenerResponse response, string text, string contentType = "text/html")
        {
            response.ContentType = contentType;
            using (var w = new StreamWriter(response.OutputStream))
            {
                w.Write(text);
            }
        }
    }
}