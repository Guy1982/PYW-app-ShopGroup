﻿using System.Reflection;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Platform.Client;
using Platform.Client.Common.Context;
using Platform.Client.Configuration;
using SywApplicationShopGroup.Domain.Configuration;
using SywApplicationShopGroup.Domain.Repositorys;
using SywApplicationShopGroup.Domain.Users;
using SywApplicationShopGroup.Web.UI.Controllers;

namespace SywApplicationShopGroup.Web.UI.Installers
{
    public class ControllersInstaller : IWindsorInstaller
    {

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient());
            container.Register(AllTypes.FromAssembly(typeof(ShopGroupRepository).Assembly).Pick().LifestyleTransient().WithService.DefaultInterfaces());
            container.Register(AllTypes.FromAssembly(typeof(PlatformProxy).Assembly).Pick().LifestyleTransient().WithService.DefaultInterfaces());
        }
    }
}

