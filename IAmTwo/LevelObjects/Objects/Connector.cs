using System.Windows.Forms.VisualStyles;
using IAmTwo.Shaders;
using OpenTK;
using OpenTK.Graphics;
using SM.Base.Types;
using SM.Base.Utility;
using SM2D.Drawing;
using SM2D.Scene;

namespace IAmTwo.LevelObjects.Objects
{
    public class Connector : DrawObject2D
    {
        public const float DefaultConnectionWidth = 20;

        protected float Distance;

        public CVector1 ConnectionWidth = new CVector1(DefaultConnectionWidth);
        public IPlaceableObject Start;
        public IPlaceableObject End;

        public Connector(IPlaceableObject start, IPlaceableObject end)
        {
            Start = start;
            End = end;

            Start.Transform.Position.Changed += UpdateConnection;
            End.Transform.Position.Changed += UpdateConnection;

            ConnectionWidth.Changed += UpdateConnection;

            Transform.ZIndex.Set(-5);
            Material.Blending = true;

            UpdateConnection();
        }

        protected void UpdateConnection()
        {
            Vector2 diff = End.Transform.Position - (Vector2)Start.Transform.Position;
            Distance = diff.Length;
            Vector2 norm = diff.Normalized();

            Transform.Size.Set(ConnectionWidth, Distance);
            Transform.Position.Set(Start.Transform.Position + diff * .5f);
            Transform.Rotation.Set(RotationUtility.TurnTowards(Vector2.Zero, norm));

            TextureTransform.Scale.Set(1, Distance / ConnectionWidth);

            ShaderArguments["ConnectorLength"] = (Vector2)Transform.Size;
        }

        public virtual void Disconnect()
        {
            Start.Transform.Position.Changed -= UpdateConnection;
            End.Transform.Position.Changed -= UpdateConnection;
            (Parent as ItemCollection).Remove(this);
        }
    }
}