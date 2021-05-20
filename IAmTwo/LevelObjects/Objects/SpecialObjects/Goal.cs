using System;
using IAmTwo.Game;
using IAmTwo.Shaders;
using OpenTK;
using OpenTK.Graphics;
using SM.Base.Utility;
using SM.Base.Window;

namespace IAmTwo.LevelObjects.Objects.SpecialObjects
{
    public class Goal : SpecialObject, IPlayerDependent
    {
        private bool _mirror = false;
        private static int _triggered = 0;

        public bool Mirror
        {
            get => _mirror;
            set
            {
                _mirror = value;
            }
        }

        private float y;

        public Goal()
        {
            Name = "Goal";

            AllowedScaling = ScaleArgs.NoScaling;
            AllowedRotationSteps = 0;
            Category = "Essential";
            StartSize = new Vector2(75, 200);

            Transform.Size.Set(StartSize);

            Material.Blending = true;
            Material.CustomShader = ShaderCollection.Shaders["Goal"].GetShader();
            Material.ShaderArguments.Add("brightness", 1f);

            const float bright = .1f;
            Color = new Color4(bright, bright, bright, 1);
        }

        protected override void DrawContext(ref DrawContext context)
        {
            y += Deltatime.RenderDelta / 20;
            ShaderArguments["yPos"] = y;

            base.DrawContext(ref context);
        }

        public override void BeganCollision(SpecialActor p, Vector2 mtv)
        {
            base.BeganCollision(p, mtv);
            if (!(p is Player)) return;
            Player player = (Player) p;

            bool allowed = (Mirror && player.Mirror) || (!Mirror && !player.Mirror);

            if (allowed)
            {
                Color = Mirror ? ColorPallete.Mirror : ColorPallete.Player;

                player.React = false;
                player.Transform.Size.Interpolate(TimeSpan.FromSeconds(1), Vector2.Zero);

                if (Scene is PlayScene playScene)
                {
                    playScene.AddTarget();
                }
            }
        }
    }
}