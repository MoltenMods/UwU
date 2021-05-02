using System.Text.RegularExpressions;
using HarmonyLib;
using Reactor;
using UnhollowerBaseLib;

namespace UwU.Patches
{
    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString), typeof(StringNames),
        typeof(Il2CppReferenceArray<Il2CppSystem.Object>))]
    [HarmonyBefore(ReactorPlugin.Id)]
    public static class GetStringPatch
    {
        public static void Postfix(ref string __result)
        {
            __result = UwUifier.Convert(__result);
        }
    }

    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetStringWithDefault))]
    [HarmonyBefore(ReactorPlugin.Id)]
    public static class GetStringWithDefaultPatch
    {
        public static void Postfix(ref string __result)
        {
            __result = UwUifier.Convert(__result);
        }
    }

    public static class UwUifier
    {
        public static string Convert(string original)
        {
            original = Regex.Replace(original, @"(?:r|l)", "w");
            original = Regex.Replace(original, @"(?:R|L)", "W");
            original = Regex.Replace(original, @"n([aeiou])", "ny$1");
            original = Regex.Replace(original, @"N([aeiou])", "Ny$1");
            original = Regex.Replace(original, @"N([AEIOU])", "Ny$1");
            original = Regex.Replace(original, @"ove", "uv");

            return original;
        }
    }
}