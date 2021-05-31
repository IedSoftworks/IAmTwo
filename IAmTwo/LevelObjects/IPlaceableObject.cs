using KWEngine.Hitbox;
using OpenTK;
using SM.Base.Scene;
using SM2D.Types;
using System;

namespace IAmTwo.LevelObjects
{
    public interface IPlaceableObject : IShowItem, ITransformItem<Transformation>, IModelItem
    {
        int ID { get; set; }
        LevelScene Scene { get; set; }

        Hitbox Hitbox { get; }

        ScaleArgs AllowedScaling { get; }
        float AllowedRotationSteps { get; }
        float? TriggerRotation { get; }
        
        string Category { get; }
        string Name { get; }

        Vector2 StartSize { get; }
    }

    [Flags]
    public enum ScaleArgs
    {
        NoScaling = 0,
        Uniform = 1,
        X = 2,
        Y = 4,

        Default =  X | Y

    }
}