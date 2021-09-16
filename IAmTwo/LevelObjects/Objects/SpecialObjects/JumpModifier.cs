using System;
using IAmTwo.Game;
using IAmTwo.Resources;
using OpenTK;
using SM.Base.Utility;
using SM.Base.Window;

namespace IAmTwo.LevelObjects.Objects.SpecialObjects
{
    public class JumpModifier : SpecialObject
    { 
        private float _multiplier = 7.5f;
        
        public float Multiplier
        {
            get => _multiplier * (Math.Abs(Transform.Rotation) < 0 ? 0 : 1);
            set
            {
                _multiplier = value;
            }
        }

        public JumpModifier()
        {
            Name = "Jump Modifier";

            AllowedRotationSteps = 180;
            AllowedScaling = ScaleArgs.Uniform;

            Color = ColorPallete.Up;
            Transform.Rotation.Changed += HandleMultiplierChange;

            Texture = Resource.RequestTexture(@".\Resources\jumparrow_e.png");
            TextureTransform.Rotation.Set(180);
            ShaderArguments["ColorScale"] = 1f;
            Material.Blending = true;
        }

        private void RotationOnChanged()
        {
        }

        private void HandleMultiplierChange()
        {
            bool up = Transform.Rotation > 179;

            Color = up ? ColorPallete.Up : ColorPallete.Down;
        }

        public override void ColliedWithPlayer(SpecialActor a, Vector2 mtv)
        {
            base.ColliedWithPlayer(a, mtv);

            if (a is Player p) 
                p.JumpHeight = Player.DefaultJumpHeight * Multiplier;
        }

        public override void EndCollision(SpecialActor a, Vector2 mtv)
        {
            base.EndCollision(a, mtv);

            if (a is Player p) 
                p.JumpHeight = Player.DefaultJumpHeight;
        }

        protected override void DrawContext(ref DrawContext context)
        {
            TextureTransform.Offset.Add(0, -Deltatime.RenderDelta * .5f);
            base.DrawContext(ref context);
        }
    }
}