using IAmTwo.Resources;
using OpenTK;

namespace IAmTwo.Game.Objects.SpecialObjects
{
    public class Elevator : SpecialObject
    {
        private bool _reverse;
        private bool _sideways;

        public float Speed = 1.5f;

        public bool Reverse
        {
            get => _reverse;
            set
            {
                _reverse = value;
                ReverseAction();
            }
        }

        public Elevator(bool sideways)
        {
            _sideways = sideways;
            Reverse = false;
            
            Texture = Resource.RequestTexture(@".\Resources\jump_arrow.png");
            _Material.Blending = true;
        }

        private void ReverseAction()
        {
            TextureTransform.Rotation = _reverse ? 0 : 180;
            Color = _reverse ? ColorPallete.Down : ColorPallete.Up;
        }

        public override void ColliedWithPlayer(Player p, Vector2 mtv)
        {
            base.Collided(p, mtv);
            float sped = Gravity * p.Mass * Speed * (_reverse ? -1 : 1);
            if (_sideways) p.Force.X += sped;
            else p.Force.Y += sped;
        }
    }
}