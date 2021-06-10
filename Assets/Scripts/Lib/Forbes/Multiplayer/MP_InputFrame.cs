using MLAPI.Serialization;
partial class MP_InputFrame : InputFrame, INetworkSerializable
{
    public void NetworkSerialize(NetworkSerializer serializer)
    {
        serializer.Serialize(ref Horizontal);
        serializer.Serialize(ref Vertical);
        serializer.Serialize(ref Jump);
        serializer.Serialize(ref Fire);
        serializer.Serialize(ref Rotation);
    }
}