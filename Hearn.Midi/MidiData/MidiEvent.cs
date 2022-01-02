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
            ProgramChange,
            NoteOn,
            NoteOff,
            SysEx
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
