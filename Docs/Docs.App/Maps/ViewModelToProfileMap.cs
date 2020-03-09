using AutoMapper;
using Docs.App.Helper;
using Docs.App.Models.Documento;
using Docs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Docs.App.Maps
{
    public class ViewModelToProfileMap : Profile
    {
        public ViewModelToProfileMap()
        {
            CreateMap<DocumentoCadastroViewModel, Documento>()
                .ForMember(doc => doc.IdDocumento, vm => vm.Ignore())
                .ForMember(doc => doc.Revisao,vm=> vm.Ignore());

            CreateMap<DocumentoEdicaoViewModel, Documento>();
        }
    }
}