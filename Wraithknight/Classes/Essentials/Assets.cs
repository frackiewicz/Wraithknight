using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    //TODO Further Develop this
    public static class Assets
    {
        private static ContentManager _content;
        private static GraphicsDevice _graphics;
        private static int _loadedTextures;


        private static Dictionary<String, RessourceTexture> TextureLibrary = new Dictionary<String, RessourceTexture>();
        //TODO Sounds
        private static Dictionary<String, SpriteFont> FontLibrary = new Dictionary<String, SpriteFont>();

        public static void Initialize(GraphicsDevice graphics, ContentManager content)
        {
            _content = content;
            _graphics = graphics;
            _loadedTextures = 0;
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

            Texture2D Texture = _content.Load<Texture2D>(filePath);
            TextureLibrary.Add(filePath, new RessourceTexture(Texture)); // maybe implement permanent storage? FLAGS
            return Texture;
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


        //maybe change access?
        #region internal Classes
        private class RessourceTexture
        {
            internal Texture2D Texture;
            internal int RefCount;
            internal Boolean Permanent;

            internal RessourceTexture(Texture2D texture)
            {
                Texture = texture;
                RefCount = 1;
                Permanent = false;
            }

            internal RessourceTexture(Texture2D texture, Boolean permanent)
            {
                Texture = texture;
                RefCount = 1;
                Permanent = permanent;
            }
        }
        #endregion
    }
}
