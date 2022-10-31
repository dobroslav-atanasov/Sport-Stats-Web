namespace SportStats.Services.Interfaces;

using SportStats.Data.Models.Http;

public interface IHttpService
{
    Task<HttpModel> GetAsync(string url, bool isOlympediaUrl = false);

    Task<HttpModel> PostAsync(string url, string json);

    Task<byte[]> DownloadBytesAsync(string url);
}