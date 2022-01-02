using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public class MidiEvent : BaseMidiEvent
    {

        public MidiEvent(long delta)
            : base(MidiDataTypes.MidiEvent, delta)
        {
        }

    }
}
