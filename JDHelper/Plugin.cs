using System;
using System.Reflection;
using HarmonyLib;
using SiraUtil;
using IPA;
using IPALogger = IPA.Logging.Logger;
using SiraUtil.Zenject;

namespace JDHelper
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        public static IPALogger Logger;
        
        [Init]
        public void Init(Zenjector zenjector ,IPALogger logger)
        {
            Logger = logger;
        }
    }
}