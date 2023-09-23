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
using JDHelper.AffinityPatches;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace JDHelper
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        public static IPALogger Logger;

        [Init]
        public void Init(Zenjector zenjector, IPALogger logger, IPA.Config.Config config)
        {
            Logger = logger;
            Config JdHelperConfig = config.Generated<Config>();

            zenjector.UseLogger();
            zenjector.Install(Location.App, _ =>
            {
                _.BindInstance(JdHelperConfig);
            });

            zenjector.Install<MenuInstaller>(Location.Menu);

            zenjector.Install(Location.GameCore, container =>
            {
                Logger.Info("knob 1");
                if (!JdHelperConfig.Enabled) return;
                Logger.Info("knob 2");
                container.BindInterfacesAndSelfTo<HalfJumpDistancePatch>().AsSingle();
            });
        }
    }
}