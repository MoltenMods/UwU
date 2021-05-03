using System;
using System.Collections.Generic;
using Reactor.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UwU.Patches
{
    public static class SpritePatches
    {
        private static AssetBundle _bundle;
        private static Dictionary<string, Sprite> _replacementSprites;
        private static Dictionary<string, Vector2> _transformations;

        public static void Patch()
        {
            SceneManager.add_sceneLoaded((Action<Scene, LoadSceneMode>) ModifySprites);
        }

        private static void ModifySprites(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (!UwUPlugin.IsEnabled.Value) return;
            
            if (!_bundle)
            {
                _bundle = AssetBundle.LoadFromMemory(typeof(UwUPlugin).Assembly
                    .GetManifestResourceStream("UwU.Assets.uwu").ReadFully());

                var amongUwUSprite = _bundle.LoadAsset<Sprite>("AmongUwU").DontUnload();

                _replacementSprites = new Dictionary<string, Sprite>()
                {
                    { "AmongUsLogo", amongUwUSprite },
                    { "bannerLogo_AmongUs", amongUwUSprite }
                };

                _transformations = new Dictionary<string, Vector2>()
                {
                    { "AmongUsLogo", new Vector2(-0.25f, 0) }
                };

                _bundle.Unload(false);
            }
            
            foreach (var (originalName, replacementSprite) in _replacementSprites)
            {
                var original = GameObject.Find(originalName);
                if (!original) return;
                
                original.GetComponent<SpriteRenderer>().sprite = replacementSprite;

                if (_transformations.TryGetValue(originalName, out var translation))
                {
                    original.transform.Translate(translation);
                }
            }
        }
    }
}