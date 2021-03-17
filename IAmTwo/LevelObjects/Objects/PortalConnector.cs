using System.Collections.Generic;
using IAmTwo.Game;
using IAmTwo.LevelObjects.Objects.SpecialObjects;
using IAmTwo.Shaders;
using OpenTK;
using OpenTK.Graphics;
using SM.Base.Time;
using SM.Base.Utility;
using SM.Base.Window;
using SM2D.Drawing;
using SM2D.Scene;

namespace IAmTwo.LevelObjects.Objects
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

        private float _distance;
        private float _shaderMotion;
        private List<PortalTraveler> _currentTravelers = new List<PortalTraveler>();

        public Portal Entrance;
        public Portal Exit;
        public DrawObject2D Connection;

        public PortalConnector(Portal entrance, Portal exit)
        {
            _shaderMotion = 0;

            Entrance = entrance;
            Exit = exit;

            Entrance._counterPart = Exit;
            Exit._counterPart = Entrance;

            Entrance.Transform.Position.Changed += UpdateConnection;
            Exit.Transform.Position.Changed += UpdateConnection;
            
            Connection = new DrawObject2D {Color = Color4.Green};
            Connection.Transform.ZIndex.Set(-1);
            Connection.SetShader(ShaderCollection.Shaders["PortalConnector"].GetShader());
            Connection.ShaderArguments["Actors"] = _currentTravelers;
            Connection.Material.Blending = true;
            UpdateConnection();

            Add(Connection);

        }

        private void UpdateConnection()
        {
            Vector2 diff = Exit.Transform.Position - (Vector2)Entrance.Transform.Position;
            _distance = diff.Length;
            Vector2 norm = diff.Normalized();

            Connection.Transform.Size.Set(ConnectionWidth, _distance);
            Connection.Transform.Position.Set(Entrance.Transform.Position + diff * .5f);
            Connection.Transform.Rotation.Set(RotationUtility.TurnTowards(Vector2.Zero, norm));

            Connection.ShaderArguments["ConnectorLength"] = (Vector2)Connection.Transform.Size;
        }

        public override void Draw(DrawContext context)
        {
            _shaderMotion += Deltatime.RenderDelta / 10;
            Connection.ShaderArguments["shaderMotion"] = _shaderMotion;

            base.Draw(context);
        }

        public void ReadyTransport(Portal emittingPortal, Portal counterPortal, SpecialActor a)
        {
            a.Transform.Position.Set(counterPortal.Transform.Position);
            counterPortal.GotTransported.Add(a);
            //a.Force += a.Force * 0.5f;
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

        public void Disconnect()
        {
            Entrance._connector = Exit._connector = null;
            Entrance._counterPart = Exit._counterPart = null;

            Entrance.Transform.Position.Changed -= UpdateConnection;
            Exit.Transform.Position.Changed -= UpdateConnection;

            (Parent as ItemCollection).Remove(this);
        }
    }
}