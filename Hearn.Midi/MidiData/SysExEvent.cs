using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public class SysExEvent : MidiEvent
    {

        public SysExEvent(long delta)
           : base(delta, MidiEventTypes.SysEx)
        {
        }

        public byte[] Data { get; set; }

        public override string ToString()
        {
            return $"SysEx Delta {Delta} length {Data?.Length}";
        }

    }
}
