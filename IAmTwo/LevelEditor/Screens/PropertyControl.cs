using IAmTwo.Game;
using IAmTwo.LevelObjects;
using OpenTK.Graphics;
using OpenTK.Input;
using SM2D.Drawing;
using SM2D.Scene;

namespace IAmTwo.LevelEditor
{
    public class PropertyControl : LevelEditorMenu
    {
        public static float Width = 200f;

        private PropertySceneControl _sceneOptions;
        private PropertyObjectControl _objectOptions;

        private Camera _camera;

        public override DrawObject2D Background { get; set; }
        
        public PropertyControl(Camera cam)
        {
            _camera = cam;

            Background = new DrawObject2D();
            Background.Transform.Size.Set(Width, cam.WorldScale.Y);
            Background.Color = ColorPallete.Background;

            DrawObject2D border = new DrawObject2D()
            {
                Mesh = ObjectSelection.Line,
                Color = new Color4(1f, 1f, 0, 1f),
            };
            border.ShaderArguments["ColorScale"] = 1.1f;

            border.Transform.Position.Set(-Width / 2, -cam.WorldScale.Y / 2);
            border.Transform.Size.Set(cam.WorldScale.Y);
            border.Transform.Rotation.Set(45);

            _sceneOptions = new PropertySceneControl();
            _sceneOptions.Transform.Size.Set(.75f);
            _sceneOptions.Transform.Position.Set(-Width / 2 + 10, cam.WorldScale.Y / 2 - 80);

            _objectOptions = new PropertyObjectControl();
            _objectOptions.Transform.Size.Set(.75f);
            _objectOptions.Transform.Position.Set(-Width / 2 + 10, _camera.WorldScale.Y / 2 - 80);
            Add(_objectOptions);

            _objectOptions.Active = false;

            Add(Background, border, _sceneOptions, _objectOptions);

            LevelEditor.CurrentEditor.EditorSelection.SelectionChanged += UpdateVisual;
        }

        private void UpdateVisual(IPlaceableObject obj)
        {
            if (obj != null)
            {
                _sceneOptions.Active = false;
                _objectOptions.Active = true;
                _objectOptions.Reload(obj);
            }
            else
            {

                _objectOptions.Active = false;
                _sceneOptions.Active = true;
            }

        }

        public override void Keybinds()
        {
            base.Keybinds();

            _objectOptions.ExecuteKeybinds();
            _sceneOptions.ExecuteKeybinds();
        }

        public override bool Input()
        {
            return SM.Base.Controls.Keyboard.IsDown(Key.P, true);
        }
    }
}