using System;
using System.Collections.Generic;
using Reactor;
using Reactor.Extensions;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UwU.Patches
{
    public static class SpritePatches
    {
        private static UnityEngine.Object[] _assets;
        private static readonly Dictionary<string, Sprite> ReplacementSprites = new Dictionary<string, Sprite>();
        private static Dictionary<string, Vector2> _transformations;

        public static void Patch()
        {
            SceneManager.add_sceneLoaded((Action<Scene, LoadSceneMode>) ModifySprites);
        }

        private static void ModifySprites(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (!UwUPlugin.IsEnabled.Value) return;
            
            Logger<UwUPlugin>.Info("Scene reloaded - replacing sprites");

            if (_assets is null)
            {
                Logger<UwUPlugin>.Info("Loading assets");
                
                var bundle = AssetBundle.LoadFromMemory(typeof(UwUPlugin).Assembly
                    .GetManifestResourceStream("UwU.Assets.uwu").ReadFully());

                _assets = bundle.LoadAllAssets(Il2CppType.Of<Sprite>());
                
                bundle.Unload(false);

                Logger<UwUPlugin>.Info($"Loaded {_assets.Length} assets");

                var replacementSpriteNames = new Dictionary<string, string[]>
                {
                    { "AmongUwU", new[] { "AmongUsLogo", "bannerLogo_AmongUs" } },
                    { "local", new[] { "PlayLocalButton" } },
                    { "online", new[] { "PlayOnlineButton" } },
                    { "htp", new [] { "HowToPlayButton" } },
                    { "freeplay", new [] { "FreePlayButton" } }
                };

                foreach (var asset in _assets)
                {
                    if (!replacementSpriteNames.TryGetValue(asset.name, out var spritesToReplace)) continue;
                    
                    foreach (var spriteToReplace in spritesToReplace)
                    {
                        asset.DontUnload();
                        
                        ReplacementSprites[spriteToReplace] = asset.TryCast<Sprite>();
                    }
                }

                _transformations = new Dictionary<string, Vector2>
                {
                    { "AmongUsLogo", new Vector2(-0.25f, 0) }
                };
                
                Logger<UwUPlugin>.Info("Mapped assets");
            }

            var successfulReplacements = 0;
            foreach (var (originalName, replacementSprite) in ReplacementSprites)
            {
                var original = GameObject.Find(originalName);
                if (original is null)
                {
                    Logger<UwUPlugin>.Warning($"Failed to find object `{originalName}`");
                    continue;
                }
                
                if (replacementSprite is null) Logger<UwUPlugin>.Warning("sprite is null");

                original.GetComponent<ImageTranslator>()?.Destroy();
                original.GetComponent<SpriteRenderer>().sprite = replacementSprite;
                
                Logger<UwUPlugin>.Debug($"Replaced sprite of object `{originalName}`");
                successfulReplacements++;

                if (_transformations.TryGetValue(originalName, out var translation))
                {
                    original.transform.Translate(translation);
                }
            }

            Logger<UwUPlugin>.Info(
                $"Successfully replaced {successfulReplacements}/{ReplacementSprites.Count} sprites");
        }
    }
}