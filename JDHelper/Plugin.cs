using System;
using System.Reflection;
using HarmonyLib;
using SiraUtil;
using IPA;
using IPALogger = IPA.Logging.Logger;
using SiraUtil.Zenject;
using JDHelper.Installers;
using IPA.Config.Stores;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace JDHelper
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        public static IPALogger Logger; 
        private Config _config;

        [Init]
        public void Init(Zenjector zenjector, IPALogger logger, Config config)
        {
            Logger = logger;
            _config = config;

            zenjector.UseLogger();
            zenjector.Install(Location.App, _ =>
            {
                _.BindInstance(config);
            });

            zenjector.Install<MenuInstaller>(Location.Menu);
        }
    }
}