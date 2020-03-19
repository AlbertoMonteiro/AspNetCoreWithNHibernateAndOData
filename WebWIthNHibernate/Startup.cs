using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            var dbConfiguration = Fluently.Configure()
                                      .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connStr))
                                      .Mappings(m => m.FluentMappings.AddFromAssembly(GetType().Assembly));
            var _sessionFactory = dbConfiguration
                                      .BuildSessionFactory();

            //if you need to create database uncomment those lines run once then comment again, because it will replace your table when it runs
            //var exporter = new SchemaExport(dbConfiguration.BuildConfiguration());
            //exporter.Execute(true, true, false);

            services.AddScoped(factory => _sessionFactory.OpenSession());
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
