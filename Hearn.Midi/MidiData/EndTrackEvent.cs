using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public class EndTrackEvent : MetaEvent
    {

        public EndTrackEvent(long delta)
            : base(delta)
        {
        }

        public override string ToString()
        {
            return $"End Track Delta {Delta}";
        }

    }
}
