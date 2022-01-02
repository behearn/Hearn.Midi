using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public class BaseMidiEvent : BaseMidiData
    {

        public BaseMidiEvent(MidiDataTypes midiDataTypes, long delta)
            : base(midiDataTypes)
        {
            Delta = delta;
        }

        public long Delta { get; set; }

    }
}

