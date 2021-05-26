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

    public class PortalConnector : Connector
    {
        private float _shaderMotion;
        private List<PortalTraveler> _currentTravelers = new List<PortalTraveler>();

        public Portal Entrance;
        public Portal Exit;

        public PortalConnector(Portal entrance, Portal exit) : base(entrance, exit)
        {
            _shaderMotion = 0;

            Entrance = entrance;
            Exit = exit;

            Entrance._counterPart = Exit;
            Exit._counterPart = Entrance;
            
            ShaderArguments["Actors"] = _currentTravelers;

            Color = Color4.Green;
            Material.CustomShader = ShaderCollection.Shaders["PortalConnector"].GetShader();
        }

        

        protected override void DrawContext(ref DrawContext context)
        {
            _shaderMotion += Deltatime.RenderDelta / 10;
            ShaderArguments["shaderMotion"] = _shaderMotion;
            base.DrawContext(ref context);
        }

        public void ReadyTransport(Portal emittingPortal, Portal counterPortal, SpecialActor a)
        {
            a.Transform.Position.Set(counterPortal.Transform.Position);
            counterPortal.GotTransported.Add(a);
            a.Force += a.Force * 0.5f;
            a.Active = false;
            a.Passive = true;

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
                traveler.CurrentY = timer.ElapsedNormalized * Distance * 2;
            };
            timer.End += (timer1, context) =>
            {
                a.Active = true;
                a.Passive = false;
                _currentTravelers.Remove(traveler);
            };
            timer.Start();
        }

        public override void Disconnect()
        {
            base.Disconnect();

            Entrance._connector = Exit._connector = null;
            Entrance._counterPart = Exit._counterPart = null;
        }
    }
}