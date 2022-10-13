namespace SportStats.Services.Interfaces;

using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Data.Models.Zip;

public interface IZipService
{
    byte[] ZipGroup(Group group);

    List<ZipModel> UnzipGroup(byte[] data);
}