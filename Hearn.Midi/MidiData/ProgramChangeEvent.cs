using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public class ProgramChangeEvent : MidiEvent
    {

        public int Channel { get; set; }

        public MidiConstants.Instruments Instrument { get; set; }

    }
}
