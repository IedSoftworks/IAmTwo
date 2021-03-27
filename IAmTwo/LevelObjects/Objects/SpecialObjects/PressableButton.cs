using IAmTwo.Game;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using System;

namespace IAmTwo.LevelObjects.Objects.SpecialObjects
{
    public class PressableButton : SpecialObject, IConnectable
    {
        public IButtonTarget ButtonActor;
        public bool Pressed { get; private set; }

        public Type ConnectTo => typeof(IButtonTarget);

        public IPlaceableObject ConnectedTo => (IPlaceableObject)ButtonActor;

        public PressableButton()
        {
            Name = "Button";

            AllowedScaling = ScaleArgs.NoScaling;
            AllowedRotationSteps = 0;

            Color = Color4.Aqua;
            Texture = Resource.RequestTexture(@".\Resources\button_d.png");
            ShaderArguments["EmissionTex"] = Resource.RequestTexture(@".\Resources\button_e.png");
            ShaderArguments["EmissionStrength"] = 2f;
            Material.Blending = true;

            Transform.ApplyTextureSize(Texture, 100);
            Transform.Size.Y *= 0.5f;
            StartSize = Transform.Size;

            TextureTransform.Scale.Set(1, .5f);
        }

        private void SetPressed(bool activate)
        {
            Pressed = activate;

            TextureTransform.Offset.Set(0, activate ? .5f : 0);
        }

        public override void BeganCollision(SpecialActor a, Vector2 mtv)
        {
            base.BeganCollision(a, mtv);

            ButtonActor?.Activation(this, a);
        }

        public override void ColliedWithPlayer(SpecialActor a, Vector2 mtv)
        {
            base.ColliedWithPlayer(a, mtv);

            ButtonActor?.Collision(this, a);
            SetPressed(true);
        }

        public override void EndCollision(SpecialActor a, Vector2 mtv)
        {
            base.EndCollision(a, mtv);

            ButtonActor?.Reset(this, a);
            SetPressed(false);
        }

        public void Connect(IPlaceableObject obj)
        {
            ButtonActor = (IButtonTarget)obj;
        }

        public void Disconnect()
        {
            ButtonActor = null;
        }
    }
}