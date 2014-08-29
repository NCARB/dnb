using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwinApp.Middleware
{
    public class Logger : OwinMiddleware
    {
        public Logger(OwinMiddleware next) : base(next) {}

        public override Task Invoke(IOwinContext context)
        {
            Console.WriteLine(context.Request.Method + " " + context.Request.Uri.ToString());

            return Next.Invoke(context);
        }
    }
}
