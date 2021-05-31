using System;
using IAmTwo.Game;
using IAmTwo.Shaders;
using OpenTK;
using SM.Base.Animation;
using SM.Base.Types;

namespace IAmTwo.LevelObjects.Objects
{
    public class GameObject : PhysicsObject, IPlaceableObject
    {
        public static CVector1 Glow = new CVector1(1f);

        static GameObject()
        {
            TimeSpan span = TimeSpan.FromSeconds(2.5f);

            InterpolationProcess intProcess = Glow.Interpolate(span, 1.5f, AnimationCurves.Smooth);
            intProcess.End += (timer, context) =>
            {
                InterpolationProcess intProcess2 = Glow.Interpolate(span, 1f, AnimationCurves.Smooth);
                intProcess2.End += (timer1, updateContext) => intProcess.Start();
            };
        }

        public GameObject()
        {
            Name = "Wall";
            
            ChecksGrounded = true;

            Material.CustomShader = ShaderCollection.Shaders["GameObject"].GetShader();

            Transform.Size.Changed += SizeOnChanged;
        }

        private void SizeOnChanged()
        {
            if (Transform.Size.X > Transform.Size.Y)
            {
                TextureTransform.Rotation.Set(0f);
                float aspect = Transform.Size.X / Transform.Size.Y;
                TextureTransform.Scale.Set(aspect, 1);

                ShaderArguments["xTex"] = aspect;
            }
            else
            {
                TextureTransform.Rotation.Set(90f);
                float aspect = Transform.Size.Y / Transform.Size.X;
                TextureTransform.Scale.Set(1, aspect);
                ShaderArguments["xTex"] = aspect;
            }
        }

        public ScaleArgs AllowedScaling { protected set; get; } = ScaleArgs.Default;
        public float AllowedRotationSteps { protected set; get; } = 0;
        public float? TriggerRotation { get; protected set; } = null;
        public string Category { get; protected set; } = "Essential";
        public Vector2 StartSize { get; protected set; } = new Vector2(30, 31);
    }
}