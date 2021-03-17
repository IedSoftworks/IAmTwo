using IAmTwo.Resources;
using OpenTK.Graphics.OpenGL4;
using SM.Base.Drawing;
using SM.Base.Textures;
using SM2D.Drawing;

namespace IAmTwo.Menu
{
    public class ImageButton : Button
    {
        public TextureTransformation TextureTransformation;

        public ImageButton(Texture image, float? width = null, float? height = null, float corners = 10) : base("", width)
        {
            Objects.Clear();

            float w = width ?? 1f;
            DrawObject2D imageDisplay = new DrawObject2D()
            {
                Texture = image,
                Material = {Blending = true}
            };
            if (height.HasValue)
                imageDisplay.Transform.Size.Set(w, height.Value);
            else imageDisplay.Transform.ApplyTextureSize(image, w);

            TextureTransformation = imageDisplay.TextureTransform;

            _border = new DrawObject2D
            {
                Mesh = Models.CreateBackgroundPolygon(imageDisplay.Transform.Size, corners),
                ForcedMeshType = PrimitiveType.LineLoop,
                ShaderArguments = { ["ColorScale"] = 1.4f },
            };
            _border.Transform.Size.Set(1.3f);

            Add(imageDisplay, _border);
        }
    }
}