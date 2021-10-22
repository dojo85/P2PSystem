using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using FileExchangeSharedClasses;

namespace FileExchangeRestServer.Models
{
    public class StoredFile
    {
        [Key]
        public string Name { get; set; }
        public string EndPoints { get; set; }

        public StoredFile()
        {
        }

        public StoredFile(string fileName, string endPoints)
        {
            Name = fileName;
            EndPoints = endPoints;
        }
    }
}
