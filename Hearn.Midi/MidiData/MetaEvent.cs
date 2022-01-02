using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public abstract class MetaEvent : BaseMidiEvent
    {

        public enum MetaEventTypes
        {
            StringEvent,
            TempoEvent,
            TimeSignatureEvent,
            KeySignatureEvent,
            PortEvent,
            EndTrackEvent
        }

        MetaEventTypes _metaEventType;

        public MetaEvent(long delta, MetaEventTypes metaEventType)
            : base(MidiDataTypes.MetaEvent, delta)
        {
            _metaEventType = metaEventType;
        }

        public MetaEventTypes MetaEventType { get => _metaEventType; }

    }
}
