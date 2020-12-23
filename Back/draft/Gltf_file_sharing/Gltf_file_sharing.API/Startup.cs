using Gltf_file_sharing.Core.Databases;
using Gltf_file_sharing.Core.EF;
using Gltf_file_sharing.Core.Repositories;
using Gltf_file_sharing.Core.Services;
using Gltf_file_sharing.Core.Services.Impl;
using Gltf_file_sharing.Data.Repositories;
using Gltf_file_sharing.Data.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Gltf_file_sharing.API
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
            services
                .AddControllers()
                .AddNewtonsoftJson(
                x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            AddRepositories(services);

            AddSettings(services);

            AddServices(services);

            AddDbConnection(services);

            AddCorsConfiguration(services);
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
            });
        }

        #region Private methods
        private static void AddRepositories(IServiceCollection services)
        {
            services.AddTransient<IGltfFileRepository, GltfFileRepository>();
            services.AddSingleton<ModificationRepository>();
            services.AddSingleton<ModelsRepository>();
        }

        private static void AddServices(IServiceCollection services)
        {


            services.AddSingleton<IModelsDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<ModelsDatabaseSettings>>().Value);
            services.AddTransient<IStorageService, StorageService>();
            services.AddTransient<IModificationService, ModificationService>();
        }

        private static void AddCorsConfiguration(IServiceCollection services) =>
           services.AddCors(options => {
               options.AddPolicy("AllowAll", builder =>
               builder.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials());
           });

        private void AddSettings(IServiceCollection services)
        {
            services.Configure<EnvironmentConfig>(Configuration);
            services.Configure<ModelsDatabaseSettings>(
                        Configuration.GetSection(nameof(ModelsDatabaseSettings)));
        }

        private void AddDbConnection(IServiceCollection services)
        {
            var connection = Configuration["DB_CONNECTION"];

            services.AddDbContext<GltfContext>(options => options.UseSqlite(connection,
                   b => b.MigrationsAssembly("Gltf_file_sharing.API")));

            services.AddSingleton<MongoContext>();

        }

        #endregion
    }
}
