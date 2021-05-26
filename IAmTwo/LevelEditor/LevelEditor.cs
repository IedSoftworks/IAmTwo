using System;
using System.Collections.Generic;
using System.Linq;
using IAmTwo.Game;
using IAmTwo.LevelObjects;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using SM.Base;
using SM.Base.Time;
using SM.Base.Types;
using SM.Base.Window;
using SM2D.Controls;
using SM2D.Drawing;
using SM2D.Scene;
using Keyboard = SM.Base.Controls.Keyboard;
using Mouse = SM.Base.Controls.Mouse;

namespace IAmTwo.LevelEditor
{
    public class LevelEditor : LevelScene
    {
        public static LevelEditor CurrentEditor;
        
        private Dictionary<Type, bool> _playerDependentObjects = new Dictionary<Type, bool>();
        private LevelEditorMenu[] _menus;
        private DrawText _firstHelptext;

        public bool DisableInput = false;

        public Action<IPlaceableObject> MouseAction;

        public LevelEditorSelection EditorSelection;

        public LevelEditor(LevelConstructor constructor) : base(constructor)
        {
            PlayActivationAnimation = false;
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

            _firstHelptext = new DrawText(Fonts.Text, "Press F1 to open the help.");
            _firstHelptext.Transform.Position.Set(-50, (HUDCamera.RequestedWorldScale.Value.Y / 2)- 20);
            HUD.Add(_firstHelptext);

            Timer helpTimer = new Timer(5f);
            helpTimer.End += (timer, context) =>
            {
                if (HUD.Contains(_firstHelptext)) HUD.Remove(_firstHelptext);
            };
            helpTimer.Start();

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
            
            _menus = new LevelEditorMenu[]{
                objectMenu, helpScreen, propertyControl, escControl
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
            Constructor.ApplyItemLists(Objects);

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

            bool overmenu = Mouse2D.MouseOver(Mouse2D.InWorld(HUDCamera), _menus.Where(a => a.Active).Select(a => a.Background).ToArray());
            foreach (LevelEditorMenu menu in _menus)
            {
                if (!overmenu) menu.Keybinds();

                if (DisableInput) continue;

                if (menu.Input())
                {
                    if (HUD.Contains(_firstHelptext)) HUD.Remove(_firstHelptext);

                    menu.Active = !menu.Active;
                    if (menu.Active)
                    {
                        CloseAllMenus(menu);
                        menu.Open();
                    }
                    else menu.Close();
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

                if (Keyboard.AreSpecificKeysPressed(45, 48, out Key[] pressed))
                {
                    CVector2 vector = EditorSelection.SelectedObject.Transform.Position;
                    const float steps = 5;
                    float step = steps * context.Deltatime;

                    foreach(Key key in pressed)
                    {
                        switch(key)
                        {
                            case Key.Up:
                                vector.Add(0, step);
                                break;
                            case Key.Down:
                                vector.Add(0, -step);
                                break;
                            case Key.Left:
                                vector.Add(-step, 0);
                                break;
                            case Key.Right:
                                vector.Add(step, 0);
                                break;
                        }
                    }
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
                
                if (Controller.Actor.Get<bool>("g_click") && !overmenu)
                {
                    Mouse2D.MouseOver(mousePos, out IPlaceableObject obj,
                        _placedObjects);

                    EditorSelection.UpdateSelection(obj);
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