using System;
using System.Collections.Generic;
using IAmTwo.Shaders;
using OpenTK;
using OpenTK.Graphics;

namespace IAmTwo.Game.Objects.SpecialObjects
{
    public class Portal : SpecialObject
    {
        private Dictionary<Player, int> _entries = new Dictionary<Player, int>();
        private PortalConnector _connector;

        internal Portal _counterPart;

        public List<Player> GotTransported = new List<Player>();

        public Portal(PortalConnector connector)
        {
            _connector = connector;
            Transform.Size.Set(50,50);

            ApplyPolygon(PlayerSpawner.Circle);
            SetShader(ShaderCollection.PortalShader);

            Color = Color4.Green;
        }

        public override void BeganCollision(Player p, Vector2 mtv)
        {
            base.BeganCollision(p, mtv);
            if (!_entries.ContainsKey(p))
                _entries.Add(p, (int)Math.Sign(mtv.X));
        }

        public override void ColliedWithPlayer(Player p, Vector2 mtv)
        {
            base.ColliedWithPlayer(p, mtv);


            float distance = Vector2.Distance(Transform.Position, p.Transform.Position);
            p.Color = new Color4(p.Color.R, p.Color.G, p.Color.B, distance / Transform.Size.X);

            if (GotTransported.Contains(p)) return;
            if (Math.Sign(mtv.X) != _entries[p])
            {
                _connector.ReadyTransport(this, _counterPart, p);
            }
        }

        public override void EndCollision(Player p, Vector2 mtv)
        {
            base.EndCollision(p, mtv);
            p.Color = new Color4(p.Color.R, p.Color.G, p.Color.B, 1);
            _entries.Remove(p);
            if (GotTransported.Contains(p)) GotTransported.Remove(p);
        }
    }
}