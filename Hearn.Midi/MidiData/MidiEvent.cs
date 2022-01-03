using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public class MidiEvent : BaseMidiEvent
    {

        public enum MidiEventTypes
        {
            NoteOff = 0x80,
            NoteOn = 0x90,
            Controller = 0xB0,
            ProgramChange = 0xC0,
            SysEx = 0xF0
        }

        private MidiEventTypes _midiEventType;

        public MidiEvent(long delta, MidiEventTypes midiEventType)
            : base(MidiDataTypes.MidiEvent, delta)
        {
            _midiEventType = midiEventType;
        }

        public MidiEventTypes MidiEventType { get => _midiEventType; }

    }
}
