using System.Text.RegularExpressions;
using HarmonyLib;
using UnhollowerBaseLib;

namespace UwU.Patches
{
    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString), typeof(StringNames),
        typeof(Il2CppReferenceArray<Il2CppSystem.Object>))]
    public static class GetStringPatch
    {
        public static void Postfix(ref string __result)
        {
            __result = UwUifier.Convert(__result);
        }
    }

    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString), typeof(SystemTypes))]
    public static class GetRoomStringPatch
    {
        public static void Postfix(ref string __result)
        {
            __result = UwUifier.Convert(__result);
        }
    }
    
    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString), typeof(TaskTypes))]
    public static class GetTaskStringPatch
    {
        public static void Postfix(ref string __result)
        {
            __result = UwUifier.Convert(__result);
        }
    }

    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetStringWithDefault))]
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
            if (!UwUPlugin.IsEnabled.Value) return original;
            
            original = Regex.Replace(original, @"(?:r|l)", "w");
            original = Regex.Replace(original, @"(?:R|L)", "W");
            
            original = Regex.Replace(original, @"n([aeiou])", "ny$1");
            original = Regex.Replace(original, @"N([aeiou])", "Ny$1");
            original = Regex.Replace(original, @"N([AEIOU])", "Ny$1");
            
            original = Regex.Replace(original, @"ove", "uv");
            original = Regex.Replace(original, @"the", "da");
            original = Regex.Replace(original, @"is", "ish");
            original = Regex.Replace(original, @"you", "u");
            original = Regex.Replace(original, @"You", "U");

            original = Regex.Replace(original, @"among (U|u)s", "among UwU");
            original = Regex.Replace(original, @"Among (U|u)s", "Among UwU");

            return original;
        }
    }
}