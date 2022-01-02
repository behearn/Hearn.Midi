using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public abstract class BaseMidiData
    {

        private MidiDataTypes _midiDataTypes;

        public enum MidiDataTypes
        {
            HeaderChunk,
            TrackChunk,
            MetaEvent,
            NoteEvent
        }

        public BaseMidiData(MidiDataTypes midiDataTypes)
        {
            _midiDataTypes = midiDataTypes;
        }

        public MidiDataTypes MidiDataType { get => _midiDataTypes; }

    }
}
