using System;

namespace WebSocket
{
    public static class Packet
    {
        public static byte[] Make(UInt16 type, byte[] data)
        {
            byte[] typeBytes = BitConverter.GetBytes(type);
            Array.Reverse(typeBytes);
            byte[] dataBytes = data;
            byte[] packet = new byte[typeBytes.Length + dataBytes.Length];

            for (int i = 0; i < typeBytes.Length; i++)
            {
                packet[i] = typeBytes[i];
            }

            for (int i = 0; i < dataBytes.Length; i++)
            {
                packet[i + typeBytes.Length] = dataBytes[i];
            }

            return packet;
        }
    }
}
