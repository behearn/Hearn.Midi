using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public class KeySignatureEvent : MetaEvent
    {

        public KeySignatureEvent(long delta)
            : base(delta, MetaEventTypes.KeySignatureEvent)
        {
        }

        public byte SharpOrFlats { get; set; }

        public bool IsMinor { get; set; }

        public override string ToString()
        {
            return $"Key Signature Delta {Delta} Sharps(+)/Flats(-) {SharpOrFlats} Is Minor {IsMinor}";
        }

    }
}
