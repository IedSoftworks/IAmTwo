using IAmTwo.LevelEditor;
using IAmTwo.Menu;
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
                Member = "AA"
            },
            new SelectUserOption()
            {
                Name = "Bloom",
                Values = new string[]
                {
                    "Off", "Low", "High"
                },
                Member = "Bloom"
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
}
