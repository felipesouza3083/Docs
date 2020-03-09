using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Docs.App.Validations
{
    public class ErrorValidation
    {
        //método para receber o ModelState 
        //e retornar apenas os erros de validação..
        public static Hashtable GetValidationErrors(ModelStateDictionary model)
        {
            Hashtable mapa = new Hashtable();

            foreach (var m in model) //varrendo o ModelState..
            {
                if (m.Value.Errors.Count() > 0) //verificando se há erro..
                {
                    //armazenar no mapa o nome do campo que esta com erro (Key)
                    //armazenar relacionado ao nome do campo as mensagens de erro
                    if (!string.IsNullOrEmpty(m.Value.Errors.Select(s => s.ErrorMessage).First()))
                    {
                        mapa[m.Key] = m.Value.Errors.Select(s => s.ErrorMessage).ToList();
                    }
                    else
                    {
                        mapa[m.Key] = m.Value.Errors.Select(s => s.Exception.Message).First();
                    }
                }
            }

            return mapa;
        }
    }
}