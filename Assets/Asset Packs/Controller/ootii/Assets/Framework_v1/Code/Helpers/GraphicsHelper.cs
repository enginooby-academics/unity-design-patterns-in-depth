using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace com.ootii.Helpers
{
    // CDL 08/05/2018 - Helpful graphics utility functions
    public static class GraphicsHelper
    {
        /// <summary>
        /// Convert a Texture2D to a Sprite object
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static Sprite TextureToSprite(Texture2D texture)
        {
            if (texture == null) { return null; }
            return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
        }

        /// <summary>
        /// Convert an older Texture format to a Sprite object
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static Sprite TextureToSprite(Texture texture)
        {
            return TextureToSprite(texture as Texture2D);
        }

        /// <summary>
        /// Load an image from disk
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Texture2D LoadImage(string filePath)
        {
            Texture2D texture = null;
            if (File.Exists(filePath))
            {
                byte[] fileData = File.ReadAllBytes(filePath);
                texture = new Texture2D(2, 2);
                // Loading will auto resize the texture dimensions
                texture.LoadImage(fileData);
            }

            return texture;
        }

        /// <summary>
        /// Save an image in PNG format
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="filePath"></param>
        public static void SaveAsPNG(Texture2D texture, string filePath)
        {
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);
        }

        /// <summary>
        /// Save an image in JPG format
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="filePath"></param>
        /// <param name="quality"></param>
        public static void SaveAsJPG(Texture2D texture, string filePath, int quality = 100)
        {
            byte[] bytes = texture.EncodeToJPG(quality);
            File.WriteAllBytes(filePath, bytes);
        }

        /// <summary>
        /// Take a screenshot using the designated camera in 24-bit color depth
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Texture2D TakeScreenshot(Camera camera, int width, int height)
        {
            RenderTexture renderTexture = new RenderTexture(width, height, 24);
            camera.targetTexture = renderTexture;
            Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
            camera.Render();
            RenderTexture.active = renderTexture;
            screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null;
            Object.Destroy(renderTexture);

            return screenshot;
        }

        /// <summary>
        /// Create a texture by assigning a color to an array of pixels. This can be used
        /// to give EditorGUI elements a foreground color or background color
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Texture2D CreateTexture(int width, int height, Color color)
        {
            var pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            var texture = new Texture2D(width, height);
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }

        /// <summary>
        /// Create a texture with a border by assigning a color to pixel arrays.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="textureColor"></param>
        /// <param name="border"></param>
        /// <param name="borderColor"></param>
        /// <returns></returns>
        public static Texture2D CreateTexture(int width, int height, Color textureColor, RectOffset border,
            Color borderColor)
        {
            int innerWidth = width;
            width += border.left;
            width += border.right;

            var pixels = new Color[width * (height + border.top + border.bottom)];

            for (int i = 0; i < pixels.Length; i++)
            {
                if (i < (border.bottom * width))
                    pixels[i] = borderColor;
                else if (i >= ((border.bottom * width) + (height * width)))  //Border Top
                    pixels[i] = borderColor;
                else
                { //Center of Texture

                    if ((i % width) < border.left) // Border left
                        pixels[i] = borderColor;
                    else if ((i % width) >= (border.left + innerWidth)) //Border right
                        pixels[i] = borderColor;
                    else
                        pixels[i] = textureColor;    //Color texture
                }
            }

            var texture = new Texture2D(width, height + border.top + border.bottom);
            texture.SetPixels(pixels);
            texture.Apply();

            return texture;
        }

    }
}
