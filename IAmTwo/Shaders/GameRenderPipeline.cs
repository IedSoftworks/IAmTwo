using System.Drawing;
using System.Dynamic;
using System.Reflection;
using IAmTwo.Resources;
using OpenTK;
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
        private Framebuffer _postBuffer;
        public static TextureTransformation BloomAmountTransform = new TextureTransformation();

        public override void Initialization()
        {
            MainFramebuffer = CreateWindowFramebuffer(0);
            
            //Framebuffers.Add(_postBuffer = CreateWindowFramebuffer(0));
            _bloom = new BloomEffect(true)
            {
                WeightCurvePickAmount = 8,

                Threshold = 1f,
                Power = 1f,
                AmountMap = Resource.RequestTexture(@".\Resources\bloom_amountMap.png"),
                AmountTransform = BloomAmountTransform,
                MinAmount = .1f,
            };
            _bloom.Initilize(this);

            PostProcessFinals.Gamma = 2.2f;

            DefaultShader = ShaderCollection.DefaultShader;


            base.Initialization();
        }

        protected override void RenderProcess(ref DrawContext context)
        {
            MainFramebuffer.Activate(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            context.Scene.Draw(context);

            //PostProcessFinals.ResolveMultisampledBuffers(MainFramebuffer, _postBuffer);
            
            BloomAmountTransform.Offset.Add(Deltatime.RenderDelta * .025f, 0);
            _bloom.Draw(context);

            Framebuffer.Screen.Activate(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            PostProcessFinals.FinalizeHDR(MainFramebuffer.ColorAttachments["color"], .75f);
        }
    }
}