using Docs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docs.Repositories.Contracts
{
    public interface IDocumentoPersistence
    {
        int CadastrarDocumento(Documento doc);
        void EditarDocumento(Documento doc);
        void ExcluirDocumento(int id);
        IList<Documento> ListarTodos();
        Documento ListarPorId(int id);
    }
}
