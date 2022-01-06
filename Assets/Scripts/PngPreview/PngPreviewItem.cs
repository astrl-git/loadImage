using System;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace PngPreview
{
    public class PngPreviewItem : MonoBehaviour
    {
        const string FORMAT = @"d\D\a\y\s\,\ hh\:mm\:ss";

        public class ImageData
        {
            public Texture2D texture;
            public string fullPath;
            public string fileName;
            public DateTime createdDate;

            public ImageData(string fullPath, Texture2D texture, DateTime createdDate)
            {
                this.fullPath = fullPath;
                this.texture = texture;
                this.createdDate = createdDate;
                fileName = fullPath.Split('/').Last();
            }
        }

        [SerializeField] private Image previewImage;
        [SerializeField] private TextMeshProUGUI fileName;
        [SerializeField] private TextMeshProUGUI createdTimer;
        [SerializeField] private Button removeItemButton;

        private ImageData _data;
        private PngImagesController _controller;

        private void OnValidate()
        {
            Assert.IsNotNull(previewImage, $"{nameof(previewImage)} is null");
            Assert.IsNotNull(fileName, $"{nameof(fileName)} is null");
            Assert.IsNotNull(createdTimer, $"{nameof(createdTimer)} is null");
            Assert.IsNotNull(removeItemButton, $"{nameof(removeItemButton)} is null");
        }

        private void Awake()
        {
            removeItemButton.onClick
                .AsObservable()
                .TakeUntilDisable(gameObject)
                .Subscribe(i =>
                {
                    if (string.IsNullOrEmpty(_data?.fullPath))
                    {
                        Debug.LogError($"Failed to remove list item due to missed image path data");
                        return;
                    }

                    if (_controller != null) _controller.RemoveItem(_data?.fullPath);
                });
        }

        public void Init(ImageData data, PngImagesController controller)
        {
            if (data == null || controller == null)
            {
                Debug.LogError($"Failed to Init {nameof(PngPreviewItem)} due to args is/are null");
                return;
            }

            _data = data;
            _controller = controller;
            _controller.Timer.TakeUntilDisable(gameObject).Subscribe(i => Tick());

            Tick();
            SetUIAccordingToData(_data);
        }

        public void UpdateData(ImageData data)
        {
            if (data == null)
            {
                Debug.LogError($"Failed to UpdateData due to arg is null");
                return;
            }

            _data = data;
            SetUIAccordingToData(_data);
        }

        private void Tick()
        {
            if (_data?.createdDate == default) return;

            var utcNow = DateTime.UtcNow;
            var timespan = utcNow - _data.createdDate.ToUniversalTime();
            var res = timespan.ToString(FORMAT);

            createdTimer.text = res;
        }

        private Sprite TextureToSprite(Texture2D texture) => Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2);

        private void SetUIAccordingToData(ImageData data)
        {
            if (data.texture == null)
            {
                Debug.LogError($"Failed to display an image due to texture data is null");
            }
            else
            {
                previewImage.sprite = TextureToSprite(data.texture);
            }

            fileName.text = data.fileName;
        }
    }
}