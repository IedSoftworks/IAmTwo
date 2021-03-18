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
    public abstract class UserOption
    {
        public string Name;
        public bool RequiresPipelineRestart = false;

        public abstract ItemCollection GetVisual();
        public abstract object GetSelectedOption();
    }

    public class SelectUserOption : UserOption
    {
        public string[] Values;

        public override object GetSelectedOption()
        {
            throw new NotImplementedException();
        }

        public override ItemCollection GetVisual()
        {
            return new OptionSelector(Values);
        }
    }
}
