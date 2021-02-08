using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using SM.Base.Textures;

namespace IAmTwo.Resources
{
    public class Resource
    {
        private static Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();

        public static Texture RequestTexture(string path, TextureMinFilter filter = TextureMinFilter.Linear, TextureWrapMode mode = TextureWrapMode.Repeat)
        {
            if(!_textures.ContainsKey(path))
                _textures.Add(path, new Texture(new Bitmap(path), filter, mode));
            return _textures[path];
        }
    }
}