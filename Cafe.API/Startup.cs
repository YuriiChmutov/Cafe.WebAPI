using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Cafe.API.Data;
using Microsoft.EntityFrameworkCore;
using Cafe.API.Models.Repository;
using Cafe.API.Models.DataManager;
using Cafe.API.Models.Entities;

namespace Cafe.API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<CafeDbContext>(option =>
            option.UseSqlServer(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog=CafeDb"));

            services.AddScoped<IDataRepository<Category>, CategoryDataManager>();
            services.AddScoped<IDataRepository<Client>, ClientDataManager>();
            services.AddScoped<IDataRepository<Product>, ProductDataManager>();
            services.AddScoped<IDataRepository<ClientProduct>, SaleDataManager>();

            services.AddSwaggerDocument();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, CafeDbContext context)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            context.Database.EnsureCreated();
            app.UseMvc();

            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
