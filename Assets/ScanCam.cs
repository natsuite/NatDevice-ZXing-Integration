/* 
*   NatSuite Integrations
*   Copyright (c) 2020 Yusuf Olokoba
*/

namespace NatSuite.Integrations {

    using UnityEngine;
    using UnityEngine.UI;
    using Devices;

    public sealed class ScanCam : MonoBehaviour {

        [Header(@"Camera Preview")]
        public RawImage rawImage;
        public AspectRatioFitter aspectFitter;

        Texture2D previewTexture;

        async void Start () {
            // Request permissions
            if (!await MediaDeviceQuery.RequestPermissions<CameraDevice>()) {
                Debug.LogError("User failed to grant camera permissions");
                return;
            }
            // Get camera
            var query = new MediaDeviceQuery(MediaDeviceQuery.Criteria.GenericCameraDevice);
            var cameraDevice = query.currentDevice as ICameraDevice;
            // Start the camera preview
            cameraDevice.previewResolution = (1280, 720);
            previewTexture = await cameraDevice.StartRunning();
            // Display preview
            rawImage.texture = previewTexture;
            aspectFitter.aspectRatio = (float)previewTexture.width / previewTexture.height;
        }

        void Update () {

        }
    }
}