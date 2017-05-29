using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace MvnoSwitcher.Http
{
    class HttpContentHandler
    {
        public delegate void HttpHandlerDelegate(HttpListenerRequest request, HttpListenerResponse response);

        private HttpListener _httpListener;
        private Thread _serverThread;
        private Dictionary<string, HttpHandlerDelegate> _handlers;

        public HttpContentHandler(string prefix)
        {
            _handlers = new Dictionary<string, HttpHandlerDelegate>();
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(prefix);
        }

        public void Start()
        {
            _httpListener.Start();
            RunServerThread();
        }

        public void Get(string path, HttpHandlerDelegate handler)
        {
            _handlers[path] = handler;
        }

        private void RunServerThread()
        {
            _serverThread = new Thread(ServerThreadHandler);
            _serverThread.Start();
        }

        private async void ServerThreadHandler()
        {
            while (true)
            {
                var context = await _httpListener.GetContextAsync();
                if (context.Request.HttpMethod != "GET")
                {
                    SendError(context.Response, 400);
                }
                else
                {
                    if (_handlers.ContainsKey(context.Request.Url.AbsolutePath))
                    {
                        _handlers[context.Request.Url.AbsolutePath](context.Request, context.Response);
                    }
                    else
                    {
                        SendError(context.Response, 404);
                    }
                }
            }
        }

        private void SendError(HttpListenerResponse response, int status)
        {
            response.StatusCode = status;
            response.Close();
        }
    }
}
