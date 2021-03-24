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
        private Dictionary<SpecialActor, int> _entries = new Dictionary<SpecialActor, int>();

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
            StartSize = new Vector2(50);

            Transform.Size.Set(50);
            TextureTransform.Scale.Set(1);

            ApplyPolygon(PlayerSpawner.Circle);
            Material.CustomShader = ShaderCollection.Shaders["Portal"].GetShader();

            Color = new Color4(0,1f,0,1f);
        }

        protected override void DrawContext(ref DrawContext context)
        {
            _rot += Deltatime.RenderDelta * .1f;
            _ringLoc -= Deltatime.RenderDelta / 2;
            if (_ringLoc < 0) _ringLoc = 1;

            ShaderArguments["move"] = _rot;
            ShaderArguments["ringLoc"] = _ringLoc;
            base.DrawContext(ref context);
        }

        public override void BeganCollision(SpecialActor a, Vector2 mtv)
        {
            base.BeganCollision(a, mtv);
            if (!_entries.ContainsKey(a))
                _entries.Add(a, Math.Sign(Transform.Position.X - a.Transform.Position.X));
        }

        public override void ColliedWithPlayer(SpecialActor a, Vector2 mtv)
        {
            base.ColliedWithPlayer(a, mtv);

            if (_connector == null) return;

            float distance = Vector2.Distance(Transform.Position, a.Transform.Position);
            a.Color = new Color4(a.Color.R, a.Color.G, a.Color.B, Math.Abs(distance) / Transform.Size.X);

            if (GotTransported.Contains(a)) return;
            if (Math.Sign(distance) != _entries[a])
            {
                _connector.ReadyTransport(this, _counterPart, a);
            }
        }

        public override void OnRemoved(object sender)
        {
            base.OnRemoved(sender);
            Disconnect();
        }

        public override void EndCollision(SpecialActor a, Vector2 mtv)
        {
            base.EndCollision(a, mtv);
            a.Color = new Color4(a.Color.R, a.Color.G, a.Color.B, 1);
            _entries.Remove(a);
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

            if (_counterPart == _connector.Entrance)
            {
                _counterPart.Disconnect();
                return;
            }

            _connector.Disconnect();
        }
    }
}