using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Input;
using SM.Base.Drawing.Text;
using SM2D.Drawing;
using SM2D.Scene;
using Button = IAmTwo.Menu.Button;
using Keyboard = SM.Base.Controls.Keyboard;

namespace IAmTwo.LevelEditor
{
    public class PropertySceneControl : ItemCollection
    {
        private Button _testButton;

        public PropertySceneControl()
        {
            DrawText header = new DrawText(Fonts.Button, "Scene Properties");

            _testButton = new Button("Test Level [F2]", -10, 150);
            _testButton.Transform.Position.Set(10, -50);
            _testButton.Click += () => LevelEditor.CurrentEditor.StartTestLevel(false);
            
            ItemCollection levelsize = GenerateLevelSizeControl();
            levelsize.Transform.Position.Set(0, -100);

            Add(header, _testButton, levelsize);
        }

        private DrawText percentageViewer;

        public void ExecuteKeybinds()
        {
            if (Keyboard.IsDown(Key.F2, true))
            {
                _testButton.TriggerClick();
            } 
        }

        private ItemCollection GenerateLevelSizeControl()
        {
            ItemCollection col = new ItemCollection();

            DrawText levelSizeText = new DrawText(Fonts.Text, "Level Size");

            percentageViewer = new DrawText(Fonts.Text, "100%");
            percentageViewer.Transform.Position.Set(100, -35);

            Button decreaseButton = new Button("-", -10, 20);
            decreaseButton.Transform.Position.Set(10, -35);
            decreaseButton.Click += () => UpdateSize(-.1f);

            Button increaseButton = new Button("+", -10, 20);
            increaseButton.Transform.Position.Set(50, -35);
            increaseButton.Click += () => UpdateSize(.1f);

            col.Add(levelSizeText, percentageViewer, decreaseButton, increaseButton);

            return col;
        }

        private void UpdateSize(float change)
        {
            LevelEditor.CurrentEditor.Constructor.SizeMultiplier = MathHelper.Clamp(LevelEditor.CurrentEditor.Constructor.SizeMultiplier + change, .2f, 2);
            LevelEditor.CurrentEditor.UpdateSize();

            percentageViewer.Text = (Math.Round(Math.Floor(LevelEditor.CurrentEditor.Constructor.SizeMultiplier * 100) / 10) * 10) + "%";
        }
    }
}