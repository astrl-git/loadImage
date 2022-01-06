using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace PngPreview.Dialogs
{
    public class ErrorDialog : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button okButton;

        private void OnValidate()
        {
            Assert.IsNotNull(okButton, $"{nameof(okButton)} is null");
            Assert.IsNotNull(messageText, $"{nameof(messageText)} is null");
        }

        private void Awake()
        {
            Close();
        }

        public void Show(string message)
        {
            messageText.text = message;
            gameObject.SetActive(true);
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            okButton.onClick.AddListener(Close);
        }

        private void OnDisable()
        {
            okButton.onClick.RemoveListener(Close);
        }
    }
}