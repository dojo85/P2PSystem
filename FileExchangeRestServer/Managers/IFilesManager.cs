using System.Collections.Generic;
using System.Threading.Tasks;
using FileExchangeRestServer.Models;
using FileExchangeSharedClasses;

namespace FileExchangeRestServer.Managers
{
    public interface IFilesManager
    {
        IEnumerable<string> GetAllFileNames();
        string GetPeersByFileName(string fileName);

        Task<StoredFile> AddFileRecordAsync(string fileName, FileEndPoint endPoint);
        Task<StoredFile> DeleteEndpointFromRecord(string fileName, FileEndPoint endPoint);
    }
}