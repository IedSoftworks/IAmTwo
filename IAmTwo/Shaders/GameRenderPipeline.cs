using System.Drawing;
using System.Reflection;
using IAmTwo.Resources;
using OpenTK.Graphics.OpenGL4;
using SM.Base.Drawing;
using SM.Base.PostEffects;
using SM.Base.Textures;
using SM.Base.Windows;
using SM.OGL.Framebuffer;
using SM.Utility;

namespace IAmTwo.Shaders
{
    public class GameRenderPipeline : RenderPipeline
    {
        private BloomEffect _bloom;
        public static TextureTransformation BloomAmountTransform = new TextureTransformation();

        public override void Initialization()
        {
            MainFramebuffer = CreateWindowFramebuffer();
            _bloom = new BloomEffect(0, true)
            {
                Power = 1.05f,
                AmountMap = Resource.RequestTexture(@".\Resources\bloom_amountMap.png"),
                AmountTransform = BloomAmountTransform,
                MinAmount = .1f
            };
            _bloom.Initilize(this);

            DefaultShader = new SimpleShader("basic", AssemblyUtility.ReadAssemblyFile("IAmTwo.Shaders.GLSL.default_frag.glsl"),
                (u, c) =>
                {
                    u["Texture"].SetTexture(c.Material.Texture, u["HasTexture"]);
                    u["Tint"].SetUniform4(c.Material.Tint);
                    u["Scale"].SetUniform1(c.Material.ShaderArguments.ContainsKey("ColorScale") ? (float)c.Material.ShaderArguments["ColorScale"] : 1f);
                });


            base.Initialization();
        }

        protected override void RenderProcess(ref DrawContext context)
        {
            MainFramebuffer.Activate(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            context.Scene.Draw(context);

            Framebuffer.Screen.Activate(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            BloomAmountTransform.Offset.Add(Deltatime.RenderDelta * .1f, 0);
            _bloom.Draw(context);
        }
    }
}