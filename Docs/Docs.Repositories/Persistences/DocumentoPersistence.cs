using Docs.Entities;
using Docs.Entities.Types;
using Docs.Repositories.Contracts;
using Docs.Repositories.DAL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docs.Repositories.Persistences
{
    public class DocumentoPersistence : Conexao, IDocumentoPersistence
    {
        public int CadastrarDocumento(Documento doc)
        {
            string query = "Insert into documento(codigoDocumento, tituloDocumento, revisaoDocumento, dataPlanejada, valor, arquivoDocumento) " +
                           "values(@codigoDocumento, @tituloDocumento, @revisaoDocumento, @dataPlanejada, @valor, @arquivoDocumento)";

            OpenConnection();

            cmd = new MySqlCommand(query, con);

            cmd.Parameters.AddWithValue("@codigoDocumento", doc.CodigoDocumento);
            cmd.Parameters.AddWithValue("@tituloDocumento", doc.TituloDocumento);
            cmd.Parameters.AddWithValue("@revisaoDocumento", doc.Revisao);
            cmd.Parameters.AddWithValue("@dataPlanejada", doc.DataPlanejada);
            cmd.Parameters.AddWithValue("@valor", doc.Valor);
            cmd.Parameters.AddWithValue("@arquivoDocumento", doc.ArquivoDocumento);

            cmd.ExecuteNonQuery();

            int id = Convert.ToInt32(cmd.LastInsertedId);

            CloseConnection();

            return id;
        }

        public void EditarDocumento(Documento doc)
        {
            string query = "update documento set codigoDocumento = @codigoDocumento, " +
                           "tituloDocumento = @tituloDocumento, " +
                           "revisaoDocumento = @revisaoDocumento, " +
                           "dataPlanejada = @dataPlanejada, " +
                           "valor = @valor, " +
                           "arquivoDocumento = @arquivoDocumento " +
                           "where idDocumento = @idDocumento";

            OpenConnection();

            cmd = new MySqlCommand(query, con);

            cmd.Parameters.AddWithValue("@idDocumento", doc.IdDocumento);
            cmd.Parameters.AddWithValue("@codigoDocumento", doc.CodigoDocumento);
            cmd.Parameters.AddWithValue("@tituloDocumento", doc.TituloDocumento);
            cmd.Parameters.AddWithValue("@revisaoDocumento", doc.Revisao);
            cmd.Parameters.AddWithValue("@dataPlanejada", doc.DataPlanejada);
            cmd.Parameters.AddWithValue("@valor", doc.Valor);
            cmd.Parameters.AddWithValue("@arquivoDocumento", doc.ArquivoDocumento);

            cmd.ExecuteNonQuery();

            CloseConnection();
        }

        public void ExcluirDocumento(int id)
        {
            string query = "delete from documento where idDocumento = @idDocumento";

            OpenConnection();

            cmd = new MySqlCommand(query, con);

            cmd.Parameters.AddWithValue("@idDocumento", id);

            cmd.ExecuteNonQuery();

            CloseConnection();
        }

        public IList<Documento> ListarTodos()
        {
            string query = "select * from documento";

            OpenConnection();

            cmd = new MySqlCommand(query, con);

            dr = cmd.ExecuteReader();

            List<Documento> documentos = new List<Documento>();

            while (dr.Read())
            {
                Documento doc = new Documento();

                doc.IdDocumento = Convert.ToInt32(dr["idDocumento"]);
                doc.CodigoDocumento = Convert.ToString(dr["codigoDocumento"]);
                doc.TituloDocumento = Convert.ToString(dr["tituloDocumento"]);
                doc.Revisao = (Revisao)Enum.Parse(typeof(Revisao), Convert.ToString(dr["revisaoDocumento"]));
                doc.DataPlanejada = Convert.ToDateTime(dr["dataPlanejada"]);
                doc.Valor = Convert.ToDecimal(dr["valor"]);
                doc.ArquivoDocumento = Convert.ToString(dr["arquivoDocumento"]);

                documentos.Add(doc);
            }

            CloseConnection();

            return documentos;
        }

        public Documento ListarPorId(int id)
        {
            string query = "select * from documento where idDocumento = @idDocumento";

            OpenConnection();

            cmd = new MySqlCommand(query, con);

            cmd.Parameters.AddWithValue("@idDocumento", id);

            dr = cmd.ExecuteReader();

            Documento doc = null;

            if (dr.Read())
            {
                doc = new Documento();

                doc.IdDocumento = Convert.ToInt32(dr["idDocumento"]);
                doc.CodigoDocumento = Convert.ToString(dr["codigoDocumento"]);
                doc.TituloDocumento = Convert.ToString(dr["tituloDocumento"]);
                doc.Revisao = (Revisao)Enum.Parse(typeof(Revisao), Convert.ToString(dr["revisaoDocumento"]));
                doc.DataPlanejada = Convert.ToDateTime(dr["dataPlanejada"]);
                doc.Valor = Convert.ToDecimal(dr["valor"]);
                doc.ArquivoDocumento = Convert.ToString(dr["arquivoDocumento"]);
            }

            CloseConnection();

            return doc;
        }
    }
}
