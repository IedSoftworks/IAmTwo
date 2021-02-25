using System;
using OpenTK;

namespace IAmTwo.LevelObjects
{
    public struct ObjectConstructor
    {
        public Type ObjectType;
        public int ID;
        
        public Vector2 Position;
        public Vector2 Size;
        public float Rotation;

        public bool Mirror;

        public int ConnectToID;
    }
}