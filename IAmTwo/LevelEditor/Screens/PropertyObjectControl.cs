using System;
using System.Collections.Generic;
using System.Linq;
using IAmTwo.Game;
using IAmTwo.LevelObjects;
using IAmTwo.LevelObjects.Objects;
using IAmTwo.Menu;
using IAmTwo.Resources;
using OpenTK.Graphics;
using OpenTK.Input;
using SM.Base.Drawing;
using SM.Base.Scene;
using SM2D.Controls;
using SM2D.Drawing;
using SM2D.Scene;
using SM2D.Types;
using Keyboard = SM.Base.Controls.Keyboard;
using Mouse = SM.Base.Controls.Mouse;

namespace IAmTwo.LevelEditor
{
    public class PropertyObjectControl : ItemCollection
    {
        private Button connectButton;
        private bool _connectMode = false;
        private List<IPlaceableObject> _connectSelectList;
        private IConnectable _connectAsked;

        private IPlaceableObject _object;

        public PropertyObjectControl()
        {
        }

        public void Reload(IPlaceableObject obj)
        {
            this.Clear();
            _object = obj;

            DrawText txt = new DrawText(Fonts.Button, _object.ToString());

            Add(txt);
            float yPos = 50;

            if (_object is IConnectable connectable)
            {
                Tuple<IShowTransformItem<Transformation>, float> d = GenerateConnectable(connectable);
                d.Item1.Transform.Position.Set(0, -yPos);
                Add(d.Item1);

                yPos += d.Item2 + 10f;
            }
        }

        private Tuple<IShowTransformItem<Transformation>, float> GenerateConnectable(IConnectable obj)
        {
            if (obj.ConnectedTo == null)
            {
                ItemCollection nConnected = new ItemCollection();

                DrawText header = new DrawText(Fonts.Text, "Not connected");
                connectButton = new Button("Connect [C]", 100);
                connectButton.Transform.Position.Set(10, -30);
                connectButton.Click += () => EnterConnectMode(obj);
                
                nConnected.Add(header, connectButton);

                return new Tuple<IShowTransformItem<Transformation>, float>(nConnected, 60f);
            }
            
            return new Tuple<IShowTransformItem<Transformation>, float>(new DrawText(Fonts.Text, "Connected to:\n"+obj.ConnectedTo), 40f);
        }

        public void ExecuteKeybinds()
        {
            if (_connectMode)
            {
                if (Mouse.IsDown(MouseButton.Left, true))
                {
                    if (Mouse2D.MouseOver(Mouse2D.InWorld(LevelEditor.CurrentEditor.Camera),
                        out IPlaceableObject clicked,
                        _connectSelectList))
                    {
                        _connectAsked.Connect(clicked);
                        ExitConnectMode();
                    }
                }

                if (Keyboard.IsDown(Key.Escape, true))
                {
                    ExitConnectMode();
                }
            }
            else if (Keyboard.IsDown(Key.C, true))
            {
                if (_object is IConnectable) connectButton.TriggerClick();
            }
        }

        private void EnterConnectMode(IConnectable connectable)
        {
            LevelEditor.CurrentEditor.DisableInput = true;
            LevelEditor.CurrentEditor.Background.Active = false;
            LevelEditor.CurrentEditor.HUD.RenderActive = false;

            _connectMode = true;
            _connectAsked = connectable;
            _connectSelectList = new List<IPlaceableObject>();
            foreach (IShowItem item in LevelEditor.CurrentEditor.Objects)
            {
                if (item is GameObject obj)
                {
                    if (LevelEditor.CurrentEditor.Walls.Contains(obj)) continue;
                }

                if (item == connectable) continue;

                if (item.GetType().IsAssignableFrom(connectable.ConnectTo) && item is IPlaceableObject placeable)
                {
                    _connectSelectList.Add(placeable);
                    continue;
                }
                
                item.Active = false;
            }
        }

        private void ExitConnectMode()
        {
            LevelEditor.CurrentEditor.DisableInput = false;
            LevelEditor.CurrentEditor.Background.Active = true;
            LevelEditor.CurrentEditor.HUD.RenderActive = true;

            _connectMode = false;
            _connectSelectList = null;

            foreach (IShowItem item in LevelEditor.CurrentEditor.Objects)
            {
                item.Active = true;
            }

            Reload(_object);
        }
    }
}