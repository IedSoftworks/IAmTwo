using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using IAmTwo.LevelObjects.Objects;
using SM.Base;
using SM.Base.Scene;
using SM2D.Scene;

namespace IAmTwo.LevelObjects
{
    [Serializable]
    public class LevelConstructor
    {
        public static float DefaultSize = 650;

        public int NextID = 0;

        public string LevelName = "";
        [NonSerialized] public string LevelPath = "";

        public float SizeMultiplier = 1;

        public List<ObjectConstructor> Objects = new List<ObjectConstructor>();
        public List<Tuple<int, int>> Connections = new List<Tuple<int, int>>();
        public List<int> Spawner;

        public void ApplyItemLists(ItemCollection items)
        {
            Objects = new List<ObjectConstructor>();
            Connections = new List<Tuple<int, int>>();
            Spawner = new List<int>();

            foreach (IShowItem show in items)
            {
                if (show is IPlaceableObject basis)
                {
                    if (basis.ID < 0) continue;

                    ObjectConstructor objConst = new ObjectConstructor()
                    {
                        ObjectType = basis.GetType(),
                        ID = basis.ID,

                        Position = basis.Transform.Position,
                        Rotation = basis.Transform.Rotation,
                        Size = basis.Transform.Size,

                        Mirror = false,

                        ConnectToID = -1,
                    };
                    if (basis is PlayerSpawner spawner) Spawner.Add(spawner.ID);

                    if (basis is IPlayerDependent pd) objConst.Mirror = pd.Mirror;

                    if (basis is IConnectable con && con.ConnectedTo != null)
                    {
                        if (!Connections.Contains(new Tuple<int, int>(basis.ID, con.ConnectedTo.ID)) &&
                            !Connections.Contains(new Tuple<int, int>(con.ConnectedTo.ID, basis.ID)))
                        {
                            Connections.Add(new Tuple<int, int>(basis.ID, con.ConnectedTo.ID));
                        }
                    }

                    Objects.Add(objConst);
                }
            }
        }

        public void Store(Stream stream, ItemCollection storingCol)
        {
            ApplyItemLists(storingCol);
            Store(stream);
        }
        
        public void Store(Stream stream)
        {
            BinaryFormatter bf = new BinaryFormatter();

            try
            {
                bf.Serialize(stream, this);
            }
            catch (SerializationException e)
            {
                Log.Write(LogType.Error, "Failed to serialize level. Reason: "+ e.Message);
            }
        }

        public static LevelConstructor Load(Stream stream)
        {
            BinaryFormatter bf = new BinaryFormatter();

            try
            {
                return (LevelConstructor) bf.Deserialize(stream);
            }
            catch (SerializationException e)
            {
                Log.Write(LogType.Error, "Failed to deserialize level. Reason: " + e.Message);
            }

            return null;
        }
    }
}