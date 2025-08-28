using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PokeFormApp.Modules
{
    public class AppModule : Module
    {
        private readonly string _token;

        public AppModule(string token)
        {
            _token = token;
        }

        protected override void Load(ContainerBuilder builder)
        {

            builder.Register(c =>
            {
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://localhost:7091/");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                return httpClient;
            }).SingleInstance();

           
        }
    }
}
