using System;
using System.Collections.Generic;
using IAmTwo.Game;
using IAmTwo.Shaders;
using OpenTK;
using OpenTK.Graphics;
using SM.Base.Utility;
using SM.Base.Window;
using SM2D.Scene;

namespace IAmTwo.LevelObjects.Objects.SpecialObjects
{
    public class Portal : SpecialObject, IConnectable
    {
        internal PortalConnector _connector;
        internal Portal _counterPart;
        private float _rot;
        public float _ringLoc;
        
        public List<SpecialActor> GotTransported = new List<SpecialActor>();

        public Type ConnectTo { get; } = typeof(Portal);
        public IPlaceableObject ConnectedTo { get; set; }
        public Portal Master;

        public Portal()
        {
            _rot = 0;
            _ringLoc = 1;

            Name = "Portal";

            AllowedScaling = ScaleArgs.NoScaling;
            AllowedRotationSteps = 0;
            StartSize = PlayerSpawner.Size;

            Transform.Size.Set(50);
            TextureTransform.Scale.Set(1);

            ApplyPolygon(PlayerSpawner.Circle);
            Material.CustomShader = ShaderCollection.Shaders["Portal"].GetShader();

            Color = new Color4(1f, 1f,1f,1f);
        }

        protected override void DrawContext(ref DrawContext context)
        {
            _rot += Deltatime.RenderDelta * .1f;
            _ringLoc -= Deltatime.RenderDelta / 2;
            if (_ringLoc < 0) _ringLoc = 1;

            ShaderArguments["Rot"] = _rot;
            ShaderArguments["RingLoc"] = _ringLoc;
            base.DrawContext(ref context);
        }

        public override void BeganCollision(SpecialActor a, Vector2 mtv)
        {
            base.BeganCollision(a, mtv);

            if (GotTransported.Contains(a)) return;

            _connector.ReadyTransport(this, _counterPart, a);
        }

        public override void OnRemoved(object sender)
        {
            base.OnRemoved(sender);
            Disconnect();
        }

        public override void EndCollision(SpecialActor a, Vector2 mtv)
        {
            base.EndCollision(a, mtv);
            if (GotTransported.Contains(a)) GotTransported.Remove(a);
        }


        public void Connect(IPlaceableObject obj)
        {
            if (obj is Portal portal)
            {
                _connector = portal._connector = new PortalConnector(this, portal);
                portal._ringLoc = _ringLoc;

                ConnectedTo = portal;
                portal.ConnectedTo = this;

                ItemCollection p = Parent as ItemCollection;
                p.Add(_connector);
            }
        }
        
        public void Disconnect()
        {
            ConnectedTo = null;

            if (_connector == null) return;
            if (_counterPart == _connector.Entrance)
            {
                _counterPart.Disconnect();
                return;
            }

            _connector.Disconnect();
        }
    }
}