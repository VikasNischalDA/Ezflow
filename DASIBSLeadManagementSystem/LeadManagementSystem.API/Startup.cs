using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using StructureMap;
using System.Globalization;
using LeadManagementSystem.Contracts.Request;
using LeadManagementSystem.Data;
using LeadManagementSystem.Providers.HttpProvider;
using LeadManagementSystem.Providers.Interface;
using LeadManagementSystem.API.AutoMapper;

namespace LeadManagementSystem.API
{
    public class Startup
    {
        string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.AllowAnyOrigin();  //set the allowed origin
                        policy.AllowAnyMethod();
                        policy.AllowAnyHeader();
                    });
            });
            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                //options.InputFormatters.RemoveType<SystemTextJsonInputFormatter>();
                //options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();

                //// Add XML formatters
                options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            }).AddXmlSerializerFormatters()
            .AddXmlDataContractSerializerFormatters()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.DictionaryKeyPolicy = null;
            });

            // Register MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));

            // Register Scoped Services
            services.AddScoped<IProvider<DbsRequest, DbsServiceCall>, ExternalDbsProvider>();
            services.AddScoped<IDalasProvider, ExternalDalasProvider>();
            services.AddScoped<SmsService>();

            // Register Authentication
            services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v0", new OpenApiInfo { Title = "Lead Management System.", Version = "v0" });

                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
               

            });


            // Register DbContext
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration["Database:ConnectionString"]);
            });

            // Register HttpClient and AutoMapper
            services.AddHttpClient();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Configure StructureMap
            var container = new Container();
            container.Configure(config =>
            {
                config.AddRegistry(new ApplicationRegistry(Configuration));
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
           // app.UseMiddleware<XmlMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("v0/swagger.json", "Lead Management Service");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
