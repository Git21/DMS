using System;
using Xunit;

namespace DocumentManagementSystem.Tests
{
    public class DocumentTests
    {
        [Fact]
        public void GetDocuments_ShouldReturnAllDocuments()
        {

        }

        [Fact]
        public void GetDocuments_NoDocuments_ShouldReturnNotFound()
        {

        }

        [Fact]
        public void Download_BlankLocation_ShouldReturnBadRequest()
        {

        }

        [Fact]
        public void Download_WrongLocation_ShouldReturnNotFound()
        {

        }
        [Fact]
        public void Download_RecordFoundForLocation_FileNotPresent_ShouldReturnNotFound()
        {

        }
        [Fact]
        public void Download_RecordFoundForLocation_FilePresent_ShouldReturnPdfFile()
        {

        }

        [Fact]
        public void Upload_NoFileUploaded_ShouldReturnBadRequest()
        {

        }

        [Fact]
        public void Upload_FailedToUpload_ShouldReturnInternalServerError()
        {

        }

        [Fact]
        public void Upload_NonPdfFile_ShouldReturnBadRequest()
        {

        }

        [Fact]
        public void Upload_PdfFileMoreThanMaxSize_ShouldReturnBadRequest()
        {

        }

        [Fact]
        public void Upload_FileUploaded_ShouldReturnCreatedResult()
        {

        }
        [Fact]
        public void Delete_InvalidId_ShouldReturnBadRequest()
        {

        }
        [Fact]
        public void Delete_IdNotPresent_ShouldReturnNotFound()
        {

        }

        [Fact]
        public void Delete_IdPresentAndFilePresent_ShouldReturnNoContent()
        {

        }
    }
}
