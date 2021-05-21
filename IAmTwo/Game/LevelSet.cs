using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using IAmTwo.LevelObjects;
using INIPass;

namespace IAmTwo.Game
{
    public class LevelSet
    {
        public static Dictionary<string, List<LevelSet>> LevelSets;

        public int Position;
        public string Name;
        public List<LevelConstructor> Levels = new List<LevelConstructor>();
        public string Credits;

        public static void Load()
        {
            LevelSets = new Dictionary<string, List<LevelSet>>();

            foreach (string directory in Directory.EnumerateDirectories("Levels"))
            {
                string packPath = Path.Combine(directory, "pack.ini");
                string creditPath = Path.Combine(directory, "credits.txt");

                if (!File.Exists(packPath)) continue;
                INIFile data = INIFile.Load(packPath);

                INISection general = data["General"];
                INISection levels = data["Levels"];

                string category = "Others";

                LevelSet levelSet = new LevelSet()
                {
                    Name = general["Name"],
                    Position = 0,
                    Credits = File.Exists(creditPath) ? File.ReadAllText(creditPath) : null
                };

                if (general.ContainsKey("Category"))
                {
                    string[] splits = general["Category"].FirstString.Split('~');
                    category = splits[0];

                    if (int.TryParse(splits[1], out int result))
                    {
                        levelSet.Position = result;
                    }
                }

                int index = 0;
                foreach (INIValue value in levels["L"])
                {
                    string levelPath = Path.Combine(directory, value.Data + ".iatl");

                    if (!File.Exists(levelPath)) continue;

                    using (FileStream stream = new FileStream(levelPath, FileMode.Open, FileAccess.Read))
                    {
                        levelSet.Levels.Add(LevelConstructor.Load(stream, levelSet, index));
                    }

                    index++;
                }

                if (!LevelSets.ContainsKey(category)) LevelSets.Add(category, new List<LevelSet>());
                LevelSets[category].Add(levelSet);
            }

            LevelSets = LevelSets.OrderBy(a => a.Key).ToDictionary(a => a.Key, b => b.Value);

            foreach (KeyValuePair<string, List<LevelSet>> set in LevelSets)
            {
                set.Value.Sort((a, b) => a.Position - b.Position);
            }
        }
    }
}