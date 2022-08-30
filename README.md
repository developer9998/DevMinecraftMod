# DevMinecraftMod
Dev's Minecraft Mod (DevMinecraftMod) is a mod for Gorilla Tag that brings aspects from Minecraft into the game.  
The mod uses Minecraft's 1.8.9 textures, but in a future update it will probably add more.    

## Credits
Mojang: Minecraft   
Notch: Textures    
C418: Music   

## License
This mod is under the GPL v3 (GNU General Public License v3.0) license, which means if you edit the mod's source code, you have to open source it under the GPL v3 license. (Info from discord.gg/monkemod)

## For Developers

If you're using DevMinecraftMod's events in your plugin, make sure you add the following in your .csproj file.

```cs
<Reference Include="DevMinecraftMod">
     <HintPath>$(PluginsPath)\DevMinecraftMod\DevMinecraftMod.dll</HintPath>
</Reference>
```

### Events
This mod also has events for placing and destroying blocks that you can use, them being `OnBlockPlaced` and `OnBlockDestroyed`. Here's an example of how the event could function.

```cs
using BepInEx;
using UnityEngine;
using DevMinecraftMod.RPC;

namespace ExampleTest
{
    [BepInDependency("org.dev9998.gorillatag.devminecraftmod", "1.0.2")]
    [BepInPlugin("com.example.gorillatag.exampletest", "ExampleTest", "1.0.0")]
    public class ExamplePlugin : BaseUnityPlugin
    {
        void Start()
        {
            MinecraftEvents.OnBlockPlaced += OnBlockPlaced;
            MinecraftEvents.OnBlockDestroyed += OnBlockDestroyed;
        }

        private void OnBlockPlaced(object sender, MinecraftEvents.BlockArgs args)
        {
            Debug.LogWarning($"{args.block} placed at {args.blockPosition}");
        }

        private void OnBlockDestroyed(object sender, MinecraftEvents.BlockArgs args)
        {
            Debug.LogWarning($"{args.block} destroyed at {args.blockPosition}");
        }
    }
}
```
