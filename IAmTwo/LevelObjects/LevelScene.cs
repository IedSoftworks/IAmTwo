using System;
using System.Collections.Generic;
using IAmTwo.Game;
using IAmTwo.LevelObjects.Objects;
using KWEngine.Hitbox;
using OpenTK;
using SM.Base;
using SM.Base.Scene;
using SM.Base.Time;
using SM.Base.Windows;
using SM2D.Scene;

namespace IAmTwo.LevelObjects
{
    public class LevelScene : Scene
    {
        private const float Aspect = 0.5625f;
        protected List<IPlaceableObject> _placedObjects = new List<IPlaceableObject>();

        public LevelConstructor Constructor;
        public GameObject[] Walls;
        protected bool ConstructWorld = true;

        public LevelScene(LevelConstructor constructor)
        {
            Constructor = constructor;
        }

        protected void Spawn()
        {
            foreach (int i in Constructor.Spawner)
            {
                IPlaceableObject spawner = _placedObjects.Find(a => a.ID == i);

                if (spawner is PlayerSpawner pspawner) pspawner.Spawn();
            }
        }

        public void UpdateSize()
        {
            Camera.RequestedWorldScale = new Vector2(0, LevelConstructor.DefaultSize * Constructor.SizeMultiplier);
            Camera.CalculateWorldScale(SMRenderer.CurrentWindow);
            GenerateWalls();
        }

        public override void Initialization()
        {
            base.Initialization();

            Camera = new Camera()
            {
                RequestedWorldScale = new Vector2(0, LevelConstructor.DefaultSize * Constructor.SizeMultiplier)
            };
            Camera.CalculateWorldScale(SMRenderer.CurrentWindow);

            BackgroundCamera = Camera;

            Background = new GameBackground(Camera);


            if (ConstructWorld)
            {
                foreach (ObjectConstructor obj in Constructor.Objects)
                {
                    IPlaceableObject o = (IPlaceableObject) Activator.CreateInstance(obj.ObjectType);
                    o.ID = obj.ID;

                    o.Transform.Position.Set(obj.Position);
                    o.Transform.Size.Set(obj.Size);
                    o.Transform.Rotation.Set(obj.Rotation);

                    if (o is IPlayerDependent pd) pd.Mirror = obj.Mirror;

                    Objects.Add(o);
                    _placedObjects.Add(o);
                }

                foreach (Tuple<int, int> connection in Constructor.Connections)
                {
                    IPlaceableObject origin = _placedObjects.Find(a => 
                        a.ID == connection.Item1);
                    IPlaceableObject target = _placedObjects.Find(a => a.ID == connection.Item2);

                    if (origin != null && target != null && origin is IConnectable connectable)
                    {
                        connectable.Connect(target);
                    }
                }
            }

            GenerateWalls();
        }


        public override void Update(UpdateContext context)
        {
            foreach (Hitbox collider in PhysicsObject.Colliders)
            {
                collider.PhysicsObject.UpdateHitbox();
            }

            base.Update(context);
        }

        public override void Deactivate()
        {
            base.Deactivate();

            PhysicsObject.Colliders.Clear();
        }

        protected void GenerateWalls()
        {
            if (Walls != null) Objects.Remove(Walls);

            Vector2 levelSize = new Vector2(LevelConstructor.DefaultSize * Constructor.SizeMultiplier / Aspect, LevelConstructor.DefaultSize * Constructor.SizeMultiplier);
            float thickness = 20;

            Walls = new GameObject[4];

            for (int i = 0; i < 4; i++)
            {
                bool vert = i % 2 == 0;
                bool reverse = i > 1;

                GameObject wall = new GameObject();
                if (vert)
                {
                    wall.Transform.Size.Set(levelSize.X - thickness * 2, thickness);
                    wall.Transform.Position.Set(0, (levelSize.Y / 2 - thickness / 2) * (reverse ? -1 : 1));
                }
                else
                {

                    wall.Transform.Size.Set(thickness, levelSize.Y - thickness * 2);
                    wall.Transform.Position.Set((levelSize.X / 2 - thickness / 2) * (reverse ? -1 : 1), 0);
                }

                Walls[i] = wall;
                Objects.Add(wall);
            }
        }
    }
}