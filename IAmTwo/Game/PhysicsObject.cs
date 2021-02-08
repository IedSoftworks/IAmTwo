using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KWEngine.Hitbox;
using OpenTK;
using SM.Base.Scene;
using SM.Base.Time;
using SM.Base.Windows;
using SM2D.Drawing;

namespace IAmTwo.Game
{
    public class PhysicsObject : DrawObject2D, IScriptable
    {
        public const float Gravity = 9.8f;

        public static List<PhysicsObject> Colliders = new List<PhysicsObject>();

        private Hitbox _hitbox;

        protected Stopwatch AirTime;

        protected bool Passive = false;

        protected bool Grounded = false;

        protected Vector2 Force = Vector2.Zero;

        protected Vector2 Acceleration { get; private set; } = Vector2.Zero;
        protected Vector2 Velocity { get; private set; } = Vector2.Zero;

        public float Mass = 1;
        public float Drag = .75f;

        public PhysicsObject(bool active = false)
        {
            _hitbox = new Hitbox(this);
            Colliders.Add(this);

            AirTime = new Stopwatch();

            Passive = !active;
        }

        public void UpdateHitbox()
        {
            _hitbox.Update(Transform.Position.X, Transform.Position.Y, Transform.Size.X, Transform.Size.Y, Transform.Rotation);
        }

        public virtual void Update(UpdateContext context)
        {
            if (Passive) return;

            if (Grounded) AirTime.Reset();
            else AirTime.Start();
            
            Force.Y -= CalculateGravity();

            Grounded = false;
            foreach (PhysicsObject collider in Colliders)
            {
                if (collider != this)
                {
                    Vector2 mtv;
                    if (Hitbox.TestIntersection(_hitbox, collider._hitbox, out mtv))
                    {
                        
                        Collided(collider, mtv);
                    }

                    if (mtv.Y > 0)
                    {
                        Grounded = true;
                    }
                }
            }

            CalculateForce(context.Deltatime);
        }

        private void CalculateForce(float deltatime)
        {
            Acceleration = Vector2.Divide(Force, Mass) * deltatime;
            Velocity = Vector2.Divide(Acceleration, deltatime);

            Transform.Position.Add(Acceleration);
        }

        public float CalculateGravity()
        {
            return Gravity * Mass;
        }

        public virtual void Collided(PhysicsObject obj, Vector2 mtv) {}

        public Vector2 ElasticCollision(PhysicsObject obj)
        {
            float u1 = Velocity.Length;
            float u2 = obj.Velocity.Length;

            float m1 = Mass;
            float m2 = obj.Mass;

            float v1 = (((m1 - m2) / (m1 + m2)) * u1) + (((2 * m2) / (m1 + m2)) * u2);
            float v2 = (((m2 - m1) / (m2 + m1)) * u2) + (((2 * m1) / (m2 + m1)) * u1);

            Vector2 v1Norm = Velocity.LengthSquared == 0 ? Vector2.Zero : Velocity.Normalized();
            Vector2 v2Norm = obj.Velocity.LengthSquared == 0 ? Vector2.Zero : obj.Velocity.Normalized();

            Vector2 d1 = v2Norm - v1Norm;
            Vector2 d2 = v1Norm - v2Norm;

            Vector2 nv1 = d1 * v1;
            Vector2 nv2 = d2 * v2;

            return nv1;
        }
    }
}