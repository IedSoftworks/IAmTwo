using System;
using System.Collections.Generic;
using IAmTwo.LevelObjects;
using KWEngine.Hitbox;
using OpenTK;
using SM.Base;
using SM.Base.Animation;
using SM.Base.Scene;
using SM.Base.Utility;
using SM.Base.Window.Contexts;
using SM2D.Drawing;

namespace IAmTwo.Game
{
    public class PhysicsObject : BaseGameObject, IFixedScriptable
    {
        public const float Drag = 1200f;
        public const float Gravity = 10f;
        public static bool Disabled = false;


        protected bool Grounded = false;

        protected bool CanCollide = true;
        protected List<PhysicsObject> CollidedWith = new List<PhysicsObject>();
        protected bool ChecksGrounded = false;

        protected Matrix4 HitboxChangeMatrix = Matrix4.Identity;
        protected bool HitboxNoRotation = false;

        public LevelScene Scene { get; set; }

        private Hitbox _hitbox;
        public Hitbox Hitbox => _hitbox;

        public bool Passive = true;

        public Vector2 Force;
        public Vector2 Velocity;

        public float Mass = 1;

        public float MaxXSpeed = 1000;

        public PhysicsObject()
        {
            _hitbox = new Hitbox(this);
        }

        public void UpdateHitbox()
        {
            _hitbox.Update(HitboxChangeMatrix * Transform.GetMatrix(true), 
                Transform.Rotation, HitboxNoRotation);
        }

        public virtual void FixedUpdate(FixedUpdateContext context)
        {
            if (Disabled || Passive) return;

            Vector2 acceleration = Force / Mass;

            Velocity += acceleration;

            int direction = Math.Sign(Velocity.X);
            Velocity.X = Math.Min(Math.Max(Math.Abs(Velocity.X) - (Drag * Deltatime.FixedUpdateDelta), 0), MaxXSpeed) * direction;

            Transform.Position.Add(Velocity * Deltatime.FixedUpdateDelta);

            Force = new Vector2(0, -Gravity);

            CollidedWith.Clear();
            Grounded = false;
            foreach (Hitbox hitbox in Scene.Hitboxes)
            {
                if (hitbox == _hitbox || !hitbox.PhysicsObject.Active || !hitbox.PhysicsObject.CanCollide) continue;

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
        }



        public override void OnAdded(object sender)
        {
            base.OnAdded(sender);

            UpdateHitbox();
            if (Scene == null || !CanCollide) return;
            Scene.Hitboxes.Add(Hitbox);
        }

        public override void OnRemoved(object sender)
        {
            base.OnRemoved(sender);
            
            if (Scene == null || !CanCollide) return;
            Scene.Hitboxes.Remove(Hitbox);
        }

        public virtual void Collided(PhysicsObject obj, Vector2 mtv) { }

        public void DefaultCollisionResolvement(PhysicsObject obj, Vector2 mtv)
        {
            Velocity += mtv * 10;

            Transform.Position.Add(mtv);
        }
    }
}