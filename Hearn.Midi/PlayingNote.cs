using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hearn.Midi.MidiConstants;

namespace Hearn.Midi
{
    class PlayingNote
    {

        private static int _globalId;
        private int _id;

        public PlayingNote()
        {
            _globalId++;
            _id = _globalId;
        }

        public int Id { get => _id; }

        public byte Channel { get; set; }

        public MidiNoteNumbers Note { get; set; }

        public long StartTick { get; set; }

        public long EndTick { get; set; }

    }
}
