using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using IAmTwo.Game.Objects.SpecialObjects;
using OpenTK;
using OpenTK.Graphics;
using SM.Base.Scene;
using SM.Base.Windows;
using SM2D.Drawing;
using SM2D.Object;
using SM2D.Scene;

namespace IAmTwo.Game.Objects
{
    public class PortalConnector : ItemCollection
    {

        public const float ConnectionWidth = 10;
        static readonly PolyLine _connectionLine = new PolyLine(new List<Vector2>() {Vector2.Zero, Vector2.UnitY})
        {
            LineWidth = ConnectionWidth
        };
        public Portal Entrance;
        public Portal Exit;

        public PortalConnector(Vector2 entrancePos, Vector2 exitPos)
        {
            Entrance = new Portal(this);
            Entrance.Transform.Position.Set(entrancePos);

            Exit = new Portal(this);
            Exit.Transform.Position.Set(exitPos);

            Entrance._counterPart = Exit;
            Exit._counterPart = Entrance;

            DrawObject2D connection = new DrawObject2D {Color = Color4.Green};
            
            connection.ApplyPolygon(_connectionLine);
            connection.Transform.Size.Y = Vector2.Distance(entrancePos, exitPos);
            connection.Transform.Position.Set(entrancePos);
            connection.Transform.TurnTo(exitPos);
            connection.GetMaterialReference().ShaderArguments["ColorScale"] = 10f;

            Add(connection, Entrance, Exit);
        }

        public void ReadyTransport(Portal emittingPortal, Portal counterPortal, Player p)
        {
            p.Transform.Position.Set(counterPortal.Transform.Position);
            counterPortal.GotTransported.Add(p);
        }
    }
}