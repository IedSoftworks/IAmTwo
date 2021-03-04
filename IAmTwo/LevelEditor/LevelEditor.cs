using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using IAmTwo.Game;
using IAmTwo.LevelObjects;
using IAmTwo.LevelObjects.Objects;
using KWEngine.Hitbox;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using SM.Base;
using SM.Base.Drawing;
using SM.Base.Scene;
using SM.Base.Time;
using SM.Base.Windows;
using SM2D.Controls;
using SM2D.Drawing;
using SM2D.Scene;
using SM2D.Types;
using Keyboard = SM.Base.Controls.Keyboard;
using Mouse = SM.Base.Controls.Mouse;

namespace IAmTwo.LevelEditor
{
    public class LevelEditor : LevelScene
    {
        public static LevelEditor CurrentEditor;
        
        private Dictionary<Type, bool> _playerDependentObjects = new Dictionary<Type, bool>();
        private LevelEditorMenu[] _menus;

        public bool DisableInput = false;

        public Action<IPlaceableObject> MouseAction;

        public LevelEditorSelection EditorSelection;
        public SaveDialog SaveDialog;

        public LevelEditor(LevelConstructor constructor) : base(constructor)
        {
            ConstructWorld = false;
        }

        public override void Initialization()
        {
            base.Initialization();
            CurrentEditor = this;

            // Camera configuration
            BackgroundCamera = Camera;
            Camera.CalculateWorldScale(SMRenderer.CurrentWindow);
            HUDCamera = new Camera()
            {
                RequestedWorldScale = new Vector2(1000,  LevelScene.Aspect * 1000)
            };
            HUDCamera.CalculateWorldScale(SMRenderer.CurrentWindow);

            // Background
            Background = new LevelEditorGrid(Camera);

            // Pre-setted In-World Objects
            EditorSelection = new LevelEditorSelection();

            Objects.Add(EditorSelection);

            // HUD (menus)
            ObjectSelection objectMenu = new ObjectSelection();
            objectMenu.Transform.Size.Set(1);
            objectMenu.Transform.Position.Set(0, -HUDCamera.WorldScale.Y / 2 + ObjectSelection.Height / 2);

            HelpScreen helpScreen = new HelpScreen()
            {
                Active = false
            };

            PropertyControl propertyControl = new PropertyControl(Camera);
            propertyControl.Transform.Position.Set(500 - PropertyControl.Width / 2, 0);

            EscapeControl escControl = new EscapeControl();

            SaveDialog = new SaveDialog();

            _menus = new LevelEditorMenu[]{
                objectMenu, helpScreen, propertyControl, escControl, SaveDialog
            };
            HUD.Add(_menus);
            CloseAllMenus();

            HUDCamera.WorldScaleChanged += camera =>
            {
                objectMenu.Transform.Position.Set(0, -HUDCamera.WorldScale.Y / 2 + ObjectSelection.Height / 2);
            };
        }

        public override void Activate()
        {
            base.Activate();

            CurrentEditor = this;
            PhysicsObject.Disabled = true;
        }

        public override void Deactivate()
        {
            base.Deactivate();

            PhysicsObject.Disabled = false;
        }

        public bool Add(IPlaceableObject obj)
        {
            // Check if object is player dependent
            if (obj is IPlayerDependent p)
            {
                if (_playerDependentObjects.ContainsKey(p.GetType()))
                {
                    if (_playerDependentObjects[p.GetType()]) return false;

                    p.Mirror = true;
                    _playerDependentObjects[p.GetType()] = true;
                }
                else
                {
                    p.Mirror = false;
                    _playerDependentObjects.Add(p.GetType(), false);
                }
            }

            obj.ID = Constructor.NextID++;

            Objects.Add(obj);
            _placedObjects.Add(obj);
            EditorSelection.UpdateSelection(obj);

            return true;
        }

        public void StartTestLevel(bool finialize)
        {
            StoreInConstructor();

            TestLevel lvl = new TestLevel(Constructor);


            SMRenderer.CurrentWindow.SetScene(lvl);
        }

        public void Remove(IPlaceableObject obj)
        {
            Objects.Remove(obj);
            _placedObjects.Remove(obj);
            EditorSelection.UpdateSelection(null);
        }

        public override void Update(UpdateContext context)
        {
            HUD.Update(context);
            
            foreach (LevelEditorMenu menu in _menus)
            {
                menu.Keybinds();

                if (DisableInput) continue;

                if (menu.Input())
                {
                    menu.Active = !menu.Active;
                    if (menu.Active)
                    {
                        CloseAllMenus(menu);
                        menu.Open();
                    } else menu.Close();
                }
            }

            if (DisableInput) return;

            if (EditorSelection.SelectedObject != null)
            {
                // Object Actions
                if (Keyboard.IsDown(Key.Delete))
                {
                    Remove(EditorSelection.SelectedObject);
                }

                // Object Transformations
                if (Keyboard.IsDown(Key.W, true))
                {
                    MouseAction = TransformationActions.MovingAction();
                }

                if (Keyboard.IsDown(Key.S, true))
                {
                    if (EditorSelection.SelectedObject.AllowedScaling != ScaleArgs.NoScaling)
                    {
                        MouseAction = TransformationActions.ScalingAction(EditorSelection.SelectedObject);
                    }
                    else
                    {
                        EditorSelection.Color = Color4.Red;
                        EditorSelection.ShaderArguments["ColorScale"] = 2f;
                        

                        Timer timer = new Timer(1);
                        timer.End += (timer1, updateContext) =>
                        {

                            EditorSelection.ShaderArguments["ColorScale"] = 1f;
                            EditorSelection.Color = Color4.YellowGreen;
                        };
                        timer.Start();
                    }
                }

                if (Keyboard.IsDown(Key.R, true))
                {
                    float rot = (EditorSelection.SelectedObject.Transform.Rotation +
                                 EditorSelection.SelectedObject.AllowedRotationSteps);
                    if (EditorSelection.SelectedObject.TriggerRotation.HasValue)
                    {
                        if (Math.Abs(EditorSelection.SelectedObject.Transform.Rotation -
                                     EditorSelection.SelectedObject.TriggerRotation.Value) < 1)
                        {
                            rot = 0;
                        }
                        else
                        {
                            rot = EditorSelection.SelectedObject.TriggerRotation.Value;
                        }
                    }
                    EditorSelection.SelectedObject.Transform.Rotation.Set(rot % 360);
                }
            }

            // Mouse Actions

            if (MouseAction != null)
            {
                MouseAction?.Invoke(EditorSelection.SelectedObject);
                if (Mouse.IsUp(MouseButton.Left, true)) MouseAction = null;
            }
            else
            {
                Vector2 mousePos = Mouse2D.InWorld(Camera);
                bool overmenu = Mouse2D.MouseOver(Mouse2D.InWorld(HUDCamera), _menus.Where(a => a.RenderActive).Select(a => a.Background).ToArray());
                if (Mouse.IsDown(MouseButton.Left, true) && !overmenu)
                {
                    Mouse2D.MouseOver(mousePos, out IPlaceableObject obj,
                        _placedObjects);

                    EditorSelection.UpdateSelection(obj);
                }
            }
        }

        public void StoreInConstructor()
        {
            Constructor.Objects = new List<ObjectConstructor>();
            Constructor.Connections = new List<Tuple<int, int>>();
            Constructor.Spawner = new List<int>();

            foreach (IShowItem show in Objects)
            {
                if (show is IPlaceableObject basis)
                {

                    ObjectConstructor objConst = new ObjectConstructor()
                    {
                        ObjectType = basis.GetType(),
                        ID = basis.ID,

                        Position = basis.Transform.Position,
                        Rotation = basis.Transform.Rotation,
                        Size = basis.Transform.Size,

                        Mirror = false,

                        ConnectToID = -1,
                    };
                    if (basis is PlayerSpawner spawner) Constructor.Spawner.Add(spawner.ID);

                    if (basis is IPlayerDependent pd) objConst.Mirror = pd.Mirror;

                    if (basis is IConnectable con && con.ConnectedTo != null)
                    {
                        if (!Constructor.Connections.Contains(new Tuple<int, int>(basis.ID, con.ConnectedTo.ID)) &&
                            !Constructor.Connections.Contains(new Tuple<int, int>(con.ConnectedTo.ID, basis.ID)))
                        {
                            Constructor.Connections.Add(new Tuple<int, int>(basis.ID, con.ConnectedTo.ID));
                        }
                    }

                    Constructor.Objects.Add(objConst);
                }
            }
        }

        public void CloseAllMenus(LevelEditorMenu openedMenu = null)
        {
            foreach (LevelEditorMenu menu in _menus)
            {
                if (menu != openedMenu)
                {
                    if (menu.Active) menu.Close();
                    menu.Active = false;
                }
            }
        }
    }
}