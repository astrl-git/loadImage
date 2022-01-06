using System;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace PngPreview.Dialogs
{
    public class PermissionDeniedDialog : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI message;
        [SerializeField] private Button openSettings;
        [SerializeField] private Button requestPermission;

        private void OnValidate()
        {
            Assert.IsNotNull(message, $"{nameof(message)} is null");
            Assert.IsNotNull(openSettings, $"{nameof(openSettings)} is null");
            Assert.IsNotNull(requestPermission, $"{nameof(requestPermission)} is null");
        }

        private void Awake()
        {
            Close();
        }

        private void OnEnable()
        {
            openSettings.onClick.AsObservable().TakeUntilDisable(gameObject).Subscribe(i => OpenSettings());
            requestPermission.onClick.AsObservable().TakeUntilDisable(gameObject).Subscribe(i => RequestPermissions());
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!gameObject.activeSelf) return;

            var permission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Read);
            
            HandlePermission(permission);
        }

        public void Show(NativeGallery.Permission permission)
        {
            HandlePermission(permission);
            gameObject.SetActive(true);
        }

        private void HandlePermission(NativeGallery.Permission permission)
        {
            SetMessage(permission);

            switch (permission)
            {
                case NativeGallery.Permission.Denied:
                    openSettings.gameObject.SetActive(true);
                    requestPermission.gameObject.SetActive(false);
                    break;
                case NativeGallery.Permission.ShouldAsk:
                    openSettings.gameObject.SetActive(false);
                    requestPermission.gameObject.SetActive(true);
                    break;
                case NativeGallery.Permission.Granted:
                    Close();
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected Permission Enum: {permission}");
            }
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }

        private void RequestPermissions()
        {
            var permission = NativeGallery.RequestPermission(NativeGallery.PermissionType.Read);
            HandlePermission(permission);
        }

        private void OpenSettings()
        {
            NativeGallery.OpenSettings();
        }

        private void SetMessage(NativeGallery.Permission permission)
        {
            message.text = $"Files read permission: {permission}";
        }
    }
}