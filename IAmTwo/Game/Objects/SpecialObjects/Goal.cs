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

        public override void BeganCollision(Player p, Vector2 mtv)
        {
            base.BeganCollision(p, mtv);

            bool allowed = (_listenToMirror && p.Mirror) || (!_listenToMirror && !p.Mirror);

            if (allowed)
            {
                _level.CompleteCount++;
                Color = Color4.White;
            }
        }

        public override void EndCollision(Player p, Vector2 mtv)
        {
            base.EndCollision(p, mtv);
            bool allowed = (_listenToMirror && p.Mirror) || (!_listenToMirror && !p.Mirror);

            if (allowed)
            {
                _level.CompleteCount--;
                Color = _oldColor;
            }
        }
    }
}