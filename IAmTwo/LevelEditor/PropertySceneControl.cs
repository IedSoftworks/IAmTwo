using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using IAmTwo.Resources;
using OpenTK;
using SM.Base.Drawing.Text;
using SM2D.Drawing;
using SM2D.Scene;
using Button = IAmTwo.Menu.Button;

namespace IAmTwo.LevelEditor
{
    public class PropertySceneControl : ItemCollection
    {
        public PropertySceneControl()
        {
            DrawText header = new DrawText(Fonts.Button, "Scene Properties");

            Button testButton = new Button("Test Level (F2)", -10, 100);
            testButton.Transform.Position.Set(10, -50);
            testButton.Click += context => LevelEditor.CurrentEditor.StartTestLevel(false);
            
            ItemCollection levelsize = GenerateLevelSizeControl();
            levelsize.Transform.Position.Set(0, -100);

            Add(header, testButton, levelsize);
        }

        private DrawText percentageViewer;

        private ItemCollection GenerateLevelSizeControl()
        {
            ItemCollection col = new ItemCollection();

            DrawText levelSizeText = new DrawText(Fonts.Text, "Level Size");

            percentageViewer = new DrawText(Fonts.Text, "100%");
            percentageViewer.Transform.Position.Set(100, -35);

            Button decreaseButton = new Button("-", -10, 20);
            decreaseButton.Transform.Position.Set(10, -35);
            decreaseButton.Click += context => UpdateSize(-.1f);

            Button increaseButton = new Button("+", -10, 20);
            increaseButton.Transform.Position.Set(50, -35);
            increaseButton.Click += context => UpdateSize(.1f);

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