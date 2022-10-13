namespace SportStats.Services;

using System.Collections.Generic;

using SportStats.Data.Models.Entities.Crawlers;
using SportStats.Data.Models.Zip;
using SportStats.Services.Interfaces;

public class ZipService : IZipService
{
    public List<ZipModel> UnzipGroup(byte[] data)
    {
        throw new NotImplementedException();
    }

    public byte[] ZipGroup(Group group)
    {
        throw new NotImplementedException();
    }
}