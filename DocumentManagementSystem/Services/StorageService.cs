using Azure.Storage;
using Azure.Storage.Blobs;
using DocumentManagementSystem.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagementSystem.Services
{
    public class StorageService : IStorageService
    {
        private string _containerName;
        private string _key;
        private string _storageName;
        private string _baseUri;
        private readonly BlobServiceClient _client;
        private readonly BlobContainerClient _container;
        public StorageService(IConfiguration config)
        {
            Initialise(config);
            var credentials = new StorageSharedKeyCredential(_storageName, _key);
            _client = new BlobServiceClient(new Uri(_baseUri), credentials);
            _container = _client.GetBlobContainerClient(_containerName);
        }
        public string UploadFile(Stream fileStream)
        {
            var fileId = Guid.NewGuid().ToString();
            
            var blob = _container.GetBlobClient(fileId);
            if (!blob.Exists())
            {
                blob.Upload(fileStream);
                return fileId;
            }
            return Guid.Empty.ToString();
        }
        public byte[] DownloadFile(string fileId)
        {
            byte[] file = null;
            var blob = _container.GetBlobClient(fileId);
            if (blob.Exists())
            {
                using (var destStream = new MemoryStream())
                {
                    blob.DownloadTo(destStream);
                    file = destStream.ToArray();
                }
            }
            return file;
        }
        public string GetFilePath(string fileId)
        {
            return $"{_baseUri}{_containerName}/{fileId}";
        }
        public bool DeleteFile(string fileId)
        {
            bool isDeleted = false;
            var blob = _container.GetBlobClient(fileId);
            if (blob.Exists())
                isDeleted = blob.DeleteIfExists().Value;

            return isDeleted;
        } 
        protected void Initialise(IConfiguration config)
        {
            _storageName = config.GetSection("BlobStorage:Name").Value;
            _containerName = config.GetSection("BlobStorage:Container").Value;
            _key = config.GetSection("BlobStorage:Key").Value;
            _baseUri = config.GetSection("BlobStorage:BaseUri").Value;
        }
    }
}
