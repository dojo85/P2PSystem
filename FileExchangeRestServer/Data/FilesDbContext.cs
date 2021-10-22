using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FileExchangeRestServer.Models;
using FileExchangeSharedClasses;
using Microsoft.EntityFrameworkCore;

namespace FileExchangeRestServer.Data
{
    public class FilesDbContext : DbContext
    {
        public FilesDbContext(DbContextOptions<FilesDbContext> options) : base(options)
        {
            
        }

        public DbSet<StoredFile> Files { get; set; }
    }
}
