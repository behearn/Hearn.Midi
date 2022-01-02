using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public class StringEvent : MetaEvent
    {
        public StringEvent(long delta)
           : base(delta)
        {
        }        

        public MidiConstants.StringTypes StringType { get; set; }

        public string Text { get; set; }

        public override string ToString()
        {
            return $"String Delta {Delta} {StringType} {Text}";
        }

    }
}
