
using System.Text;
using Newtonsoft.Json;
using WebMetadataService;
using WebMetadataService.MetadataService;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var fileScannerBuilder = FileScannerBuilder.Builder();
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
            endpoint.MapPost("/metadata", async (context) =>
            {
                var fileScanner = FileScannerBuilder.GetFileScanner();
                var files=fileScanner.GetFiles();
                if (files != null)
                {
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
