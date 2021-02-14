using OpenTK;
using OpenTK.Graphics;

namespace IAmTwo.Game.Objects.SpecialObjects
{
    public class Goal : SpecialObject
    {
        private static int _triggered = 0;

        private bool _listenToMirror;
        private Color4 _oldColor;
        private Level _level;

        public Goal(Level lvl, bool listenToMirror)
        {
            _listenToMirror = listenToMirror;
            _level = lvl;

            Transform.Size.Set(50);

            Color = _oldColor = listenToMirror ? ColorPallete.Mirror : ColorPallete.Player;
        }

        public override void BeganCollision(SpecialActor p, Vector2 mtv)
        {
            base.BeganCollision(p, mtv);
            if (!(p is Player)) return;
            Player player = (Player) p;

            bool allowed = (_listenToMirror && player.Mirror) || (!_listenToMirror && !player.Mirror);

            if (allowed)
            {
                _level.CompleteCount++;
                Color = Color4.White;
            }
        }

        public override void EndCollision(SpecialActor p, Vector2 mtv)
        {
            base.EndCollision(p, mtv);
            if (!(p is Player)) return;
            Player player = (Player)p;

            bool allowed = (_listenToMirror && player.Mirror) || (!_listenToMirror && !player.Mirror);

            if (allowed)
            {
                _level.CompleteCount--;
                Color = _oldColor;
            }
        }
    }
}