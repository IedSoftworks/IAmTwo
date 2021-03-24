using IAmTwo.Game;
using IAmTwo.LevelObjects.Objects.SpecialObjects;
using IAmTwo.Shaders;
using OpenTK;
using OpenTK.Graphics;
using SM.Base.Utility;
using SM.Base.Window;

namespace IAmTwo.LevelObjects.Objects
{
    public class Door : GameObject, IButtonTarget
    {
        private float _moved = 0;

        public Door()
        {
            AllowedScaling = ScaleArgs.NoScaling;
            TriggerRotation = 90;
            Category = "Special";
            StartSize = new Vector2(20, 120);

            Material.Blending = true;

            Color = new Color4(1, 0, 0, 1f);
            Material.CustomShader = ShaderCollection.Shaders["Door"].GetShader();

            Transform.Size.Changed += () => ShaderArguments["Size"] = (Vector2) Transform.Size;
        }

        public void Activation(PressableButton button, SpecialActor trigger)
        {
        }

        public void Collision(PressableButton button, SpecialActor trigger)
        {
            Color = new Color4(0, 1, 0, 1f);
            CanCollide = false;
        }

        public void Reset(PressableButton button, SpecialActor trigger)
        {
            Color = new Color4(1, 0, 0, 1f);
            CanCollide = true;
        }

        protected override void DrawContext(ref DrawContext context)
        {
            ShaderArguments["Move"] = _moved += Deltatime.RenderDelta;
            base.DrawContext(ref context);
        }
    }
}