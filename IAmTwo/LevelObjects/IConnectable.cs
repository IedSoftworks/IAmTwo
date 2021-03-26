using System;

namespace IAmTwo.LevelObjects
{
    public interface IConnectable : IPlaceableObject
    {
        Type ConnectTo { get; }
        IPlaceableObject ConnectedTo { get; }

        void Connect(IPlaceableObject obj);
        void Disconnect();
    }
}