using IAmTwo.LevelEditor;
using IAmTwo.Menu;
using OpenTK;
using SM2D.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmTwo
{
    public enum OptionType
    {
        String,
        Int,
        Float,
        Bool
    }

    public abstract class UserOption
    {
        private static DropDownUserOption _resolutionOption = new DropDownUserOption
        {
            Name = "Resolution",
            Member = ""
        };

        public static List<UserOption> Options = new List<UserOption>()
        {
            new SelectUserOption()
            {
                Name = "Window Type",
                Values = new string[]
                {
                    "Window", "Borderless Window", "Fullscreen"
                },
                Member = "CurrentWindowMode"
            },
            _resolutionOption,

            new BoolUserOption()
            {
                Name = "VSync",
                Member = "VSync"
            },
            new SeperatorOption(),
            new SelectUserOption()
            {
                Name = "Anti Aliasing",
                Values = new string[]
                {
                    "Off", "2x", "4x", "8x", "16x",
                },
                Member = "AA",
                RequiresPipelineRestart = true
            },
            new SelectUserOption()
            {
                Name = "Bloom",
                Values = new string[]
                {
                    "Off", "Low", "High"
                },
                Member = "Bloom",
                RequiresPipelineRestart = true
            },
            new SelectUserOption()
            {
                Name = "Material Quality",
                Values = new string[]
                {
                    "Low", "High"
                },
                Member = "MaterialQuality"
            },
        };

        static UserOption()
        {
            DisplayDevice display = DisplayDevice.Default;
            float displayRatio = (float)display.Bounds.Width / display.Bounds.Height;

            List<string> resolution = new List<string>();
            foreach (DisplayResolution res in display.AvailableResolutions.Where(a => a.RefreshRate == display.RefreshRate && Math.Abs((float)a.Width / a.Height - displayRatio) < 0.1))
            {
                string resString = $"{res.Width}x{res.Height}";
                if (!resolution.Contains(resString)) resolution.Add(resString);
            }
            _resolutionOption.Values = resolution.ToArray();
        }

        public string Name;
        public bool RequiresPipelineRestart = false;
        public OptionType Type;
        public string Member;

        public abstract ItemCollection GetVisual();
        public abstract object GetSelectedOption();

        public virtual void SetString(string str)
        {

        }
        public virtual void SetInt(int i)
        {

        }
        public virtual void SetFloat(float f)
        {

        }
        public virtual void SetBool(bool b)
        {

        }
    }

    public class SeperatorOption : UserOption
    {
        public SeperatorOption()
        {
            Name = "";
            Member = "";

            Type = OptionType.Bool;
        }

        public override object GetSelectedOption()
        {
            return null;
        }

        public override ItemCollection GetVisual()
        {
            return new ItemCollection();
        }
    }

    public class SelectUserOption : UserOption
    {
        private OptionSelector _selector;

        public string[] Values;

        public SelectUserOption()
        {
            Type = OptionType.String;
        }

        public override object GetSelectedOption()
        {
            return _selector.SelectedText;
        }

        public override ItemCollection GetVisual()
        {
            return _selector = new OptionSelector(Values);
        }

        public override void SetString(string str)
        {
            base.SetString(str);
            _selector.Select(str);
        }
    }


    public class BoolUserOption : UserOption
    {
        private CheckBox _checkBox;

        public BoolUserOption()
        {
            Type = OptionType.Bool;
        }

        public override object GetSelectedOption()
        {
            return _checkBox.Checked;
        }

        public override ItemCollection GetVisual()
        {
            return _checkBox = new CheckBox();
        }

        public override void SetBool(bool b)
        {
            base.SetBool(b);
            _checkBox.SetChecked(b);
        }
    }

    public class DropDownUserOption : UserOption
    {
        public string[] Values;

        public DropDownUserOption()
        {
            Type = OptionType.String;
        }

        public override object GetSelectedOption()
        {
            throw new NotImplementedException();
        }

        public override ItemCollection GetVisual()
        {
            return new DropDown(300, Values);
        }
    }
}
