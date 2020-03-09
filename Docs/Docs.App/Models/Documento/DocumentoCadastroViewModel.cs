using Docs.Entities.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Docs.App.Models.Documento
{
    public class DocumentoCadastroViewModel
    {
        [Required(ErrorMessage = "Informe o código.")]
        public string CodigoDocumento { get; set; }

        [Required(ErrorMessage = "Informe o título.")]
        public string TituloDocumento { get; set; }

        [Required(ErrorMessage = "Informe a revisão.")]
        public Revisao Revisao { get; set; }

        [Required(ErrorMessage = "Informe a data planejada.")]
        public DateTime DataPlanejada { get; set; }

        [Required(ErrorMessage = "Informe o valor.")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Informe o arquivo.")]
        public string ArquivoDocumento { get; set; }
    }
}