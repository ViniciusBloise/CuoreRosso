using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CuoreRosso.Data;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.Threading;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace CuoreRosso
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo(baseCulture),
                    new CultureInfo(enUSCulture)
                 };

                options.DefaultRequestCulture = new RequestCulture(culture: baseCulture, uiCulture: baseCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
                {
                    // My custom request culture logic
                    return new ProviderCultureResult(baseCulture);
                }));
            });
            services.AddSingleton<Microsoft.AspNetCore.Mvc.DataAnnotations.IValidationAttributeAdapterProvider, LocalizedValidationAttributeAdapterProvider>();

            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Account/Manage");
                    options.Conventions.AuthorizePage("/Account/Logout");
                })
                // Add support for finding localized views, based on file name suffix, e.g. Index.fr.cshtml
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                // Add support for localizing strings in data annotations (e.g. validation messages) via the
                // IStringLocalizer abstractions.
                .AddDataAnnotationsLocalization();

            // Register no-op EmailSender used by account confirmation and password reset during development
            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
            //services.AddSingleton<IEmailSender, EmailSender>();
        }

        private const string enUSCulture = "en-US";
        private const string baseCulture = "pt-BR";

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("en"),
                new CultureInfo("pt-BR"),
                new CultureInfo("it-IT")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(supportedCultures[2]), //"pt-BR"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();

            app.UseRequestLocalization(locOptions.Value);
            app.UseStaticFiles();
            // To configure external authentication, 
            // see: http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public class LocalizedValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
        {
            private readonly ValidationAttributeAdapterProvider _originalProvider = new ValidationAttributeAdapterProvider();

            public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
            {
                attribute.ErrorMessage = attribute.FormatErrorMessage("{0}");

                return _originalProvider.GetAttributeAdapter(attribute, stringLocalizer);
            }
        }
    }
}
