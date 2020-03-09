using Docs.Entities.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Docs.App.Models.Documento
{
    public class DocumentoConsultaViewModel
    {
        public int IdDocumento { get; set; }
        public string CodigoDocumento { get; set; }
        public string TituloDocumento { get; set; }
        public Revisao Revisao { get; set; }
        public DateTime DataPlanejada { get; set; }
        public decimal Valor { get; set; }
        public string ArquivoDocumento { get; set; }
    }
}