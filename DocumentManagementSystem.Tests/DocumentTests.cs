using AutoMapper;
using DocumentManagementSystem.Controllers;
using DocumentManagementSystem.DTOs;
using DocumentManagementSystem.Interfaces;
using DocumentManagementSystem.Mapper;
using DocumentManagementSystem.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using Xunit;

namespace DocumentManagementSystem.Tests
{
    public class DocumentTests
    {
        [Fact]
        public void GetDocuments_ShouldReturnAllDocuments()
        {
            // Arrange
            var docRepo = Substitute.For<IRepository>();
            var docs = new List<Document>();
            docs.Add(new Document
            {
                DocId = "2b4e176c-3608-4a7f-8aef-1eb25d6f531c",
                FileSize = 556796,
                Name = "Test.pdf",
                Location = "https://sjchambers.blob.core.windows.net/sjchambers/2b4e176c-3608-4a7f-8aef-1eb25d6f531c",
                DisplayOrder = 1

            });
            docs.Add(new Document
            {
                DocId = "2c4e176c-3608-4a7f-8aef-1eb25d6f531c",
                FileSize = 226796,
                Name = "Test1.pdf",
                Location = "https://sjchambers.blob.core.windows.net/sjchambers/2c4e176c-3608-4a7f-8aef-1eb25d6f531c",
                DisplayOrder = 2

            });
            docRepo.GetDocs().Returns(docs);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DocumentMapper());
            });
            var mapper = config.CreateMapper();

            var docController = new DocumentController(docRepo, Substitute.For<IStorageService>(), mapper);

            var result = docController.GetDocuments().Result as OkObjectResult;

            var resultDoc = result.Value as IList<DocumentDto>;

            Assert.True(resultDoc.Count == 2);
            Assert.IsType<List<DocumentDto>>(resultDoc);
            Assert.Equal(docs[0].Name, resultDoc[0].Name);
            Assert.Equal(docs[0].Location, resultDoc[0].Location);
            Assert.Equal(docs[0].FileSize, resultDoc[0].FileSize);
        }

        [Fact]
        public void GetDocuments_NoDocuments_ShouldReturnNotFound()
        {
            // Arrange
            var docRepo = Substitute.For<IRepository>();
            var docs = new List<Document>();

            docRepo.GetDocs().Returns(docs);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DocumentMapper());
            });
            var mapper = config.CreateMapper();

            var docController = new DocumentController(docRepo, Substitute.For<IStorageService>(), mapper);

            //Action
            var actionResult = docController.GetDocuments();

            //Assert
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public void Download_BlankLocation_ShouldReturnBadRequest()
        {
            //Arrange
            string location = string.Empty;
            var docController = new DocumentController(Substitute.For<IRepository>(), Substitute.For<IStorageService>(), Substitute.For<IMapper>());

            var actionResult = docController.Download(location);

            Assert.IsType<BadRequestResult>(actionResult);
        }

        [Fact]
        public void Download_WrongLocation_ShouldReturnNotFound()
        {
            //Arrange
            string location = "Wrong location";
            var docController = new DocumentController(Substitute.For<IRepository>(), Substitute.For<IStorageService>(), Substitute.For<IMapper>());

            var actionResult = docController.Download(location);

            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public void Download_RecordFoundForLocation_FileNotPresent_ShouldReturnNotFound()
        {
            var docRepo = Substitute.For<IRepository>();
            var docs = new List<Document>();
            docs.Add(new Document
            {
                DocId = "2b4e176c-3608-4a7f-8aef-1eb25d6f531c",
                FileSize = 556796,
                Name = "Test.pdf",
                Location = "https://sjchambers.blob.core.windows.net/sjchambers/2b4e176c-3608-4a7f-8aef-1eb25d6f531c",
                DisplayOrder = 1

            });
            docRepo.GetDocs().Returns(docs);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DocumentMapper());
            });
            var mapper = config.CreateMapper();

            var service = Substitute.For<IStorageService>();
            service.DownloadFile(Arg.Any<string>()).Returns((byte[])null);
            var docController = new DocumentController(docRepo, service, mapper);

            var result = docController.Download(HttpUtility.UrlEncode(docs[0].Location));

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Download_RecordFoundForLocation_FilePresent_ShouldReturnPdfFile()
        {
            var docRepo = Substitute.For<IRepository>();
            var docs = new List<Document>();
            docs.Add(new Document
            {
                DocId = "2b4e176c-3608-4a7f-8aef-1eb25d6f531c",
                FileSize = 556796,
                Name = "Test.pdf",
                Location = "https://sjchambers.blob.core.windows.net/sjchambers/2b4e176c-3608-4a7f-8aef-1eb25d6f531c",
                DisplayOrder = 1

            });
            docRepo.GetDocs().Returns(docs);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DocumentMapper());
            });
            var mapper = config.CreateMapper();

            var docController = new DocumentController(docRepo, Substitute.For<IStorageService>(), mapper);

            var result = docController.Download(HttpUtility.UrlEncode(docs[0].Location));

            Assert.IsType<FileContentResult>(result);

            Assert.Equal("application/pdf", ((FileContentResult)result).ContentType);
        }

        [Fact]
        public void Upload_NoFileUploaded_ShouldReturnBadRequest()
        {
            var pdf = new PdfDocument
            {
                Doc = null
            };

            var docController = new DocumentController(Substitute.For<IRepository>(), Substitute.For<IStorageService>(), Substitute.For<IMapper>());

            var result = docController.Upload(pdf).Result;
            Assert.IsType<BadRequestResult>(result);
        }
        [Fact]
        public void Upload_FailedToUpload_ShouldReturnInternalServerError()
        {
            var pdf = new PdfDocument
            {
                Doc = Substitute.For<IFormFile>()
            };

            using var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write("Test File");
            writer.Flush();
            ms.Position = 0;

            pdf.Doc.OpenReadStream().Returns(ms);
            pdf.Doc.FileName.Returns("Test.pdf");
            pdf.Doc.Length.Returns(ms.Length);

            var service = Substitute.For<IStorageService>();
            service.UploadFile(Arg.Any<Stream>()).Returns(Guid.Empty.ToString());
            var docController = new DocumentController(Substitute.For<IRepository>(), service, Substitute.For<IMapper>());

            var result = docController.Upload(pdf);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result).StatusCode);
        }
        [Fact]
        public void Upload_FileUploaded_ShouldReturnCreatedResult()
        {
            var pdf = new PdfDocument
            {
                Doc = Substitute.For<IFormFile>()
            };

            using var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write("Test File");
            writer.Flush();
            ms.Position = 0;

            pdf.Doc.OpenReadStream().Returns(ms);
            pdf.Doc.FileName.Returns("Test.pdf");
            pdf.Doc.Length.Returns(ms.Length);

            var service = Substitute.For<IStorageService>();
            service.UploadFile(Arg.Any<Stream>()).Returns(Guid.NewGuid().ToString());
            var docController = new DocumentController(Substitute.For<IRepository>(), service, Substitute.For<IMapper>());

            var result = docController.Upload(pdf);
            Assert.IsType<CreatedResult>(result.Result);
        }

        [Fact]
        public void Delete_InvalidId_ShouldReturnBadRequest()
        {
            var id = string.Empty;
            var docController = new DocumentController(Substitute.For<IRepository>(), Substitute.For<IStorageService>(), Substitute.For<IMapper>());

            var result = docController.DeleteDoc(id);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Delete_IdNotPresent_ShouldReturnNotFound()
        {
            var id = Guid.NewGuid().ToString();
            var docRepo = Substitute.For<IRepository>();

            docRepo.GetDoc(Arg.Any<string>()).Returns((Document)null);
            var docController = new DocumentController(docRepo, Substitute.For<IStorageService>(), Substitute.For<IMapper>());

            var result = docController.DeleteDoc(id);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_IdPresentAndFilePresent_ShouldReturnNoContent()
        {
            var id = Guid.NewGuid().ToString();
            var docRepo = Substitute.For<IRepository>();

            docRepo.GetDoc(Arg.Any<string>()).Returns(new Document { DocId = id });
            var docController = new DocumentController(docRepo, Substitute.For<IStorageService>(), Substitute.For<IMapper>());

            var result = docController.DeleteDoc(id);
            Assert.IsType<NoContentResult>(result);
        }

    }
}
