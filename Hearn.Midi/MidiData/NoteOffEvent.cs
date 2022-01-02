using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public class NoteOffEvent : MidiEvent
    {

        public NoteOffEvent(long delta)
           : base(delta)
        {
        }

        public int Channel { get; set; }

        public MidiConstants.MidiNoteNumbers Note { get; set; }

        public byte Velocity { get; set; }

    }
}
