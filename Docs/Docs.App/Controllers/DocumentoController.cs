using AutoMapper;
using Docs.App.Models.Documento;
using Docs.App.Validations;
using Docs.Business.Contracts;
using Docs.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Docs.App.Controllers
{
    public class DocumentoController : Controller
    {
        private IDocumentoBusiness business;

        public DocumentoController(IDocumentoBusiness business)
        {
            this.business = business;
        }

        // GET: Documento
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CadastrarDocumento(DocumentoCadastroViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var documento = Mapper.Map<Documento>(model);

                    var retorno = new DocumentoCadastroRetornoViewModel();

                    retorno.IdDocumento =  business.CadastrarDocumento(documento);
                    retorno.Mensagem = "Documento cadastrado com sucesso.";

                    return Json(retorno);
                }
                catch (Exception e)
                {
                    return Json($"Ocorreu um erro {e.Message}");
                }
            }
            else
            {
                return Json(ErrorValidation.GetValidationErrors(ModelState));
            }
        }

        [HttpPost]
        public JsonResult EditarDocumento(DocumentoEdicaoViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var documento = Mapper.Map<Documento>(model);

                    business.EditarDocumento(documento);

                    var retorno = new DocumentoCadastroRetornoViewModel();

                    retorno.IdDocumento = model.IdDocumento;
                    retorno.Mensagem = "Documento alterado com sucesso.";

                    return Json(retorno);
                }
                catch (Exception e)
                {
                    return Json($"Ocorreu um erro {e.Message}");
                }
            }
            else
            {
                return Json(ErrorValidation.GetValidationErrors(ModelState));
            }
        }

        [HttpPost]
        public JsonResult UploadDocumento(int id)
        {
            HttpPostedFileBase arquivo = null;
            if (Request.Files.Count > 0)
            {
                HttpFileCollectionBase files = Request.Files;
                arquivo = Request.Files[0];
            }
            string caminho = "";
            if (arquivo != null)
            {
                try
                {
                    caminho = Server.MapPath($"/Documentos/{id}/");
                    //Se o caminho não existe, Cria
                    if (!Directory.Exists(caminho))
                    {
                        Directory.CreateDirectory(caminho);
                    }

                    caminho = caminho + Path.GetFileName(arquivo.FileName);
                    arquivo.SaveAs(caminho);
                    return Json("Arquivo salvo com sucesso!");
                }
                catch (Exception e)
                {
                    return Json($"Erro ao importar arquivo {e.Message}");
                }
            }
            else
            {
                return Json("Nenhum arquivo selecionado!");
            }

        }

        [HttpGet]
        public JsonResult ExcluirDocumento(int id)
        {
            try
            {
                business.ExcluirDocumento(id);

                return Json("Documento excluído com sucesso.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json($"Ocorreu um erro:{e.Message}", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ListarDocumentos()
        {
            try
            {
                List<DocumentoConsultaViewModel> lista = new List<DocumentoConsultaViewModel>();

                foreach (var doc in business.ListarTodos())
                {
                    var model = Mapper.Map<DocumentoConsultaViewModel>(doc);

                    lista.Add(model);
                }

                return Json(lista, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json($"Ocorreu um erro {e.Message}", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ListarDocumentosPorId(int id)
        {
            try
            {
                var model = Mapper.Map<DocumentoEdicaoViewModel>(business.ListarPorId(id));

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json($"Ocorreu um erro {e.Message}", JsonRequestBehavior.AllowGet);
            }
        }
    }
}