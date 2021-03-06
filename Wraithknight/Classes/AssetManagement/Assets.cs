﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    public static class Assets
    {
        private static ContentManager _content;
        private static GraphicsDevice _graphics;
        //private static int _loadedTextures;


        private static Dictionary<String, RessourceTexture> TextureLibrary = new Dictionary<String, RessourceTexture>();
        private static Dictionary<String, RessourceSoundEffect> SoundLibrary = new Dictionary<String, RessourceSoundEffect>();
        private static Dictionary<String, RessourceFont> FontLibrary = new Dictionary<String, RessourceFont>();

        public static void Initialize(GraphicsDevice graphics, ContentManager content)
        {
            _content = content;
            _graphics = graphics;
            //_loadedTextures = 0;
            SetDummy();
        }

        private static void SetDummy()
        {
            Texture2D dummy = new Texture2D(_graphics, 1, 1);
            dummy.SetData(new Color[] { Color.White });
            TextureLibrary.Add("DummyTexture", new RessourceTexture(dummy));
        }


        public static Texture2D GetTexture(String filePath)
        {
            if (TextureLibrary.ContainsKey(filePath))
            {
                if (!TextureLibrary[filePath].Permanent) TextureLibrary[filePath].RefCount++;
                return TextureLibrary[filePath].Texture;
            }

            Texture2D texture = _content.Load<Texture2D>(filePath);
            TextureLibrary.Add(filePath, new RessourceTexture(texture)); // maybe implement permanent storage? FLAGS
            return texture;
        }

        public static void UnloadTexture(String filePath)
        {
            if (TextureLibrary.ContainsKey(filePath))
            {
                RessourceTexture ressource = TextureLibrary[filePath];
                if (ressource.Permanent) return;
                ressource.RefCount--;
                if (ressource.RefCount <= 0) TextureLibrary.Remove(filePath);
            }
        }

        public static SoundEffect GetSound(String filePath)
        {
            if (SoundLibrary.ContainsKey(filePath))
            {
                return SoundLibrary[filePath].SoundEffect;
            }

            SoundEffect SoundEffect = _content.Load<SoundEffect>(filePath);
            SoundLibrary.Add(filePath, new RessourceSoundEffect(SoundEffect)); // maybe implement permanent storage? FLAGS
            return SoundEffect;
        }

        public static SpriteFont GetFont(String filePath)
        {
            if (FontLibrary.ContainsKey(filePath))
            {
                if (!FontLibrary[filePath].Permanent) FontLibrary[filePath].RefCount++;
                return FontLibrary[filePath].Font;
            }

            SpriteFont font = _content.Load<SpriteFont>(filePath);
            FontLibrary.Add(filePath, new RessourceFont(font)); // maybe implement permanent storage? FLAGS
            return font;
        }


        //maybe change access?
        #region internal Classes
        private class RessourceTexture
        {
            internal readonly Texture2D Texture;
            internal int RefCount;
            internal readonly bool Permanent;

            internal RessourceTexture(Texture2D texture, bool permanent = false)
            {
                Texture = texture;
                RefCount = 1;
                Permanent = permanent;
            }
        }

        private class RessourceSoundEffect
        {
            internal readonly SoundEffect SoundEffect;
            internal int RefCount;

            internal RessourceSoundEffect(SoundEffect soundEffect, bool permanent = false)
            {
                SoundEffect = soundEffect;
                RefCount = 1;
            }
        }

        private class RessourceFont
        {
            internal readonly SpriteFont Font;
            internal int RefCount;
            internal readonly bool Permanent;

            internal RessourceFont(SpriteFont font, bool permanent = false)
            {
                Font = font;
                RefCount = 1;
                Permanent = permanent;
            }
        }
        #endregion
    }
}
