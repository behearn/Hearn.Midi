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

        //LSB for text events is length, use MASK_META_EVENT_TEXT to compare
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

        //LSB for channel, use MIDI_EVENT_MASK to compare
        public const byte MIDI_EVENT_NOTE_OFF = 0x80;
        public const byte MIDI_EVENT_NOTE_ON = 0x90;
        public const byte MIDI_EVENT_PROGRAM_CHANGE = 0xC0;

        //For use with META_EVENT_SET_TEMPO
        public const long MICROSECONDS_PER_MINUTE = 60000000;

        //Bit masks
        public const byte MASK_MIDI_EVENT_TYPE = 0xF0;
        public const byte MASK_MIDI_EVENT_CHANNEL = 0x0F;
        public const int MASK_META_EVENT_TEXT = 0xFF00;
        public const int MASK_META_EVENT_TEXT_TYPE = 0x00FF;

    }
}
