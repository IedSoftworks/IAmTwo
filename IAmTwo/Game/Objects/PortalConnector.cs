using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using IAmTwo.Game.Objects.SpecialObjects;
using IAmTwo.Shaders;
using OpenTK;
using OpenTK.Graphics;
using SM.Base.Scene;
using SM.Base.Time;
using SM.Base.Windows;
using SM.Utility;
using SM2D.Drawing;
using SM2D.Object;
using SM2D.Scene;

namespace IAmTwo.Game.Objects
{
    public class PortalTraveler
    {
        public float CurrentY;
        public bool Reverse;
        public Color4 Color;
    }

    public class PortalConnector : ItemCollection
    {

        public const float ConnectionWidth = 20;
        static readonly PolyLine _connectionLine = new PolyLine(new List<Vector2>() {Vector2.Zero, Vector2.UnitY})
        {
            LineWidth = ConnectionWidth
        };

        private float _distance;
        private List<PortalTraveler> _currentTravelers = new List<PortalTraveler>();

        public Portal Entrance;
        public Portal Exit;
        public DrawObject2D Connection;

        public PortalConnector(Vector2 entrancePos, Vector2 exitPos)
        {
            Entrance = new Portal(this);
            Entrance.Transform.Position.Set(entrancePos);

            Exit = new Portal(this);
            Exit.Transform.Position.Set(exitPos);

            Entrance._counterPart = Exit;
            Exit._counterPart = Entrance;

            Vector2 diff = exitPos - entrancePos;
            _distance = diff.Length;
            Vector2 norm = diff.Normalized();
            Connection = new DrawObject2D {Color = Color4.Green};
            Connection.Transform.Size.Set(ConnectionWidth, _distance);
            Connection.Transform.Position.Set( entrancePos + diff * .5f);
            Connection.Transform.Rotation = RotationUtility.TurnTowards(Vector2.Zero, norm);
            Connection.SetShader(ShaderCollection.PortalConnectorShader);

            Connection.GetMaterialReference().ShaderArguments["ConnectorLength"] = (Vector2)Connection.Transform.Size;
            Connection.GetMaterialReference().ShaderArguments["Actors"] = _currentTravelers;
            Connection.GetMaterialReference().Blending = true;

            Add(Connection, Entrance, Exit);
        }

        public void ReadyTransport(Portal emittingPortal, Portal counterPortal, SpecialActor a)
        {
            a.Transform.Position.Set(counterPortal.Transform.Position);
            counterPortal.GotTransported.Add(a);
            a.Force *= 1.5f;
            a.Active = false;
            
            PortalTraveler traveler = new PortalTraveler()
            {
                CurrentY = 0,
                Reverse = emittingPortal != Entrance,
                Color = a.Color
            };
            _currentTravelers.Add(traveler);

            Timer timer = new Timer(2);
            timer.Tick += (s, c) =>
            {
                traveler.CurrentY = timer.ElapsedNormalized * _distance * 2;
            };
            timer.End += (timer1, context) =>
            {
                a.Active = true;
                _currentTravelers.Remove(traveler);
            };
            timer.Start();
        }
    }
}