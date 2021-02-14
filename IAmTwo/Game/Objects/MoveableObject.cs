using System;
using IAmTwo.Game.Objects.SpecialObjects;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SM.Utility;

namespace IAmTwo.Game.Objects
{
    public class MoveableObject : SpecialActor
    {
        public MoveableObject()
        {
            Mass = 10;
            Passive = false;
            Transform.Size.Set(50);
            Texture = Resource.RequestTexture(@".\Resources\MovingBox_d.png");

            _ShaderArguments["EmissionTex"] = Resource.RequestTexture(@".\Resources\MovingBox_e.png");
            _ShaderArguments["EmissionStrength"] = 2f;

            //ChecksGrounded = true;

            _Material.Blending = true;
        }

        public override void Collided(PhysicsObject obj, Vector2 mtv)
        {
            base.Collided(obj, mtv);

            switch (obj)
            {
                case SpecialObject special:
                    HandleCollision(special, mtv);
                    return;
                case Player p:
                    mtv.Y = 0;
                    break;
            }

            DefaultCollisionResolvement(mtv);

            Force.X *= .9f;
        }
    }
}