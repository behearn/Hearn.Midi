using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public class TimeSignatureEvent : MetaEvent
    {

        public TimeSignatureEvent(long delta)
            : base(delta, MetaEventTypes.TimeSignatureEvent)
        {
        }

        public byte TopNumber { get; set; }

        public byte BottomNumber { get; set; }

        public byte MidiClocksPerMetronome { get; set; }

        public byte ThirtySecondNotesPerClock { get; set; }

        public override string ToString()
        {
            return $"Time Signature Delta {Delta} {TopNumber}:{BottomNumber} (Midi Clocks {MidiClocksPerMetronome} 32nds per quarter note {ThirtySecondNotesPerClock})";
        }

    }
}
