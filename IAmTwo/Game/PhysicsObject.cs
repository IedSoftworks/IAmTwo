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
        public static List<Hitbox> Colliders = new List<Hitbox>();

        public static bool Disabled = false;

        private Hitbox _hitbox;

        public Hitbox Hitbox => _hitbox;
        public int ID { get; set; } = -1;

        protected Stopwatch AirTime;
        protected List<PhysicsObject> CollidedWith = new List<PhysicsObject>();
        protected bool Grounded = false;

        protected Vector2 Acceleration { get; private set; } = Vector2.Zero;
        protected Vector2 Velocity { get; private set; } = Vector2.Zero;

        protected bool ChecksGrounded;

        public Vector2 Force = Vector2.Zero;

        public float Mass = 1;
        public float Drag = .75f;

        public bool Passive = true;
        public bool CanCollide { get; set; } = true;

        public PhysicsObject()
        {
            _hitbox = new Hitbox(this);

            AirTime = new Stopwatch();
        }

        public void UpdateHitbox()
        {
            _hitbox.Update(Transform.GetMatrix(), Transform.Rotation);
        }

        public bool UpdateActive { get; set; }

        public virtual void Update(UpdateContext context)
        {
            if (Disabled || Passive) return;

            if (Grounded) AirTime.Reset();
            else AirTime.Start();

            if (!Grounded) Force.Y -= CalculateGravity(context.Deltatime);
            CollidedWith.Clear();
            Grounded = false;
            foreach (Hitbox hitbox in Colliders)
            {
                if (hitbox == _hitbox || !hitbox.PhysicsObject.CanCollide || !hitbox.PhysicsObject.Active) continue;

                Vector2 mtv;
                if (Hitbox.TestIntersection(_hitbox, hitbox, out mtv))
                {
                    CollidedWith.Add(hitbox.PhysicsObject);
                    Collided(hitbox.PhysicsObject, mtv);

                    if (mtv.Y > 0 && hitbox.PhysicsObject.ChecksGrounded)
                    {
                        Grounded = true;
                    }
                }
            }

            CalculateForce(context.Deltatime);
        }

        public override void OnAdded(object sender)
        {
            base.OnAdded(sender);

            UpdateHitbox();
            Colliders.Add(Hitbox);
        }

        public override void OnRemoved(object sender)
        {
            base.OnRemoved(sender);

            Colliders.Remove(Hitbox);
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

        public override string ToString()
        {
            return $"{Name} ({ID})";
        }
    }
}