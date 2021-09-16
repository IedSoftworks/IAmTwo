using System;
using IAmTwo.LevelObjects;
using OpenTK;
using OpenTK.Input;
using SM2D.Controls;
using Keyboard = SM.Base.Controls.Keyboard;

namespace IAmTwo.LevelEditor
{
    public class TransformationActions
    {
        public static Action<IPlaceableObject> MovingAction(bool offset = true)
        {
            Vector2 setoff = Vector2.Zero;

            if (offset)
                setoff = LevelEditor.CurrentEditor.EditorSelection.SelectedObject.Transform.Position -
                         Mouse2D.InWorld(LevelEditor.CurrentEditor.Camera);

            return a =>
            {
                Vector2 mousePos = Mouse2D.InWorld(LevelEditor.CurrentEditor.Camera) + setoff;

                Vector2 restriction = Vector2.One;
                if (Keyboard.IsDown(Key.X)) restriction = Vector2.UnitX;
                if (Keyboard.IsDown(Key.Y)) restriction = Vector2.UnitY;

                mousePos *= restriction;

                Vector2 newPos = new Vector2((float)Math.Floor(mousePos.X / LevelEditorGrid.GridSize), (float)Math.Floor(mousePos.Y / LevelEditorGrid.GridSize)) * LevelEditorGrid.GridSize;
                a.Transform.Position.Set(newPos);
            };
        }

        public static Action<IPlaceableObject> ScalingAction(IPlaceableObject obj)
        {
            Vector2 oldSize = obj.Transform.Size;

            float aspect = ObjectButton.CalculateAspect(oldSize, out bool xGTRy);

            return a =>
            {
                Vector2 mousePos = Mouse2D.InWorld(LevelEditor.CurrentEditor.Camera);
                Vector2 distance = a.Transform.Position - mousePos;
                distance = Vector2.Transform(distance, a.Transform.GetMatrix(true).ExtractRotation());

                Vector2 newSize = Vector2.Zero;
                if ((a.AllowedScaling & ScaleArgs.Uniform) != 0)
                {
                    if (xGTRy) newSize = new Vector2(distance.Length, distance.Length / aspect);
                    else newSize = new Vector2(distance.Length / aspect, distance.Length);
                }
                else
                {
                    Vector2 restriction = new Vector2(a.AllowedScaling.HasFlag(ScaleArgs.X) ? 1f : 0, a.AllowedScaling.HasFlag(ScaleArgs.Y) ? 1f : 0);

                    bool pressedX = Keyboard.IsDown(Key.X);
                    bool pressedY = Keyboard.IsDown(Key.Y);
                    if (pressedX) restriction *= new Vector2(1,0);
                    if (pressedY) restriction *= new Vector2(0,1);

                    newSize = distance * restriction;

                    if (pressedX) newSize += new Vector2(0, oldSize.Y);
                    if (pressedY) newSize += new Vector2(oldSize.X, 0);
                }

                newSize = new Vector2((float)Math.Abs(Math.Floor(newSize.X / LevelEditorGrid.GridSize)),
                    (float)Math.Abs(Math.Floor(newSize.Y / LevelEditorGrid.GridSize))) * LevelEditorGrid.GridSize;

                if (newSize.X <= 0) newSize.X += LevelEditorGrid.GridSize; 
                if (newSize.Y <= 0) newSize.Y += LevelEditorGrid.GridSize; 
                    
                a.Transform.Size.Set(newSize * 2);
            };
        }
    }
}