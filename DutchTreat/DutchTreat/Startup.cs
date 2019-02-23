using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace DutchTreat
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ProductContext>(cfg=>
            {
                //cfg.UseSqlServer(_config.GetConnectionString("DutchCOnnectionString")));
                cfg.UseInMemoryDatabase("MyDatabase");
            });

            services.AddAutoMapper();

            services.AddTransient<DbSeeder>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Products API",
                        Version = "v1",
                        Description = "Products API",
                        Contact = new Contact
                        {
                            Name = "Name",
                            Url = "https://github.com/therciopo"
                        }
                    }
                    );
            });

            services.AddCors();

            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(ValidateModelFilter));
            })
            .AddJsonOptions(opt=> opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });   

            if(env.IsDevelopment())
            {
                app.UseCors(builder =>
                    builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowCredentials());            
                            
                app.UseDeveloperExceptionPage();
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var seeder = scope.ServiceProvider.GetService<DbSeeder>();
                    seeder.Seed();
                }
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            app.UseExceptionMiddleware();

            app.UseHttpsRedirection();
            app.UseMvc();

        }
    }
}
