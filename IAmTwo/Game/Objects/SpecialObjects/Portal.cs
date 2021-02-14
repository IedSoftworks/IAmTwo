using System;
using System.Collections.Generic;
using IAmTwo.Resources;
using IAmTwo.Shaders;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using SM.Base.Windows;
using SM.Utility;

namespace IAmTwo.Game.Objects.SpecialObjects
{
    public class Portal : SpecialObject
    {
        private Dictionary<SpecialActor, int> _entries = new Dictionary<SpecialActor, int>();
        private PortalConnector _connector;

        internal Portal _counterPart;
        private Vector2 _shaderMove;


        public List<SpecialActor> GotTransported = new List<SpecialActor>();

        public Portal(PortalConnector connector)
        {
            _connector = connector;
            Transform.Size.Set(50);
            TextureTransform.Scale.Set(1);

            ApplyPolygon(PlayerSpawner.Circle);
            SetShader(ShaderCollection.PortalShader);

            Texture = Resource.RequestTexture(@".\Resources\portal_d.png");
            _ShaderArguments["EmissionTex"] = Resource.RequestTexture(@".\Resources\portal_e.png");
            Color = new Color4(0,1f,0,1f);
        }

        protected override void DrawContext(ref DrawContext context)
        {
            _shaderMove.X += Deltatime.RenderDelta * .1f;
            _ShaderArguments["move"] = (Vector2)_shaderMove;
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


            float distance = Transform.Position.X - a.Transform.Position.X;
            a.Color = new Color4(a.Color.R, a.Color.G, a.Color.B, Math.Abs(distance) / Transform.Size.X);

            if (GotTransported.Contains(a)) return;
            if (Math.Sign(distance) != _entries[a])
            {
                _connector.ReadyTransport(this, _counterPart, a);
            }
        }

        public override void EndCollision(SpecialActor a, Vector2 mtv)
        {
            base.EndCollision(a, mtv);
            a.Color = new Color4(a.Color.R, a.Color.G, a.Color.B, 1);
            _entries.Remove(a);
            if (GotTransported.Contains(a)) GotTransported.Remove(a);
        }
    }
}