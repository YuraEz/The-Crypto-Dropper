using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPhotoButton : MonoBehaviour
{
    [SerializeField] private Button clickbtn;
    [SerializeField] private Image icon;
    [SerializeField] private bool showDefaultOnEnable = true;
    
    [SerializeField] private Sprite defaultSprite;

    public Image Icon { get => icon; }

    public bool IsDefaultIcon => icon.sprite == defaultSprite;

    private void OnEnable()
    {
        if (showDefaultOnEnable && defaultSprite != null)
        {
            icon.sprite = defaultSprite;
        }
    }

    private void Start()
    {
        clickbtn.onClick.AddListener(() =>
        {
            StartCoroutine(PhotosController.loadImageFromGallery((texture) =>
            {
                if (texture != null)
                {
                    Sprite sprite = PhotosController.SpriteFromTexture(texture);
                    icon.sprite = sprite;
                }
            }));
        });
    }


    public void SetPhotoDefaut()
    {
        icon.sprite = defaultSprite;
    }
}
