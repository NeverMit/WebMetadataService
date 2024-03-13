using Microsoft.Extensions.Options;

namespace WebMetadataService.MetadataService;

public class FileScannerBuilder
{
    public static async Task Builder()
    {
        //Настройка хоста приложения
            var builder = new HostBuilder()
            .ConfigureAppConfiguration // настройка конфигурации приложения
            (
                (hostContext, config) =>
                {
                    config.SetBasePath
                    (
                        Directory.GetCurrentDirectory()
                    );
                    config.AddJsonFile
                    (
                        @"C:\Games\WebMetadataService\WebMetadataService\appsettings.json",
                        optional: true,
                        reloadOnChange: true
                    );
                }
            )
            .ConfigureServices // подключаем зависимость между FileScanner и FileScannerOptions
            (
                (hostContext, services) =>
                {
                    services.Configure<FileScannerOptions>
                    (
                        hostContext.Configuration.GetSection("FileScannerOptions")
                    );
                    services.AddSingleton<FileMetadataProvider>();
                    services.AddHostedService<FileHandlerService>();
                }
            );
        //создаем экземпляр хоста приложения
        await builder.Build().RunAsync();
    }
    
}