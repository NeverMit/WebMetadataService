using System.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace WebMetadataService.MetadataService;
// класс для десериализации, валидации, хранения данных файлов и сравнения хэшей. 
public class FileMetadataProvider
{
    public List<FileData> Files { get; set; } = new();
    
    public void FindFileChanges(FileData fileData)
    {
        
        var f = Files.Find
        (
            x =>
                x.FileName.Equals
                (
                    fileData.FileName,
                    StringComparison.CurrentCultureIgnoreCase
                )
        );           
        if (f == null)
        {
            Files.Add(fileData);
        }
        else
        {
            if (f.FileHash != fileData.GetHashCode())
            {
                Console.WriteLine(f.FileName + " изменен");
                f.FileHash = fileData.GetHashCode();
            }
        }
    }
    //чтение содержимого файла
    public  async Task<FileData> ReadFileDataAsync(string filePath)
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