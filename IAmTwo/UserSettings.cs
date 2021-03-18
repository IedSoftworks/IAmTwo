using SM.Base.Window;
using System;
using System.Collections.Generic;

namespace IAmTwo
{
    public class UserSettings
    {
        public static WindowFlags CurrentWindowMode = WindowFlags.ExclusiveFullscreen;

        public static List<UserOption> Options = new List<UserOption>()
        {
            new SelectUserOption()
            {
                Name = "Window Type",
                Values = new string[]
                {
                    "Window", "Borderless Window", "Fullscreen"
                }
            },
            new SelectUserOption()
            {
                Name = "VSync",
                Values = new string[]
                {
                    "Off", "On"
                }
            },
            new SelectUserOption()
            {
                Name = "Anti Aliasing",
                Values = new string[]
                {
                    "Off", "2x", "4x", "8x", "16x",
                }
            },
            new SelectUserOption()
            {
                Name = "Bloom",
                Values = new string[]
                {
                    "Off", "Low", "High"
                }
            },
            new SelectUserOption()
            {
                Name = "Material Complexity",
                Values = new string[]
                {
                    "Low", "High"
                }
            },
        };
    }
}