using ComputerInterface.Interfaces;
using Zenject;

namespace DevMinecraftMod.ComputerInterface
{
    internal class MainInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<IComputerModEntry>().To<MinecraftEnter>().AsSingle();
        }
    }
}
