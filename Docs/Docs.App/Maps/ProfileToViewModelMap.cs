using AutoMapper;
using Docs.App.Models.Documento;
using Docs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Docs.App.Maps
{
    public class ProfileToViewModelMap : Profile
    {
        public ProfileToViewModelMap()
        {
            CreateMap<Documento, DocumentoConsultaViewModel>();
            CreateMap<Documento, DocumentoEdicaoViewModel>();
        }
    }
}