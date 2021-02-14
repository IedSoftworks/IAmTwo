using System.Collections.Generic;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace IAmTwo.Game.Objects.SpecialObjects
{
    public class PressableButton : SpecialObject
    {
        public IButtonTarget ButtonActor;
        public bool Pressed { get; private set; }


        public PressableButton()
        {
            Color = Color4.Aqua;
            Texture = Resource.RequestTexture(@".\Resources\button_d.png");
            _ShaderArguments["EmissionTex"] = Resource.RequestTexture(@".\Resources\button_e.png");
            _ShaderArguments["EmissionStrength"] = 2f;
            _Material.Blending = true;

            Transform.ApplyTextureSize(Texture, 100);
            Transform.Size.Y *= 0.5f;

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
            
            ButtonActor.Activation(this, a);
        }

        public override void ColliedWithPlayer(SpecialActor a, Vector2 mtv)
        {
            base.ColliedWithPlayer(a, mtv);

            ButtonActor.Collision(this, a);
            SetPressed(true);
        }

        public override void EndCollision(SpecialActor a, Vector2 mtv)
        {
            base.EndCollision(a, mtv);

            ButtonActor.Reset(this, a);
            SetPressed(false);
        }
    }
}