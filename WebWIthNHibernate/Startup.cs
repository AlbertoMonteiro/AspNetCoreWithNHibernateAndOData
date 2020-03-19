using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NHibernate.Tool.hbm2ddl;
using System.Linq;

namespace WebWIthNHibernate
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOData();

            var connStr = Configuration.GetConnectionString("DefaultConnection");
            var sessionFactory = Fluently.Configure()
                                      .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connStr).ShowSql())
                                      .Mappings(m => m.FluentMappings.AddFromAssembly(GetType().Assembly))
                                     //if you need to create database uncomment line below run once then comment again, because it will replace your table when it runs
                                      .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true))
                                      .BuildSessionFactory();
            services.AddScoped(factory =>
            {
#if (DEBUG)
                var interceptor = new SqlDebugOutputInterceptor();
                var session = sessionFactory.WithOptions()
                                            .Interceptor(interceptor)
                                            .OpenSession();
#else
    return sessionFactory.OpenSession();
#endif
                return session;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.EnableDependencyInjection();
                endpoints.MaxTop(30).Filter().Count().Select().OrderBy();
            });
        }
    }
}
