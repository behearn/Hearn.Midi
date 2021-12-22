using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi
{
    public static class StreamEx
    {

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
