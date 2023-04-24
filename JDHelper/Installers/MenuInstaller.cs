using Colors.AffinityPatches;
using System;
using System.Collections.Generic;
using System.Text;
using Zenject;

namespace JDHelper.Installers
{
    internal class MenuInstaller : Installer
    {
        public MenuInstaller()
        {

        }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<HalfJumpDistancePatch>().AsSingle();
        }
    }
}
