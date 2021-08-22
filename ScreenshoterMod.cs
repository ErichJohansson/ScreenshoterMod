using VoxelTycoon.Modding;
using UnityEngine;
using UnityEngine.UI;
using VoxelTycoon;
using VoxelTycoon.UI;

namespace VoxelScreenshoterMod
{
    public class ScreenshoterMod : Mod
    {
        private float _buttonOffset = 40f;
        private Screenshoter _screenshoter;
        
        protected override void OnGameStarted()
        {
            GameObject cameraBtn = GameObject.Find("ModernGameUI/Camera");

            // Creating a new GameObject, then adding required components to it, setting its position, etc
            var screenshoterObject = GameObject.Instantiate(cameraBtn, cameraBtn.transform.parent);
            _screenshoter = screenshoterObject.AddComponent<Screenshoter>();
            
            var screenshoterRect = screenshoterObject.GetComponent<RectTransform>();
            screenshoterRect.anchoredPosition += new Vector2(_buttonOffset, 0);

            var screenshoterBtn = screenshoterObject.GetComponentInChildren<Button>();
            screenshoterBtn.onClick.RemoveAllListeners();
            screenshoterBtn.onClick.AddListener(ToggleScreenshoter);

            Tooltip.For(screenshoterBtn, $"Screenshoter\n{$"Makes a 4K screenshot of your home region every 60 seconds".Colorify(UIColors.Popup.SubtleText)}");
        }

        private void ToggleScreenshoter()
        {
            if (_screenshoter) _screenshoter.ToggleScreenshotRoutine();
        }
    }
}