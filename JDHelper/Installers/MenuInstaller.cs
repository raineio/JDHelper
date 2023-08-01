using JDHelper.AffinityPatches;
using System;
using System.Collections.Generic;
using System.Text;
using Zenject;

namespace JDHelper.Installers
{
    internal class MenuInstaller : Installer
    {

        public MenuInstaller()
        {   }

        public override void InstallBindings()
        {
            // Will make sure this works when I get UI to work
            // Container.BindInterfacesAndSelfTo<HalfJumpDistancePatch>().AsSingle();
            Container.BindInterfacesAndSelfTo<JumpDistanceViewController>().AsSingle();
        }
    }
}
