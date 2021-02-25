using IAmTwo.Game;
using OpenTK;
using OpenTK.Graphics;

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

        public Goal()
        {
            Name = "Goal";

            AllowedScaling = ScaleArgs.NoScaling;
            AllowedRotationSteps = 0;
            Category = "Essential";
            StartSize = new Vector2(50);


            Transform.Size.Set(50);

            Color = ColorPallete.Player;
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

        public override void EndCollision(SpecialActor p, Vector2 mtv)
        {
            base.EndCollision(p, mtv);
            if (!(p is Player)) return;
            Player player = (Player)p;

            bool allowed = (Mirror && player.Mirror) || (!Mirror && !player.Mirror);

            if (allowed)
            {
                Color = _oldColor;
            }
        }

    }
}