using Microsoft.Extensions.Options;

namespace WebMetadataService.MetadataService;
//класс для работы с файлом
public class FileHandlerService:BackgroundService
{
    private readonly FileMetadataProvider _fileMetadataProvider;
    private readonly FileScannerOptions _options;
    public FileHandlerService(FileMetadataProvider fileMetadataProvider,IOptions<FileScannerOptions> options)
    {
        _fileMetadataProvider = fileMetadataProvider;
        _options = options.Value;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (_options.Path != null && _options.SearchPattern != null)
                {
                    await GetSearchedFilesAsync(stoppingToken);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            await Task.Delay(TimeSpan.FromSeconds(_options.Interval), stoppingToken);
        }
    }
    private async Task GetSearchedFilesAsync(CancellationToken stoppingToken)
    {
        var files = Directory.GetFiles
        (
            _options.Path,
            _options.SearchPattern
        );
        Console.WriteLine($"Найдено {files.Length} файлов");
        foreach (var file in files)
        {
            var fileData = await _fileMetadataProvider.ReadFileDataAsync(file);
            _fileMetadataProvider.FindFileChanges(fileData);
            if (_fileMetadataProvider.Files == null)
            {
                Console.WriteLine("Files пустой");
            }
            else
            {
                Console.WriteLine
                    (
                        $"Количество обрабатываемых файлов: {_fileMetadataProvider.Files.Count}"
                    );
            }
            Console.WriteLine
            (
                $"File:{fileData.FileName}," +
                $"Hash:{fileData.FileHash}"
            );
        }
    }
}