﻿using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OwinApp.Middleware
{
    public class StaticFiles : OwinMiddleware
    {
        private readonly string _basePath;
        private readonly IDictionary<string, string> _contentTypeByExtension = new Dictionary<string, string> 
        {
            {".html", "text/html"},
            {".js", "text/javascript"}
        };

        public StaticFiles(OwinMiddleware next, string basePath) : base(next)
        {
            _basePath = basePath;
        }

        public override Task Invoke(IOwinContext context)
        {
            var requestPath = context.Request.Path.ToString();

            if (requestPath == "/")
            {
                requestPath = "/index.html";
            }

            var filePath = Path.Combine(_basePath, requestPath.Substring(1));

            if (File.Exists(filePath))
            {
                SetContentType(filePath, context.Response);

                Console.WriteLine("Serving {0} as {1}", filePath, context.Response.ContentType);
                
                var bytes = File.ReadAllBytes(filePath);
                context.Response.Body.Write(bytes, 0, bytes.Length);
            }

            return Next.Invoke(context);
        }

        private void SetContentType(string filePath, IOwinResponse response)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();

            string contentType;
            if (_contentTypeByExtension.TryGetValue(extension, out contentType))
            {
                response.ContentType = contentType;
            }
            else
            {
                response.ContentType = "text/plain";
            }
        }
    }
}
