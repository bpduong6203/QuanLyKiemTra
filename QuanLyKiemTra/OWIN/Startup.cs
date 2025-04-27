using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MyApp.OWIN.Startup))]
namespace MyApp.OWIN
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Hello from OWIN!");
                await next.Invoke();
            });
        }
    }
}
