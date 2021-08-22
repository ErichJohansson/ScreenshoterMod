using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using VoxelTycoon;

namespace VoxelScreenshoterMod
{
    public class Screenshoter : MonoBehaviour
    {
        private bool _screenshoterActive;
        private Coroutine _screenshotRoutine;
        private float _timeInterval = 60f;
        private int _width = 4096;
        private int _height = 3072;
        private Image _background;
        private Vector3 _center;
        private float _orthoSize;
        
        private void OnEnable()
        {
            _background = GetComponentInChildren<Image>();
            _background.color = Color.white;

            var homeRegion = Manager<RegionManager>.Current.HomeRegion;
            var center = homeRegion.Center;
            _center = new Vector3(center.X, 0, center.Z);

            foreach (var point in homeRegion.ColliderPath)
            {
                var max = Mathf.Max(point.x, point.y);
                if (max > _orthoSize) _orthoSize = max;
            }
            _orthoSize /= 2;
        }

        public void ToggleScreenshotRoutine()
        {
            _screenshoterActive = !_screenshoterActive;
            if (_screenshotRoutine != null)
            {
                _background.color = Color.white;
                StopCoroutine(_screenshotRoutine);
                CameraController.Current.TryRestoreDefaultView();
            }
            else
            {
                _background.color = Color.green;
                _screenshotRoutine = StartCoroutine(ScreenshotRoutine());
            }
        }

        private IEnumerator ScreenshotRoutine()
        {
            while (_screenshoterActive)
            {
                yield return new WaitForSeconds(_timeInterval);
                var orthoView = new OrthographicCameraView();
                CameraController.Current.View = orthoView;
                orthoView.Init(_center, _orthoSize);
                yield return new WaitForEndOfFrame();
                MakeScreenshot();
                yield return new WaitForEndOfFrame();
                CameraController.Current.TryRestoreDefaultView();
            }
        }

        private void MakeScreenshot()
        {
            var bytes = Helper.TakeScreenshot(_width, _height, false).EncodeToPNG();

            var path = Path.Combine(Application.persistentDataPath, "Screenshots");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = Path.Combine(path, $"Screenshot_{DateTime.Now:yyyy-mm-dd hh-mm-ss}.png").Replace('\\', '/');

            using (FileStream fs = File.Open(path, FileMode.OpenOrCreate))
            {
                fs.Seek(0, SeekOrigin.End);
                fs.Write(bytes, 0, bytes.Length);
            }
        }
    }
}