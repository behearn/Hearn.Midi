using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public class HeaderChunk : BaseMidiData
    {

        public HeaderChunk()
            :base(MidiDataTypes.HeaderChunk)
        {
        }

        public MidiConstants.Formats Format { get; set; }

        public int Tracks { get; set; }

        public int Division { get; set; }

        public override string ToString()
        {
            return $"MIDI Header {Format} {Tracks} tracks ({Division} ticks per quarter note)";
        }
    }
}
