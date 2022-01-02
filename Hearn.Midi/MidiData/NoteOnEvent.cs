using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public class NoteOnEvent : MidiEvent
    {

        public NoteOnEvent(long delta)
           : base(delta, MidiEventTypes.NoteOn)
        {
        }

        public int Channel { get; set; }

        public MidiConstants.MidiNoteNumbers Note { get; set; }

        public byte Velocity { get; set; }

        public override string ToString()
        {
            return $"NoteOn Delta {Delta} Channel {Channel + 1} {Note.ToString()} Velocity {Velocity}";
        }

    }
}
