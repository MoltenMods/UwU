using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UwU.Controls;

namespace UwU.Patches
{
    public static class InterfacePatches
    {
        private static Button _toggleButton;
        
        public static async void Patch()
        {
            Button.InitializeBaseButton();

            _toggleButton = await Button.Create($"{(UwUPlugin.IsEnabled.Value ? "Disable" : "Enable")} UwU",
                new Vector2(1.5f, 0.4f), new Vector3(0.85f, 0.3f, 0));
            
            _toggleButton.OnClick.AddListener((Action) (() =>
            {
                UwUPlugin.IsEnabled.Value = !UwUPlugin.IsEnabled.Value;
                _toggleButton.Text.text = $"{(UwUPlugin.IsEnabled.Value ? "Disable": "Enable")} UwU";
                SceneManager.LoadScene("MainMenu");
            }));
            _toggleButton.Position.Alignment = AspectPosition.EdgeAlignments.RightBottom;
            _toggleButton.GameObject.SetActive(true);
            
            SceneManager.add_sceneLoaded((Action<Scene, LoadSceneMode>) CheckToggleButton);
        }

        private static void CheckToggleButton(Scene scene, LoadSceneMode loadSceneMode)
        {
            _toggleButton.GameObject.SetActive(scene.name == "MainMenu");
        }
    }
}