using System;
using IAmTwo.LevelObjects;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using SM2D.Drawing;

namespace IAmTwo.LevelEditor
{
    public class LevelEditorSelection : DrawObject2D
    {
        public event Action<IPlaceableObject> SelectionChanged;

        public IPlaceableObject SelectedObject;

        public LevelEditorSelection()
        {
            Mesh = Models.QuadricBorder;
            Color = Color4.YellowGreen;
            Active = false;
        }

        public void UpdateSelection(IPlaceableObject obj)
        {
            if (SelectedObject != null)
            {
                SelectedObject.Transform.Position.Changed -= FixPos;
                SelectedObject.Transform.Size.Changed -= FixScale;
                SelectedObject.Transform.Rotation.Changed -= FixRot;
            }


            SelectionChanged?.Invoke(obj);
            if (obj == null)
            {
                Active = false;
                SelectedObject = null;
                return;
            }

            Transform.Position.Set(obj.Transform.Position);
            Transform.Size.Set(Vector2.Add(obj.Transform.Size, new Vector2(10f)));
            Transform.Rotation.Set(obj.Transform.Rotation);

            obj.Transform.Position.Changed += FixPos;
            obj.Transform.Rotation.Changed += FixRot;
            obj.Transform.Size.Changed += FixScale;

            SelectedObject = obj;
            Active = true;
        }

        private void FixPos() => Transform.Position.Set(SelectedObject.Transform.Position);
        private void FixScale() => Transform.Size.Set(Vector2.Add(SelectedObject.Transform.Size, new Vector2(10f)));
        private void FixRot() => Transform.Rotation.Set(SelectedObject.Transform.Rotation);
    }
}