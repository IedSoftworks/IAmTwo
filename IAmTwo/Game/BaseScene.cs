using IAmTwo.Shaders;
using SM.Base;
using SM.Base.Scene;
using SM.Base.Time;
using SM.Base.Window;
using SM2D.Scene;

namespace IAmTwo.Game
{
    public class BaseScene : Scene
    {
        protected bool PlayActivationAnimation = true;

        public override void Activate()
        {
            base.Activate();

            if (PlayActivationAnimation) GameRenderPipeline.ExposureUp.Start();
        }

        private GenericScene _changeToScene;
        public void ChangeScene(GenericScene scene)
        {
            _changeToScene = scene;
            GameRenderPipeline.ExposureDown.End += _changeScene;
            GameRenderPipeline.ExposureDown.Start();
        }

        private void _changeScene(Timer timer, UpdateContext context)
        {
            SMRenderer.CurrentWindow.SetScene(_changeToScene);
            GameRenderPipeline.ExposureDown.End -= _changeScene;
        }
    }
}