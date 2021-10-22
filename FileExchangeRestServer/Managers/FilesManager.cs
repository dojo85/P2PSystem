using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FileExchangeRestServer.Data;
using FileExchangeRestServer.Models;
using FileExchangeSharedClasses;
using Microsoft.EntityFrameworkCore;

namespace FileExchangeRestServer.Managers
{
    public class FilesManager : IFilesManager
    {
        private FilesDbContext _context;

        public FilesManager(FilesDbContext context)
        {
            _context = context;
        }

        public IEnumerable<string> GetAllFileNames()
        {
            return _context.Files.Select(f=>f.Name);
        }

        public string GetPeersByFileName(string fileName)
        {
            var file = _context.Files.Find(fileName);
            if (file is null) return null;
            string endPointsString = _context.Files.Find(fileName).EndPoints;
            return endPointsString;
        }

        public async Task<StoredFile> AddFileRecordAsync(string fileName, FileEndPoint endPoint)
        {
            var file = await _context.Files.FirstOrDefaultAsync(f => f.Name == fileName);
            if (file is null)
            {
                var epList = new List<FileEndPoint>();
                epList.Add(endPoint);
                var epString = JsonSerializer.Serialize(epList);
                StoredFile newFile = new StoredFile(fileName, epString);
                file = _context.Files.Add(newFile).Entity;
                await _context.SaveChangesAsync();
            }
            else
            {
                List<FileEndPoint> epList1 = JsonSerializer.Deserialize<List<FileEndPoint>>(file.EndPoints);
                var epList = new List<FileEndPoint>(epList1);
                epList.Add(endPoint);
                string newList = JsonSerializer.Serialize(epList);
                file.EndPoints = newList;
                await _context.SaveChangesAsync();
            }
            return file;
        }

        public async Task<StoredFile> DeleteEndpointFromRecord(string fileName, FileEndPoint endPoint)
        {
            var file = await _context.Files.FindAsync(fileName);
            if (file is null) return null;
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var epList = JsonSerializer.Deserialize<List<FileEndPoint>>(file.EndPoints, options);
            var item = epList.FirstOrDefault(e => e.IPAddress == endPoint.IPAddress && e.Port == endPoint.Port);
            bool removed = epList.Remove(item);
            if (epList.Count == 0)
            {
                _context.Files.Remove(file);
                await _context.SaveChangesAsync();
                return null;

            }
            file.EndPoints = JsonSerializer.Serialize(epList);  
            
            await _context.SaveChangesAsync();
            return file;
        }
    }
}
