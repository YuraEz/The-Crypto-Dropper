using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotosController : MonoBehaviour
{
    public static IEnumerator loadImageFromGallery(System.Action<Texture2D> onImageLoaded)
    {
        // Запрашиваем изображение из галереи
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                // Загружаем изображение в Texture2D
                Texture2D texture = LoadImage(path);

                if (texture != null)
                {
                    // Вызываем коллбэк с загруженным изображением
                    onImageLoaded?.Invoke(texture);
                }
                else
                {
                    onImageLoaded?.Invoke(null);
                }
            }
            else
            {
                onImageLoaded?.Invoke(null);
            }
        }, "Select a PNG or JPG image", "image/*");

        // Ждем, пока изображение будет загружено
        yield return new WaitForEndOfFrame();
    }

    // Метод для загрузки изображения из файла в Texture2D
    private static Texture2D LoadImage(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        byte[] fileData = System.IO.File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        bool isLoaded = texture.LoadImage(fileData);

        if (isLoaded)
        {
            return texture;
        }
        else
        {
            return null;
        }
    }
    public static Sprite SpriteFromTexture(Texture2D textureToSet)
    {
        return Sprite.Create(textureToSet, new Rect(0, 0, textureToSet.width, textureToSet.height), new Vector2(0.5f, 0.5f));
    }

    public static Texture2D TextureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }
}
