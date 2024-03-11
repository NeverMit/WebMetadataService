using WebMetadataService.MetadataService;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var fileScannerBuilder = FileScannerBuilder.Builder();
        builder.Services.AddSingleton<FileScanner>(); // Регистрируем FileScanner в контейнере зависимостей
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseRouting();
        app.UseEndpoints(endpoint =>
        {
            endpoint.MapGet("/metadata", async (context) =>
            {
                var serviceProvider = builder.Services.BuildServiceProvider();
                var fileScanner = serviceProvider.GetService<FileScanner>();
                if (fileScanner != null)
                {
                    var files = fileScanner.GetFiles();
                    foreach (var file in files)
                    {
                        await context.Response.WriteAsync(file.FileContent);
                    }
                }
            });
        });
        app.Run();
    }
}