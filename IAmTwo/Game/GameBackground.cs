using IAmTwo.Resources;
using IAmTwo.Shaders;
using SM.Base.Windows;
using SM2D.Drawing;

namespace IAmTwo.Game
{
    public class GameBackground : DrawBackground
    {
        public GameBackground()
        {
            Texture = Resource.RequestTexture(@".\Resources\bloom_amountMap.png");

            TextureTransform = GameRenderPipeline.BloomAmountTransform;
        }

        protected override void DrawContext(ref DrawContext context)
        {
            Material.ShaderArguments["ColorScale"] = .05f;
            base.DrawContext(ref context);
        }
    }
}