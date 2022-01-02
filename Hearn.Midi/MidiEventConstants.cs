using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi
{
    internal class MidiEventConstants
    {

        public const byte META_EVENT = 0xFF;
        public const int META_EVENT_SEQUENCE_NUMBER = 0x0002;        
        
        //LSB for text events is length, defined as 0x00 to allow anything throw in bit mask
        public const int META_EVENT_TEXT = 0x0100;
        public const int META_EVENT_COPYRIGHT_NOTICE = 0x0200;
        public const int META_EVENT_TRACK_NAME = 0x0300;
        public const int META_EVENT_INSTRUMENT_NAME = 0x0400;
        public const int META_EVENT_LYRIC = 0x0500;
        public const int META_EVENT_MARKER = 0x0600;
        public const int META_EVENT_CUE_POINT = 0x0700;

        public const int META_EVENT_CHANNEL_PREFIX = 0x2001;
        public const int META_EVENT_END_OF_TRACK = 0x2F00;
        public const int META_EVENT_SET_TEMPO = 0x5103;
        public const int META_EVENT_TIME_SIGNATURE = 0x5804;

        public const byte NOTE_ON_EVENT = 0x90;
        public const byte NOTE_OFF_EVENT = 0x80;

    }
}
