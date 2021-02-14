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

        protected List<PhysicsObject> CollidedWith = new List<PhysicsObject>();

        protected bool Grounded = false;

        public Vector2 Force = Vector2.Zero;

        protected Vector2 Acceleration { get; private set; } = Vector2.Zero;
        protected Vector2 Velocity { get; private set; } = Vector2.Zero;

        protected bool ChecksGrounded;

        public float Mass = 1;
        public float Drag = .75f;

        public bool Passive = true;
        public bool CanCollide = true;

        public PhysicsObject()
        {
            _hitbox = new Hitbox(this);
            Colliders.Add(this);

            AirTime = new Stopwatch();
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

            if (!Grounded) Force.Y -= CalculateGravity(context.Deltatime);
            CollidedWith.Clear();
            Grounded = false;
            foreach (PhysicsObject collider in Colliders)
            {
                if (collider == this || !collider.CanCollide || !collider.Active) continue;

                Vector2 mtv;
                if (Hitbox.TestIntersection(_hitbox, collider._hitbox, out mtv))
                {
                    CollidedWith.Add(collider);
                    Collided(collider, mtv);

                    if (mtv.Y > 0 && collider.ChecksGrounded)
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

        public float CalculateGravity(float deltatime)
        {
            return Gravity * Mass;
        }

        public virtual void Collided(PhysicsObject obj, Vector2 mtv) {}

        public void DefaultCollisionResolvement(Vector2 mtv)
        {
            Force += mtv * 75;
            Transform.Position.Add(mtv);
        }
    }
}