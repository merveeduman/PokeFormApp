using Autofac;
using PokeFormApp.Services;

namespace PokeFormApp.Autofac
{
    internal class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpRequest>().As<IHttpRequest>();
        }
    }
}
