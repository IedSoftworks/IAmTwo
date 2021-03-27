using OpenTK;
using SM.Base;
using SM.Base.Window;
using System;
using System.Collections.Generic;

namespace IAmTwo
{
    public class UserSettings
    {
        static UserSettings _settings = new UserSettings();

        public static string CurrentWindowMode {
            get => _settings._windowMode; 
            set {
                _settings._windowMode = value;

                WindowFlags flag = WindowFlags.Window;
                switch (value)
                {
                    case "Window":
                        flag = WindowFlags.Window;
                        break;

                    case "Borderless Window":
                        flag = WindowFlags.BorderlessWindow;
                        break;

                    case "Fullscreen":
                        flag = WindowFlags.ExclusiveFullscreen;
                        break;
                }

                (SMRenderer.CurrentWindow as GLWindow).WindowFlags = flag;
            }
        }
        public static string CurrentResolution
        {
            get => _settings._resolution;
            set
            {
                _settings._resolution = value;

                int width = int.Parse(value.Split('x')[0]);
                int height = int.Parse(value.Split('x')[1]);

                (SMRenderer.CurrentWindow as GLWindow).ChangeFullscreenResolution(DisplayDevice.Default.SelectResolution(width, height, DisplayDevice.Default.BitsPerPixel, DisplayDevice.Default.RefreshRate));
            }
        }

        public static bool VSync 
        {
            get => _settings._vSync;
            set
            {
                if (_settings._vSync != value)
                {
                    _settings._vSync = value;
                    (SMRenderer.CurrentWindow as GLWindow).VSync = value ? OpenTK.VSyncMode.On : OpenTK.VSyncMode.Off;
                }
            }
        }
        public static string AA
        {
            get => _settings._aa;
            set
            {
                if (_settings._aa != value)
                    _settings._aa = value;
            }
        }
        public static string Bloom
        {
            get => _settings._bloom;
            set
            {
                if (_settings._bloom != value)
                    _settings._bloom = value;
            }
        }
        public static bool HighMaterialQuality 
        {
            get => _settings._highMaterialQuality;
            set
            {
                if (_settings._highMaterialQuality != value)
                    _settings._highMaterialQuality = value;
            }
        }

        private string _windowMode = "Window";
        private string _resolution = $"{DisplayDevice.Default.Width}x{DisplayDevice.Default.Height}";
        private bool _vSync = false;
        private string _aa = "4x";
        private string _bloom = "Off";
        private bool _highMaterialQuality = true;

        internal static void ApplySettings(UserSettings settings)
        {
            CurrentWindowMode = settings._windowMode;
            CurrentResolution = settings._resolution;
            VSync = settings._vSync;

            AA = settings._aa;
            Bloom = settings._bloom;
            HighMaterialQuality = settings._highMaterialQuality;
        }
    }
}