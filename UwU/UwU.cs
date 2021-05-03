using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;
using UwU.Patches;

namespace UwU
{
    [BepInPlugin(Id, Name, Version)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    [ReactorPluginSide(PluginSide.ClientOnly)]
    public class UwUPlugin : BasePlugin
    {
        public const string Id = "daemon.uwu";
        public const string Name = "UwU";
        public const string Version = "1.0.0";

        public Harmony Harmony { get; } = new Harmony(Id);

        public static ConfigEntry<bool> IsEnabled;

        public override void Load()
        {
            IsEnabled = Config.Bind("Preferences", "Enabled", true, "Whether Among Us should be UwUified.");
            
            SpritePatches.Patch();
            InterfacePatches.Patch();
            
            Harmony.PatchAll();
        }
    }
}