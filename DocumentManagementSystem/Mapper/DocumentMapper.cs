using AutoMapper;
using DocumentManagementSystem.DTOs;
using DocumentManagementSystem.Model;

namespace DocumentManagementSystem.Mapper
{
    public class DocumentMapper : Profile
    {
        public DocumentMapper()
        {
            CreateMap<Document, DocumentDto>();
            CreateMap<DocumentDto, Document>();
        }
    }
}
