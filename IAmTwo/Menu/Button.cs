using System;
using System.Collections.Generic;
using System.Drawing;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using SM.Base.Drawing;
using SM.Base.Drawing.Text;
using SM.Base.Objects.Static;
using SM.Base.Window;
using SM.OGL.Mesh;
using SM2D.Controls;
using SM2D.Drawing;
using SM2D.Object;
using SM2D.Scene;
using SM2D.Types;
using Font = SM.Base.Drawing.Text.Font;
using Mouse = SM.Base.Controls.Mouse;

namespace IAmTwo.Menu
{
    public class Button : ItemCollection
    {
        static Dictionary<Vector2, Polygon> _polygons = new Dictionary<Vector2, Polygon>();

        private bool _wasOver;
        private Camera camera;

        private DrawingBasis _primaryObj => (EnableBorder ? (DrawingBasis) _border : _text);

        protected DrawObject2D _border;
        protected Polygon _borderMesh;
        protected DrawText _text;

        public event Action Click;
        public bool React = true;
        public float Width;
        public float Height;

        public Action<DrawingBasis> HoverAction = ChangeColor(Color4.LightBlue);
        public Action<DrawingBasis> RecoverAction = ChangeColor(Color4.Blue);

        public bool EnableBorder = true;

        public Color4 Color
        {
            get => _primaryObj.Material.Tint;
            set => _primaryObj.Material.Tint = value;
        }

        public Button(string text, float? width = null, Font font = null, bool allowBorder = true, Vector4? menuRect = null, bool center = false) 
        {
            _text = new DrawText(font ?? Fonts.Button, text);
            if (center) _text.Origin = TextOrigin.Center;
            _text.GenerateMatrixes();
            Width = _text.Width;
            Height = _text.Height;

            float w = width ?? _text.Width;


            EnableBorder = allowBorder;

            Vector2 size = new Vector2(w, _text.Height);
            if (allowBorder)
            {
                if (!_polygons.ContainsKey(size))
                {
                    _polygons.Add(size, Models.CreateBackgroundPolygon(size, 10));
                }

                _borderMesh = _polygons[size];
            }

            _border = new DrawObject2D()
            {
                Mesh = allowBorder ? (GenericMesh)_borderMesh : Plate.Object,
                ForcedMeshType = PrimitiveType.LineLoop,
                ShaderArguments = { ["ColorScale"] = 1.4f },
                RenderActive = allowBorder
            };
            if (center)
            {
                _border.Transform.Position.Set(-size / 2);
            }

            if (allowBorder)
            {
                _border.Transform.Size.Set(1.2f);
            } else _border.Transform.Size.Set(new Vector2(w, _text.Height));

            float x = w / 2 - 10;
            if (center) x = 0;
            _border.Transform.Position.Set(x, -(_text.Height - Fonts.Button.Height) / 2);

            if (menuRect.HasValue)
            {
                _text.Material.ShaderArguments.Add("MenuRect", menuRect.Value);
                _border.Material.ShaderArguments.Add("MenuRect", menuRect.Value);
            }
            
            Add(_text, _border);

            RecoverAction(_primaryObj);
        }

        public override void Update(UpdateContext context)
        {
            base.Update(context);

            DrawingBasis obj = _primaryObj;

            if (React && camera != null && Mouse2D.MouseOver(Mouse2D.InWorld(camera), out _, _border))
            {
                if (!_wasOver) HoverAction(obj);
                if (Mouse.IsDown(MouseButton.Left, true)) TriggerClick();
                _wasOver = true;
                return;
            }

            if (_wasOver) RecoverAction(obj);
            _wasOver = false;
        }

        public override void Draw(DrawContext context)
        {
            camera = context.UseCamera as Camera;

            base.Draw(context);
            if (!EnableBorder) _border.Transform.LastMaster = this.Transform.InWorldSpace;
        }

        public void TriggerClick()
        {
            Click?.Invoke();
        }
        
        public static Action<DrawingBasis> ChangeColor(Color4 color)
        {
            return (a) => a.Material.Tint = color;
        }
    }
}