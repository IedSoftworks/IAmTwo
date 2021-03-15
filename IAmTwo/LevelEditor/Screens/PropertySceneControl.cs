using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using IAmTwo.Menu;
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
        private DrawText percentageViewer;

        public PropertySceneControl()
        {
            DrawText header = new DrawText(Fonts.Button, "Scene Properties");


            ItemCollection levelName = new ItemCollection();
            levelName.Transform.Position.Set(0, -50);
            
            DrawText levelNameText = new DrawText(Fonts.Text, "Level Name");

            TextField levelNameEntry = new TextField(Fonts.Button, width: 175);
            levelNameEntry.Transform.Position.Set(17.5f, -30f);
            levelNameEntry.Changed += () => 
                LevelEditor.CurrentEditor.Constructor.LevelName = levelNameEntry.Text;

            levelName.Add(levelNameText, levelNameEntry);


            ItemCollection levelsize = GenerateLevelSizeControl();
            levelsize.Transform.Position.Set(0, -120);


            Add(header, levelName, levelsize);
        }


        public void ExecuteKeybinds()
        { }

        private ItemCollection GenerateLevelSizeControl()
        {
            ItemCollection col = new ItemCollection();

            DrawText levelSizeText = new DrawText(Fonts.Text, "Level Size");

            percentageViewer = new DrawText(Fonts.Text, "100%");
            percentageViewer.Transform.Position.Set(100, -35);

            Button decreaseButton = new Button("-", 20);
            decreaseButton.Transform.Position.Set(10, -35);
            decreaseButton.Click += () => UpdateSize(-.1f);

            Button increaseButton = new Button("+", 20);
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