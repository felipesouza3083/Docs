using Docs.Entities.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docs.Entities
{
    public class Documento
    {
        public int IdDocumento { get; set; }
        public string CodigoDocumento { get; set; }
        public string TituloDocumento { get; set; }
        public Revisao Revisao { get; set; }
        public DateTime DataPlanejada { get; set; }
        public decimal Valor { get; set; }
        public string ArquivoDocumento { get; set; }

        public Documento()
        {

        }

        public Documento(int idDocumento, string codigoDocumento, string tituloDocumento, Revisao revisao, DateTime dataPlanejada, decimal valor, string arquivoDocumento)
        {
            IdDocumento = idDocumento;
            CodigoDocumento = codigoDocumento;
            TituloDocumento = tituloDocumento;
            Revisao = revisao;
            DataPlanejada = dataPlanejada;
            Valor = valor;
            ArquivoDocumento = arquivoDocumento;
        }

        public override string ToString()
        {
            return $"Id do documento: {IdDocumento}, " +
                   $"Código do documento:{CodigoDocumento}, " +
                   $"Nome do documento: {TituloDocumento}, " +
                   $"Revisão do documento:{Revisao}, " +
                   $"Data planejada do documento: {DataPlanejada.ToString("dd/MM/yyyy")}, " +
                   $"Valor do documento: {Valor}, " +
                   $"Arquivo do documento: {ArquivoDocumento}";
        }
    }
}
