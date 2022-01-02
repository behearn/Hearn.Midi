using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public class TimeSignatureEvent : MetaEvent
    {

        public byte TopNumber { get; set; }

        public byte BottomNumber { get; set; }

        public byte MidiClocksPerMetronome { get; set; }

        public byte ThirtySecondNotesPerClock { get; set; }

    }
}
