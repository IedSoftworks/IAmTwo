using System;
using System.Collections.Generic;

namespace IAmTwo.LevelObjects
{
    public class LevelConstructor
    {
        public static float DefaultSize = 650;

        public int NextID = 0;

        public float SizeMultiplier = 1;

        public List<ObjectConstructor> Objects = new List<ObjectConstructor>();
        public List<Tuple<int, int>> Connections = new List<Tuple<int, int>>();

        public List<int> Spawner;
    }
}