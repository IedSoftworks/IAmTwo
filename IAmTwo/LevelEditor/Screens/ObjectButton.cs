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
    public class ObjectButton : Button
    {
        private static float _scale = .9f;

        private Camera _lastCam;

        private IPlaceableObject _placeObject;
        private bool _allowedClick;
        private DrawObject2D _borderObject;

        public ObjectButton(IPlaceableObject obj) : base("")
        {
            Name = "Object Button";
            _placeObject = obj;

            Objects.Clear();
            Click += ClickEvent;

            float aspect = CalculateAspect(obj.StartSize, out bool xGTRy);
            if (xGTRy)
                obj.Transform.Size.Set(_scale, _scale / aspect, false);
            else
                obj.Transform.Size.Set(_scale / aspect, _scale, false);

            Remove(_border);

            _border = new DrawObject2D {Mesh = Models.QuadricBorder, Transform = {Position = obj.Transform.Position}};
            _border.Transform.Size.Set(1f, 1f);
            _border.Color = Color4.Blue;
            _border.ShaderArguments["ColorScale"] = 1.4f;
            _border.Transform.Position.Set(0, -.1f);

            DrawText title = new DrawText(Fonts.Button, obj.Name);
            title.Transform.Position.Set(-.5f, .6f);
            title.Transform.Size.Set(.0075f);
            
            Add(obj, _border, title);
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

        private void ClickEvent()
        {
            IPlaceableObject obj = (IPlaceableObject)Activator.CreateInstance(_placeObject.GetType());
            obj.Transform.Size.Set(_placeObject.StartSize);

            if (!LevelEditor.CurrentEditor.Add(obj))
            {
                _borderObject.Color = Color4.Red;
            }
        }
    }
}