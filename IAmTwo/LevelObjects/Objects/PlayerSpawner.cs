using IAmTwo.Game;
using IAmTwo.Shaders;
using OpenTK;
using SM.Base.Time;
using SM.Base.Utility;
using SM.Base.Window;
using SM.Utils.Controls;
using SM2D.Object;
using SM2D.Scene;

namespace IAmTwo.LevelObjects.Objects
{
    public class PlayerSpawner : GameObject, IPlayerDependent
    {
        public static GameKeybindActor KeybindActor = GameKeybindActor.CreateKeyboardActor();

        public static Polygon Circle = Polygon.GenerateCircle();



        public bool Mirror
        {
            get => _mirror;
            set
            {
                _mirror = value;
                Color = _mirror ? ColorPallete.Mirror : ColorPallete.Player;
            }
        }

        private bool _mirror = false;
        private Timer _spawnTimer;

        private Player _player;
        private float _rot;
        private float _ringLoc;

        public PlayerSpawner()
        {
            Name = "Player Spawner";

            AllowedRotationSteps = 0;
            AllowedScaling = ScaleArgs.NoScaling;
            StartSize = new Vector2(50);

            ApplyPolygon(Circle);

            _rot = 0;
            _ringLoc = 0;
            Color = Mirror ? ColorPallete.Mirror : ColorPallete.Player;
            Material.CustomShader = ShaderCollection.Shaders["Portal"].GetShader();
            
            Material.Blending = true;
            CanCollide = false;

            _spawnTimer = new Timer(1);
            _spawnTimer.Tick += SpawnAnimation;
            _spawnTimer.End += (timer, context) => _player.React = true;
        }

        private void SpawnAnimation(Stopwatch arg1, UpdateContext arg2)
        {
            _player.Transform.Size.Set(Player.PlayerSize * MathHelper.Clamp(arg1.Elapsed, 0,1));
            _player.Transform.Rotation.Set(360 * MathHelper.Clamp(arg1.Elapsed, 0, 1) % 360);

        }

        protected override void DrawContext(ref DrawContext context)
        {
            _rot += Deltatime.RenderDelta * .1f * (Mirror ? -1 : 1);
            _ringLoc += Deltatime.RenderDelta / 2;
            _ringLoc %= 1.1f;

            ShaderArguments["move"] = _rot;
            ShaderArguments["ringLoc"] = _ringLoc;
            base.DrawContext(ref context);
        }

        public void Spawn()
        {
            _player = new Player(KeybindActor, Mirror);
            _player.Transform.Size.Set(0);
            _player.Transform.Position.Set(Transform.Position);
            (Parent as ItemCollection).Add(_player);

            _spawnTimer.Start();
        }
    }
}