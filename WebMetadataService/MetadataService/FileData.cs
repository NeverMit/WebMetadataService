namespace WebMetadataService.MetadataService;

public class FileData
{
    //имя файла
    public string FileName { get; set; } 
    //хеш-код данных файла
    public int FileHash{ get; set; }
    public string FileContent { get; set; }
    public override bool Equals(object? obj)
    {
        if (obj is FileData other)
        {
            return FileName == other.FileName && 
                   FileHash == other.FileHash && 
                   FileContent == other.FileContent;
        }
        return false;
    }
    
    public override int GetHashCode()
    {
        return FileHash.GetHashCode();
    }
}