using UnityEngine;
using MLAPI.Serialization;

namespace Forbes.Multiplayer
{
    [System.Serializable]
    public class PacketToClient : INetworkSerializable
    {
        public Vector3 Position;
        public Vector3 V;

        public void NetworkSerialize(NetworkSerializer serializer)
        {
            serializer.Serialize(ref Position);
            serializer.Serialize(ref V);
        }
    }
}