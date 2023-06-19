using ComputerInterface.Interfaces;
using System;

namespace DevMinecraftMod.ComputerInterface
{
    public class MinecraftEnter : IComputerModEntry
    {
        public string EntryName => "DevMinecraftMod";

        public Type EntryViewType => typeof(MinecraftView);
    }
}
