using Microsoft.Extensions.Options;

namespace WebMetadataService.MetadataService;

public class FileScannerBuilder
{
    private static IServiceCollection? _services;

    public static FileScanner? GetFileScanner()
    {
        if (_services != null)
        {
            var serviceProvider = _services.BuildServiceProvider();
            var fileScanner = serviceProvider.GetService<FileScanner>();
            return fileScanner;
        }
        return null;
    }
    public static async Task Builder()
    {
        _services = new ServiceCollection();
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
                        @"appsettings.json",
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
                        hostContext.Configuration.GetSection("FileScanner")
                    );
                    services.AddHostedService<FileScanner>();
                    _services = services;
                }
            );
        //создаем экземпляр хоста приложения
        await builder.Build().RunAsync();
    }
}