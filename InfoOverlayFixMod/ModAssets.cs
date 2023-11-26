using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace SquareLib
{
    // Token: 0x02000004 RID: 4
    public static class ModAssets
    {
        // Token: 0x06000005 RID: 5 RVA: 0x00002074 File Offset: 0x00000274
        public static TextureAtlas GetCustomTileAtlas(string name)
        {
            string text = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), name + ".png");
            TextureAtlas textureAtlas = null;
            try
            {
                byte[] array = File.ReadAllBytes(text);
                Texture2D texture2D = new Texture2D(2, 2);
                ImageConversion.LoadImage(texture2D, array);
                textureAtlas = ScriptableObject.CreateInstance<TextureAtlas>();
                textureAtlas.texture = texture2D;
                //  textureAtlas.vertexScale = ModAssets.TileAtlas.vertexScale; //注释这个
                textureAtlas.items = ModAssets.TileAtlas.items;
            }
            catch
            {
                Debug.LogError("[Item Permeable Tiles]: Could not load atlas image at path " + text);
            }
            return textureAtlas;
        }

        // Token: 0x06000006 RID: 6 RVA: 0x00002114 File Offset: 0x00000314
        public static void AddSpriteFromFile(string name)
        {
            Texture2D texture2D = ModAssets.LoadTexture(name, null);
            Sprite value = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2((float)texture2D.width / 2f, (float)texture2D.height / 2f));
            Assets.Sprites.Add(name, value);
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00002184 File Offset: 0x00000384
        public static Texture2D LoadTexture(string name, string folder = null)
        {
            Texture2D texture2D = null;
            string text = Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), folder ?? "assets"), name + ".png");
            try
            {
                byte[] array = File.ReadAllBytes(text);
                texture2D = new Texture2D(1, 1);
                ImageConversion.LoadImage(texture2D, array);
            }
            catch (Exception exception)
            {
                Debug.LogError(string.Concat(new string[]
                {
                    "Could not load texture at ",
                    folder ?? "assets",
                    "/",
                    text,
                    ".png"
                }));
                Debug.LogException(exception);
            }
            return texture2D;
        }

        // Token: 0x04000002 RID: 2
        private static readonly TextureAtlas TileAtlas = Assets.GetTextureAtlas("tiles_solid");
    }
}
