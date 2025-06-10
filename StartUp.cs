using TrackMyAssets_API.Domain.ModelsViews;

namespace TrackMyAssets_API
{
    public class StartUp
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", () => Results.Json(new HomeModelView())).WithTags("Home").AllowAnonymous();
            });

        }
    }
}