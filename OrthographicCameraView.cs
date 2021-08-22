using UnityEngine;
using VoxelTycoon;

namespace VoxelScreenshoterMod
{
    public class OrthographicCameraView : ICameraView
    {
        public Vector3 AudioListenerPosition { get; }
        public TiltShiftSettings TiltShiftSettings { get; }
        
        private const float DefaultRotationX = 90f;
        private const float DefaultRotationY = 45f;
        
        private Camera _camera;
        private Vector3 _initialPos;
        private float _initialDistance;  
        private float _currentDistance;
        private float _targetDistance;
        private Vector3 _center;
        
        public void OnEnable(CameraController cameraController)
        {
            _camera = cameraController.Camera;
            _camera.orthographic = true;
            
            var gameCameraView = GameCameraView.Current;
            _initialDistance = gameCameraView.Distance;
            _initialPos = gameCameraView.Target;
        }

        public void Init(Vector3 center, float distance)
        {
            _center = center;
            _targetDistance = distance;
            GoTo(_center, _targetDistance);
        }
        
        public void OnDisable()
        {
            _camera.orthographic = false;
            GoTo(_initialPos, _initialDistance);
        }

        public void OnUpdate()
        {
            _camera.orthographicSize += Time.deltaTime * 15f;
        }

        public void OnLateUpdate() { }

        private void GoTo(Vector3 target, float distance)
        {    
            _currentDistance = distance;
            _camera.transform.position = new Vector3(target.x, 10, target.z);
            _camera.transform.rotation = Quaternion.Euler(new Vector3(DefaultRotationX, DefaultRotationY, 0.0f));
            _camera.orthographicSize = _currentDistance;
            //_camera.orthographicSize = 0;
        }
    }
}