using IAmTwo.Game;
using IAmTwo.Shaders;
using OpenTK;
using OpenTK.Graphics;
using SM.Base.Utility;
using SM.Base.Window;

namespace IAmTwo.LevelObjects.Objects.SpecialObjects
{
    public class Goal : SpecialObject, IPlayerDependent
    {
        private bool _mirror = false;
        private static int _triggered = 0;

        public bool Mirror
        {
            get => _mirror;
            set
            {
                _mirror = value;
                Color = _mirror ? ColorPallete.Mirror : ColorPallete.Player;
            }
        }

        private Color4 _oldColor;
        private float y;

        public Goal()
        {
            Name = "Goal";

            AllowedScaling = ScaleArgs.NoScaling;
            AllowedRotationSteps = 0;
            Category = "Essential";
            StartSize = new Vector2(75, 200);

            Transform.Size.Set(StartSize);

            Material.Blending = true;
            Material.CustomShader = ShaderCollection.Shaders["Goal"].GetShader();

            Color = ColorPallete.Player;
        }

        protected override void DrawContext(ref DrawContext context)
        {
            y += Deltatime.RenderDelta / 20;
            ShaderArguments["yPos"] = y;

            base.DrawContext(ref context);
        }

        public override void BeganCollision(SpecialActor p, Vector2 mtv)
        {
            base.BeganCollision(p, mtv);
            if (!(p is Player)) return;
            Player player = (Player) p;

            bool allowed = (Mirror && player.Mirror) || (!Mirror && !player.Mirror);

            if (allowed)
            {
                Color = Color4.White;
            }
        }
    }
}