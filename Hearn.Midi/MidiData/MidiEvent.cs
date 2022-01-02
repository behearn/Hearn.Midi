using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public class MidiEvent : BaseMidiData
    {

        public MidiEvent()
            : base(MidiDataTypes.MidiEvent)
        {
        }

        public long Delta { get; set; }

    }
}
