using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public class PortEvent : MetaEvent
    {
        public PortEvent(long delta) 
            : base(delta, MetaEventTypes.PortEvent)
        {
        }

        public byte Port { get; set; }

        public override string ToString()
        {
            return $"Port Delta {Delta} Port {Port}";
        }

    }
}
