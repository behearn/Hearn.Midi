using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public class TrackChunk : BaseMidiData
    {

        public TrackChunk()
            :base(MidiDataTypes.TrackChunk)
        {
        }

        public int TrackNo { get; set; }

        public long Length { get; set; }

    }
}
