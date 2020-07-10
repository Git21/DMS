using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagementSystem.Interfaces
{
    public interface IStorageService
    {
        string UploadFile(Stream fileStream);
        byte[] DownloadFile(string fileId);

        bool DeleteFile(string fileId);
        string GetFilePath(string fileId);
    }
}
