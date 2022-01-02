using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hearn.Midi.MidiConstants;
using static Hearn.Midi.MidiStreamWriter;

namespace Hearn.Midi.Models
{
    public class MidiNote
    {

        public MidiNoteNumbers Note { get; set; }

        public byte Velocity { get; set; }

        public NoteDurations Duration { get; set; }

    }
}
