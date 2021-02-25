using System;
using System.Collections.Generic;
using System.Reflection;

namespace IAmTwo.LevelObjects
{
    public class PlaceableObjectRegister
    {
        private static List<Type> _placeables;

        public static List<Type> ReadPlaceables()
        {
            if (_placeables != null) return _placeables;

            _placeables = new List<Type>();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.FindInterfaces((a, b) => a.ToString() == b.ToString(), typeof(IPlaceableObject).FullName).Length > 0)
                {
                    _placeables.Add(type);
                }
            }

            return _placeables;
        }
    }
}