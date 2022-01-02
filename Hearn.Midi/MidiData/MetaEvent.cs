using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public abstract class MetaEvent : BaseMidiData
    {

        public MetaEvent()
            : base(MidiDataTypes.MetaEvent)
        {
        }
    }
}
