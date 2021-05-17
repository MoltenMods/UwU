using System;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;
using Reactor.Patches;
using TMPro;
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
        public const string Version = "2.0.0-pre.1";

        public Harmony Harmony { get; } = new Harmony(Id);

        public static ConfigEntry<bool> IsEnabled;
        public static ConfigEntry<bool> Lowercase;

        public override void Load()
        {
            ConfigureConfig();
            
            SpritePatches.Patch();
            InterfacePatches.Patch();
            
            ReactorVersionShower.TextUpdated += text =>
            {
                text.text = UwUifier.Convert(text.text);
            };

            Harmony.PatchAll();
        }

        private void ConfigureConfig()
        {
            IsEnabled = Config.Bind("Preferences", "Enabled", true, "Whether Among Us should be UwUified.");
            Lowercase = Config.Bind("Preferences", "Lowercase", true, "Whether text should be lowercased.");
        }
        
        [HarmonyPatch(typeof(TextTranslatorTMP), nameof(TextTranslatorTMP.Start))]
        public static class TranslateStartPatch
        {
            public static void Postfix(TextTranslatorTMP __instance)
            {
                if (!IsEnabled.Value) return;
                
                var textMeshPro = __instance.GetComponent<TextMeshPro>();
                textMeshPro.text = UwUifier.Convert(textMeshPro.text);
                textMeshPro.ForceMeshUpdate(true, true);
            }
        }

        [HarmonyPatch(typeof(TextTranslatorTMP), nameof(TextTranslatorTMP.ResetText))]
        public static class TranslatePatch
        {
            public static void Postfix(TextTranslatorTMP __instance)
            {
                if (!IsEnabled.Value) return;
                
                var textMeshPro = __instance.GetComponent<TextMeshPro>();
                textMeshPro.text = UwUifier.Convert(textMeshPro.text);
                textMeshPro.ForceMeshUpdate(true, true);
            }
        }

        private static class NonTranslatedTextPatches
        {
            [HarmonyPatch(typeof(TextMeshPro), nameof(TextMeshPro.Rebuild))]
            public static class ModifyTextPatch
            {
                private static readonly string[] IgnoredObjects =
                {
                    "UserName_TMP",
                    "NameText",
                    "NameText_TMP",
                    "PlayerCounter_TMP",
                    "GameRoomName_TMP",
                    "TaskText_TMP",
                    "File(Clone)"
                };

                public static void Postfix(TextMeshPro __instance)
                {
                    if (!__instance || !IsEnabled.Value) return;
                    
                    if (IgnoredObjects.Contains(__instance.gameObject.name) ||
                        __instance.transform.parent &&
                        IgnoredObjects.Contains(__instance.transform.parent.name)) return;

                    var textTranslator = __instance.gameObject.GetComponent<TextTranslatorTMP>();
                    if (textTranslator && textTranslator.isActiveAndEnabled) return;

                    __instance.text = UwUifier.Convert(__instance.GetParsedText());
                    __instance.ForceMeshUpdate(true, true);
                }
            }
        }
    }
}