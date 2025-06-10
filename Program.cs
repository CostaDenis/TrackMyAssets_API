using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using TrackMyAssets_API;


IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<StartUp>();

    });
}

CreateHostBuilder(args).Build().Run();

