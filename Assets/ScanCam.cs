/* 
*   NatDevice ZXing Integration
*   Copyright (c) 2021 Yusuf Olokoba
*/

namespace NatSuite.Integrations {

    using UnityEngine;
    using UnityEngine.UI;
    using NatSuite.Devices;
    using ZXing;

    public sealed class ScanCam : MonoBehaviour {

        [Header(@"Camera Preview")]
        public RawImage rawImage;
        public AspectRatioFitter aspectFitter;

        ICameraDevice cameraDevice;
        IBarcodeReader barcodeReader;
        Texture2D previewTexture;
        Color32[] pixelBuffer;
        
        async void Start () {
            // Request permissions
            if (!await MediaDeviceQuery.RequestPermissions<CameraDevice>()) {
                Debug.LogError("User failed to grant camera permissions");
                return;
            }
            // Get camera
            var query = new MediaDeviceQuery(MediaDeviceCriteria.GenericCameraDevice);
            cameraDevice = query.current as ICameraDevice;
            // Start the camera preview
            cameraDevice.previewResolution = (1280, 720);
            previewTexture = await cameraDevice.StartRunning();
            // Create barcode reader
            barcodeReader = new BarcodeReader();
            pixelBuffer = previewTexture.GetPixels32();
            // Display preview
            rawImage.texture = previewTexture;
            aspectFitter.aspectRatio = (float)previewTexture.width / previewTexture.height;
        }

        void Update () {
            // Check
            if (cameraDevice == null || barcodeReader == null)
                return;
            // Detect
            previewTexture.GetRawTextureData<Color32>().CopyTo(pixelBuffer);
            var result = barcodeReader.Decode(pixelBuffer, previewTexture.width, previewTexture.height);
            // Check
            if (result == null)
                return;
            // Display
            Debug.Log($"Detected barcode: {result.Text}");
        }
    }
}