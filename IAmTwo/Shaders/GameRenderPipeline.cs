using IAmTwo.Resources;
using OpenTK.Graphics.OpenGL4;
using SM.Base.Drawing;
using SM.Base.PostEffects;
using SM.Base.Utility;
using SM.Base.Window;
using SM.OGL.Framebuffer;
using SM.OGL.Texture;

namespace IAmTwo.Shaders
{
    public class GameRenderPipeline : RenderPipeline
    {
        private BloomEffect _bloom;
        private Framebuffer _postBuffer;
        public static TextureTransformation BloomAmountTransform = new TextureTransformation();

        public override void Initialization()
        {
            MainFramebuffer = CreateWindowFramebuffer( UserSettings.AA != "Off" ? int.Parse(UserSettings.AA.Remove(UserSettings.AA.Length - 1, 1)) : 0, PixelInformation.RGBA_HDR );

            Framebuffers.Add(_postBuffer = CreateWindowFramebuffer(0, PixelInformation.RGBA_HDR, false));

            if (UserSettings.Bloom != "Off")
            {
                _bloom = new BloomEffect(_postBuffer, true, UserSettings.Bloom == "High" ? .75f : .25f)
                {
                    WeightCurvePickAmount = 5,
                    AmountMap = Resource.RequestTexture(@".\Resources\bloom_amountMap.png"),
                    AmountTransform = BloomAmountTransform,
                    MinAmount = .1f,
                };
                _bloom.Initilize(this);
            }

            PostProcessUtility.Gamma = 2.2f;

            DefaultShader = ShaderCollection.Shaders["Default"].GetShader();


            base.Initialization();
        }

        protected override void RenderProcess(ref DrawContext context)
        {
            MainFramebuffer.Activate(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            context.Scene.Draw(context);
            
            _postBuffer.Activate(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            PostProcessUtility.ResolveMultisampledBuffers(MainFramebuffer, _postBuffer);

            if (_bloom != null) { 
                BloomAmountTransform.Offset.Add(Deltatime.RenderDelta * .025f, 0);
                _bloom.Draw(context);
            }

            Framebuffer.Screen.Activate(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            PostProcessUtility.FinalizeHDR(_postBuffer.ColorAttachments["color"], .5f);
        }
    }
}