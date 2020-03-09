using Docs.Business.Contracts;
using Docs.Entities;
using Docs.Repositories.Persistences;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Docs.Business.Business_Rules
{
    public class DocumentoBusiness : IDocumentoBusiness
    {
        private DocumentoPersistence persistence = new DocumentoPersistence();

        public int CadastrarDocumento(Documento doc)
        {
            return persistence.CadastrarDocumento(doc);
        }

        public void EditarDocumento(Documento doc)
        {
            persistence.EditarDocumento(doc);
        }

        public void ExcluirDocumento(int id)
        {
            persistence.ExcluirDocumento(id);

            string directory = HttpContext.Current.Server.MapPath($"~/Documentos/{id}");

            DirectoryInfo source = new DirectoryInfo(directory);

            foreach (FileInfo fi in source.GetFiles())
            {
                fi.Delete();
            }
        }

        public IList<Documento> ListarTodos()
        {
            return persistence.ListarTodos();
        }

        public Documento ListarPorId(int id)
        {
            return persistence.ListarPorId(id);
        }
    }
}
