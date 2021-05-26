using OpenTK;
using SM.Base;
using SM.Base.Window;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using INIPass;

namespace IAmTwo
{
    public class UserSettings
    {
        static Dictionary<string, Dictionary<string, PropertyInfo>> _fieldInfos;
        static UserSettings _settings = new UserSettings();

        static UserSettings()
        {
            Type t = typeof(UserSettings);

            _fieldInfos = new Dictionary<string, Dictionary<string, PropertyInfo>>()
            {
                {
                    "Controls", new Dictionary<string, PropertyInfo>()
                    {
                        {"PlayStationLayout", GetProperty("PlaystationLayout")}
                    }
                },
                {
                    "Graphics", new Dictionary<string, PropertyInfo>()
                    {
                        {"WindowMode", GetProperty("CurrentWindowMode")},
                        {"Resolution", GetProperty("CurrentResolution")},
                        {"VSync", GetProperty("VSync")}, 
                        {"MSAA", GetProperty("AA")},
                        {"Bloom", GetProperty("Bloom")},
                    }
                }
            };
        }

        private static PropertyInfo GetProperty(string name)
        {
            return typeof(UserSettings).GetProperty(name,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        }

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

        public static bool PlaystationLayout
        {
            get => _settings._playstationLayout;
            set => _settings._playstationLayout = value;
        }

        public static void Save()
        {
            INIFile file = new INIFile();

            foreach (KeyValuePair<string, Dictionary<string, PropertyInfo>> info in _fieldInfos)
            {
                INISection section = new INISection();
                file.Add(info.Key, section);

                foreach (KeyValuePair<string, PropertyInfo> pair in info.Value)
                {
                    section.Add(pair.Key, pair.Value.GetValue(null).ToString());
                }
            }

            File.WriteAllText("options.ini", file.Compile());
        }

        public static void Load()
        {
            if (!File.Exists("options.ini")) return;

            INIFile file = INIFile.Load("options.ini");
            foreach (KeyValuePair<string, Dictionary<string, PropertyInfo>> info in _fieldInfos)
            {
                if (!file.ContainsKey(info.Key)) return;

                INISection section = file[info.Key];
                foreach (KeyValuePair<string, PropertyInfo> pair in info.Value)
                {
                    object value = section[pair.Key].FirstString;

                    if (pair.Value.PropertyType == typeof(bool))
                    {
                        value = section[pair.Key].FirstValue.BoolData;
                    }

                    pair.Value.SetValue(null, value);
                }
            }
        }

        private string _windowMode = "Window";
        private string _resolution = $"{DisplayDevice.Default.Width}x{DisplayDevice.Default.Height}";
        private bool _vSync = false;
        private string _aa = "4x";
        private string _bloom = "High";
        private bool _highMaterialQuality = true;
        private bool _playstationLayout = false;

        internal static void ApplySettings(UserSettings settings)
        {
            CurrentWindowMode = settings._windowMode;
            CurrentResolution = settings._resolution;
            VSync = settings._vSync;

            AA = settings._aa;
            Bloom = settings._bloom;
        }
    }
}