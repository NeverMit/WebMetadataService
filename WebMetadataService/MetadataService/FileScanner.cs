using System.Collections;
using Microsoft.Extensions.Options;

namespace WebMetadataService.MetadataService;

public class FileScanner :BackgroundService
{
    //параметры из файла конфигурации
    private readonly FileScannerOptions _options;
    //список файлов
    private readonly List<FileData>? _files;
    
    public List<FileData>? GetFiles()
    {
        return _files;
    }
    // Конструктор
    public FileScanner(IOptions<FileScannerOptions> options)
    {
        _options = options.Value;
        _files = new List<FileData>();
    }
    //сканирование фалла
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (_options.Path != null && _options.SearchPattern != null)
                {
                    GetSearchedFiles();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            await Task.Delay(TimeSpan.FromSeconds(_options.Interval), stoppingToken);
        }
    }
    //получение найденных файлов
    private async void GetSearchedFiles()
    {
            var files = Directory.GetFiles(_options.Path, _options.SearchPattern);
            Console.WriteLine($"Найдено {files.Length} файлов");
            foreach (var file in files)
            {
                var fileData = await ReadFileDataAsync(file);
                FindFileChanges(fileData);
                Console.WriteLine
                (
                    $"File:{fileData.FileName},"+
                    $"Hash:{fileData.FileHash}"
                );
            }
    }
    //поиск изменений в файле
    private void FindFileChanges(FileData fileData)
    {
        var f = _files.Find
        (x => 
            x.FileName.Equals
            (
                fileData.FileName,
                StringComparison.CurrentCultureIgnoreCase
            )    
        );
        if (f == null)
        {
            _files.Add(fileData);
        }
        else
        {
            if (f.FileHash!=fileData.GetHashCode())
            {
                Console.WriteLine(f.FileName+" изменен");
                f.FileHash = fileData.GetHashCode();
            }
        }
    }
    //чтение содержимого файла
    private static async Task<FileData> ReadFileDataAsync(string filePath)
    {
            await using var fileStream = new FileStream
            (
                filePath,
                FileMode.Open,
                FileAccess.Read
            );
            using var streamReader = new StreamReader(fileStream);
            var fileContent = await streamReader.ReadToEndAsync();
            var fileName = Path.GetFileName(filePath);
            var fileData = new FileData
            {
                FileName = fileName,
                FileHash = fileContent.GetHashCode(),
                FileContent=fileContent
            };
            return fileData;
    }
}