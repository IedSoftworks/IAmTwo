using System;
using System.Runtime.Remoting;
using IAmTwo.Game;
using IAmTwo.LevelObjects;
using IAmTwo.Menu;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using SM.Base.Objects;
using SM.Base.Windows;
using SM2D.Controls;
using SM2D.Drawing;
using SM2D.Object;
using SM2D.Scene;
using Mouse = SM.Base.Controls.Mouse;

namespace IAmTwo.LevelEditor
{
    public class ObjectButton : ItemCollection
    {
        public static InstancedMesh Border;


        static ObjectButton()
        {
            Border = new InstancedMesh(PrimitiveType.Lines, new string[0]);
            Border.Vertex.Add(new[]
            {
                new Vector3(-.4f, .5f, 0f),
                new Vector3(.4f, .5f, 0f),
                new Vector3(.5f, .4f, 0f),
                new Vector3(.5f, -.4f, 0f),
                new Vector3(.4f, -.5f, 0f),
                new Vector3(-.4f, -.5f, 0f),
                new Vector3(-.5f, -.4f, 0f),
                new Vector3(-.5f, .4f, 0f),
            });
            Border.LineWidth = 2;
        }
        private static float _scale = .9f;

        private IPlaceableObject _placeObject;
        private bool _allowedClick;
        private DrawObject2D _borderObject;

        public ObjectButton(IPlaceableObject obj)
        {
            _placeObject = obj;

            float aspect = CalculateAspect(obj.StartSize, out bool xGTRy);
            if (xGTRy)
                obj.Transform.Size.Set(_scale, _scale / aspect, false);
            else
                obj.Transform.Size.Set(_scale / aspect, _scale, false);

            _borderObject = new DrawObject2D {Mesh = Border, Transform = {Position = obj.Transform.Position}};
            _borderObject.Transform.Size.Set(1f);
            _borderObject.Color = Color4.Blue;
            _borderObject.ShaderArguments["ColorScale"] = 1.4f;
            _borderObject.Transform.Position.Set(0, -.1f);

            DrawText title = new DrawText(Fonts.Button, obj.Name);
            title.Transform.Position.Set(-.5f, .6f);
            title.Transform.Size.Set(.005f);
            
            Add(obj, _borderObject, title);
            PhysicsObject.Colliders.Remove(obj.Hitbox);
        }

        public static float CalculateAspect(Vector2 size, out bool xGTRy)
        {
            if (size.X > size.Y)
            {
                xGTRy = true;
                return size.X / size.Y;
            }
            else
            {
                xGTRy = false;
                return size.Y / size.X;
            }
        }

        public override void Update(UpdateContext context)
        {
            if (_allowedClick)
            {
                _borderObject.Color = Color4.LightBlue;

                if (Mouse.IsDown(MouseButton.Left, true))
                {
                    IPlaceableObject obj = (IPlaceableObject)Activator.CreateInstance(_placeObject.GetType());
                    obj.Transform.Size.Set(_placeObject.StartSize);

                    if (!LevelEditor.CurrentEditor.Add(obj))
                    {
                        _borderObject.Color = Color4.Red;
                    }
                }
            } 
            else _borderObject.Color = Color4.Blue;
        }

        public override void Draw(DrawContext context)
        {
            Vector2 mousePos = Mouse2D.InWorld(context.UseCamera as Camera);

            _allowedClick = Mouse2D.MouseOver(mousePos, out _, _borderObject);

            base.Draw(context);
        }
    }
}