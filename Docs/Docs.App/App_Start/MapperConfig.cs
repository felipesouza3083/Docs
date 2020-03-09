using AutoMapper;
using Docs.App.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Docs.App.App_Start
{
    public class MapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<ProfileToViewModelMap>();
                x.AddProfile<ViewModelToProfileMap>();
            });
            Mapper.Configuration.AssertConfigurationIsValid();
        }
    }
}