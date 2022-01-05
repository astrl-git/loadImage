using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class PNGPreviewItem : MonoBehaviour
{
    [SerializeField] private Image previewImage;
    [SerializeField] private TextMeshProUGUI fileName;
    [SerializeField] private TextMeshProUGUI createdTimer;

    private void OnValidate()
    {
        Assert.IsNotNull(previewImage, $"{nameof(previewImage)} is null");
        Assert.IsNotNull(fileName, $"{nameof(fileName)} is null");
        Assert.IsNotNull(createdTimer, $"{nameof(createdTimer)} is null");
    }
    
    
}