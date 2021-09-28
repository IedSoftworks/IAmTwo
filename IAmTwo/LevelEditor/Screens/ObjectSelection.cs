using System;
using System.Collections.Generic;
using IAmTwo.Game;
using IAmTwo.LevelObjects;
using IAmTwo.Menu;
using IAmTwo.Resources;
using OpenTK.Graphics;
using OpenTK.Input;
using SM2D.Drawing;
using SM2D.Scene;
using Mouse = SM.Base.Controls.Mouse;

namespace IAmTwo.LevelEditor
{
    public class ObjectSelection : LevelEditorMenu
    {
        public static float Height = 100;

        private ItemCollection _buttons;
        private ItemCollection _items;

        public override DrawObject2D Background { get; set; }
        
        public ObjectSelection()
        {
            Background = new DrawObject2D();

            Background.Transform.Size.Set(1000, Height);
            Background.Color = ColorPallete.Background;

            DrawObject2D lineDraw = new DrawObject2D();
            lineDraw.ApplyPolygon(Models.Border);
            lineDraw.Transform.Rotation.Set(-90);
            lineDraw.Transform.Position.Set(-500, Height / 2);
            lineDraw.Transform.Size.Set(1000);
            lineDraw.Color = Color4.Blue;
            lineDraw.ShaderArguments["ColorScale"] = 15f;

            ItemCollection collection = CreateLists();
            collection.Transform.Size.Set(1);

            Add(Background, lineDraw, collection);
        }

        private ItemCollection CreateLists()
        {
            ItemCollection col = new ItemCollection();

            Dictionary<string, Tuple<List<IPlaceableObject>, ItemCollection>> sorted = new Dictionary<string, Tuple<List<IPlaceableObject>, ItemCollection>>();
            foreach (Type placeable in PlaceableObjectRegister.ReadPlaceables())
            {
                if (placeable.IsAbstract) continue;
                IPlaceableObject obj = (IPlaceableObject)Activator.CreateInstance(placeable);

                if (!sorted.ContainsKey(obj.Category))
                {
                    sorted.Add(obj.Category, new Tuple<List<IPlaceableObject>, ItemCollection>(new List<IPlaceableObject>(), new ItemCollection()));
                }
                sorted[obj.Category].Item1.Add(obj);
            }

            _buttons = new ItemCollection();
            _items = new ItemCollection();

            int pi = 0;
            float itemSize = 60;
            foreach (KeyValuePair<string, Tuple<List<IPlaceableObject>, ItemCollection>> pair in sorted)
            {
                Button b = new Button(pair.Key, 110);
                b.Transform.Size.Set(.75f);
                b.Transform.Position.Set(0, Fonts.Button.Height * 1.25f * pi);
                b.Click += () =>
                {
                    _items.ForEach(a => a.Active = false);
                    pair.Value.Item2.Active = true;
                };

                int itemI = 0;
                foreach (IPlaceableObject o in pair.Value.Item1)
                {
                    o.Transform.Size.Set(o.StartSize);

                    ObjectButton objButton = new ObjectButton(o);
                    objButton.Transform.Size.Set(itemSize);
                    objButton.Transform.Position.Set(itemI * (itemSize + 20), 0);
                    pair.Value.Item2.Add(objButton);
                    itemI++;
                }

                _buttons.Add(b);
                pair.Value.Item2.Active = false;
                _items.Add(pair.Value.Item2);
                pi++;
            }

            _buttons.Transform.Position.Set(-480, 0);
            _buttons.Transform.Size.Set(1);
            col.Add(_buttons);

            _items.Transform.Position.Set(-300, 0);
            _items.Transform.Size.Set(1f);
            col.Add(_items);

            return col;
        }


        public override bool Input()
        {
            return Mouse.IsDown(MouseButton.Right, true);
        }
    }
}