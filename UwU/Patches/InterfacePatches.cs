using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UwU.Controls;

namespace UwU.Patches
{
    public static class InterfacePatches
    {
        private static Button _toggleButton;
        private static Button _lowercaseButton;
        
        public static async void Patch()
        {
            Button.InitializeBaseButton();

            await SetUpToggleButton();
            await SetUpLowercaseButton();
            
            SceneManager.add_sceneLoaded((Action<Scene, LoadSceneMode>) CheckButtons);
        }
        
        private static async Task SetUpToggleButton()
        {
            _toggleButton = await Button.Create($"{(UwUPlugin.IsEnabled.Value ? "Disable" : "Enable")} UwU", 
                new Vector2(1.5f, 0.4f), new Vector3(0.85f, 0.3f, 0));
                        
            _toggleButton.OnClick.AddListener((Action) (() =>
            {
                UwUPlugin.IsEnabled.Value = !UwUPlugin.IsEnabled.Value;
                ResetButtons();
                SceneManager.LoadScene("MainMenu");
            }));
            
            _toggleButton.Position.Alignment = AspectPosition.EdgeAlignments.RightBottom;
            _toggleButton.GameObject.SetActive(true);
        }

        private static async Task SetUpLowercaseButton()
        {
            _lowercaseButton = await Button.Create($"{(UwUPlugin.Lowercase.Value ? "Disable" : "Enable")} Lowercase",
                new Vector2(2.1f, 0.4f), new Vector3(1.15f, 0.8f, 0));
            
            _lowercaseButton.OnClick.AddListener((Action) (() =>
            {
                UwUPlugin.Lowercase.Value = !UwUPlugin.Lowercase.Value;
                ResetButtons();
                SceneManager.LoadScene("MainMenu");
            }));

            _lowercaseButton.Position.Alignment = AspectPosition.EdgeAlignments.RightBottom;
            _lowercaseButton.GameObject.SetActive(UwUPlugin.IsEnabled.Value);
        }

        private static void CheckButtons(Scene scene, LoadSceneMode loadSceneMode)
        {
            var showButtons = scene.name == "MainMenu";
            var isEnabled = UwUPlugin.IsEnabled.Value;
            
            if (!showButtons) ResetButtons();

            _toggleButton.GameObject.SetActive(showButtons);
            _lowercaseButton.GameObject.SetActive(showButtons && isEnabled);
        }

        private static void ResetButtons()
        {
            _toggleButton.Text.text = $"{(UwUPlugin.IsEnabled.Value ? "Disable" : "Enable")} UwU";

            if (!UwUPlugin.IsEnabled.Value) return;
            
            _lowercaseButton.Text.text = $"{(UwUPlugin.Lowercase.Value ? "Disable" : "Enable")} Lowercase";
        }
    }
}