using System;
using ComputerInterface.Interfaces;

namespace DevMinecraftMod.CI
{
    public class MinecraftEnter : IComputerModEntry
    {
        public string EntryName => "DevMinecraftMod";

        public Type EntryViewType => typeof(MinecraftView);
    }
}
