using System;
using IAmTwo.Menu;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SM.Base;
using SM.Base.Animation;
using SM.Base.Drawing;
using SM.Base.PostEffects;
using SM.Base.Types;
using SM.Base.Utility;
using SM.Base.Window;
using SM.OGL.Framebuffer;
using SM.OGL.Texture;

namespace IAmTwo.Shaders
{
    public class GameRenderPipeline : RenderPipeline
    {
        public const float BaseExposure = .5f;
        public static CVector1 Exposure = new CVector1(0);

        public static InterpolationProcess ExposureUp;
        public static InterpolationProcess ExposureDown;

        private BloomEffect _bloom;
        private Framebuffer _postBuffer;

        public static TextureTransformation BloomAmountTransform = new TextureTransformation();
        public static TextureBase AmountTex = Resource.RequestTexture(@".\Resources\bloom_amountMap.png");

        static GameRenderPipeline()
        {
            ExposureUp = Exposure.Interpolate(TimeSpan.FromSeconds(.25f), 0, BaseExposure, null, false);
            ExposureDown = Exposure.Interpolate(TimeSpan.FromSeconds(.5f), BaseExposure, 0, null, false);
        }

        public override void Initialization()
        {
            MainFramebuffer = CreateWindowFramebuffer( UserSettings.AA != "Off" ? int.Parse(UserSettings.AA.Remove(UserSettings.AA.Length - 1, 1)) : 0, PixelInformation.RGBA_HDR );

            Framebuffers.Add(_postBuffer = CreateWindowFramebuffer(0, PixelInformation.RGBA_HDR, false));

            if (UserSettings.Bloom != "Off")
            {
                _bloom = new BloomEffect(_postBuffer, true, UserSettings.Bloom == "High" ? 1 : .5f)
                {
                    WeightCurvePickAmount = 8,
                    AmountMap = AmountTex,
                    AmountTransform = BloomAmountTransform,
                    MinAmount = 0,
                    Radius = 1.5f
                };
                _bloom.Initilize(this);
                BloomAmountTransform.Scale.Set(3);
            }

            PostProcessUtility.Gamma = 2.2f;

            DefaultShader = ShaderCollection.Shaders["Default"].GetShader();


            base.Initialization();
        }

        protected override void RenderProcess(ref DrawContext context)
        {
            MainFramebuffer.Activate(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            context.Scene.Draw(context);
            if (Controller.AllowedCursor) MouseCursorVisual.visual.Draw(context);


            _postBuffer.Activate(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            PostProcessUtility.ResolveMultisampledBuffers(MainFramebuffer, _postBuffer);

            BloomAmountTransform.Offset.Add(Deltatime.RenderDelta * Randomize.GetFloat(.075f, .1f), 0);
            _bloom?.Draw(_postBuffer["color"], context);

            Framebuffer.Screen.Activate(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            PostProcessUtility.FinalizeHDR(_postBuffer.ColorAttachments["color"], Exposure.X);
        }
    }
}