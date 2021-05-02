using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;

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
        public const string Version = "0.1.0";

        public Harmony Harmony { get; } = new Harmony(Id);

        public override void Load()
        {
            Harmony.PatchAll();
        }
    }
}