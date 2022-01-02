using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.Extensions
{
    public static class StreamEx
    {

        public static int ReadInt(this Stream stream)
        {
            var bytes = new byte[2];
            stream.Read(bytes, 0, 2);
            var value = bytes[0] << 8 | bytes[1];
            return value;
        }

        public static long ReadLong(this Stream stream)
        {
            var bytes = new byte[4];
            stream.Read(bytes, 0, 4);
            var value = bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3];
            return value;
        }

        public static long ReadVariableLengthQuantity(this Stream stream)
        {
            long value;

            var KEEP_READING_MASK = 0b_10000000;
            var VALUE_MASK = 0b_01111111;

            var nextByte = (byte)stream.ReadByte();

            value = nextByte & VALUE_MASK;

            while ((nextByte & KEEP_READING_MASK) == KEEP_READING_MASK)
            {
                nextByte = (byte)stream.ReadByte();

                var nextValue = (byte)(nextByte & VALUE_MASK);

                value <<= 8;
                value |= nextValue;
            }

            return value;
        }


        public static long Read24bitInt(this Stream stream)
        {
            var bytes = new byte[3];
            stream.Read(bytes, 0, 3);
            var value = bytes[0] << 16 | bytes[1] << 8 | bytes[2];
            return value;
        }

        public static void WriteInt(this Stream stream, int value)
        {

            var bytes = new byte[2]
            {
                (byte)((value & 0b_11111111_00000000) >> 8),
                (byte)(value & 0b_00000000_11111111),
            };

            stream.Write(bytes, 0, 2);

        }

        public static void WriteLong(this Stream stream, long value)
        {

            var bytes = new byte[4]
            {
                (byte)((value & 0b_11111111_00000000_00000000_00000000) >> 24),
                (byte)((value & 0b_00000000_11111111_00000000_00000000) >> 16),
                (byte)((value & 0b_00000000_00000000_11111111_00000000) >> 8),
                (byte)(value & 0b_00000000_00000000_00000000_11111111),
            };

            stream.Write(bytes, 0, 4);

        }

        public static void Write24bitInt(this Stream stream, long value)
        {

            var bytes = new byte[3]
            {
                (byte)((value & 0b_00000000_11111111_00000000_00000000) >> 16),
                (byte)((value & 0b_00000000_00000000_11111111_00000000) >> 8),
                (byte)(value & 0b_00000000_00000000_00000000_11111111),
            };

            stream.Write(bytes, 0, 3);

        }

        public static void WriteVariableLengthQuantity(this Stream stream, long value)
        {

            if (value <= 0b_00000000_00000000_00000000_01111111) //1x groups of 7 bits
            {
                stream.WriteByte((byte)value.ToVariableLengthQuantity());
            }
            else if (value <= 0b_00000000_00000000_00111111_11111111) //2x groups of 7 bits
            {
                stream.WriteInt((int)value.ToVariableLengthQuantity());
            }
            else if (value <= 0b_00000000_00011111_11111111_11111111) //3x groups of 7 bits
            {
                stream.Write24bitInt(value.ToVariableLengthQuantity());
            }
            else if (value <= 0b_00001111_11111111_11111111_11111111) //4x groups of 7 bits
            {
                stream.WriteLong(value.ToVariableLengthQuantity());
            }
            else
            {
                throw new InvalidCastException("Invalid value for VariableLengthQuantity");
            }

        }



    }
}
