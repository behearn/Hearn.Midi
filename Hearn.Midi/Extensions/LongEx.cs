using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.Extensions
{
    public static class LongEx
    {

        public static long ToVariableLengthQuantity(this long value)
        {
            const long MASK_7BIT = 0b_00000000_00000000_00000000_01111111;
            
            var output = value & MASK_7BIT;
            var shiftLeft = 8;

            var workingValue = value >> 7;
            while (workingValue > 0)
            {
                var next7 = workingValue & MASK_7BIT;
                next7 |= 0b_10000000;
                next7 <<= shiftLeft;

                output |= next7;

                workingValue = workingValue >> 7;
                shiftLeft += 8;
            }

            return output;

        }

    }
}
