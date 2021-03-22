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
                if (_settings._windowMode != value)
                {
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

                    (SMRenderer.CurrentWindow as GLWindow).ChangeWindowFlag(flag);
                }
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
        public static string MaterialQuality 
        {
            get => _settings._highMaterialQuality ? "High" : "Low";
            set
            {
                bool high = value == "High";
                if (_settings._highMaterialQuality != high)
                    _settings._highMaterialQuality = high;
            }
        }

        private string _windowMode = "Window";
        private bool _vSync = false;
        private string _aa = "4x";
        private string _bloom = "Off";
        private bool _highMaterialQuality = false;
    }
}