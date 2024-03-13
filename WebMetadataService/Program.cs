using WebMetadataService.MetadataService;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var fileScannerBuilder = FileScannerBuilder.Builder();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<FileMetadataProvider>();
        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseRouting();
        var fileMetadataProvider = builder
            .Services
            .BuildServiceProvider()
            .GetService<FileMetadataProvider>();
        app.UseEndpoints(endpoint =>
        {
            endpoint.MapGet("/metadata", async (context) =>
            {
                foreach (var file in fileMetadataProvider.Files)
                {
                    await context.Response.WriteAsync(file.FileContent);
                }
            });
        });
        app.Run();
    }
}