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

        //public const byte META_EVENT_SEQUENCE_NUMBER = 0x00;
        public const byte META_EVENT_TEXT = 0x01;
        public const byte META_EVENT_COPYRIGHT_NOTICE = 0x02;
        public const byte META_EVENT_TRACK_NAME = 0x03;
        public const byte META_EVENT_INSTRUMENT_NAME = 0x04;
        public const byte META_EVENT_LYRIC = 0x05;
        public const byte META_EVENT_MARKER = 0x06;
        public const byte META_EVENT_CUE_POINT = 0x07;
        //public const byte META_EVENT_CHANNEL_PREFIX = 0x20;
        public const byte META_EVENT_PORT = 0x21;
        public const byte META_EVENT_END_OF_TRACK = 0x2F;
        public const byte META_EVENT_SET_TEMPO = 0x51;
        public const byte META_EVENT_TIME_SIGNATURE = 0x58;
        public const byte META_EVENT_KEY_SIGNATURE = 0x59;

        //LSB for channel, use MIDI_EVENT_MASK to compare
        public const byte MIDI_EVENT_NOTE_OFF = 0x80;
        public const byte MIDI_EVENT_NOTE_ON = 0x90;
        public const byte MIDI_EVENT_CONTROL_CHANGE = 0xB0;
        public const byte MIDI_EVENT_PROGRAM_CHANGE = 0xC0;
        public const byte MIDI_EVENT_SYSEX = 0xF0;

        //For use with META_EVENT_SET_TEMPO
        public const long MICROSECONDS_PER_MINUTE = 60000000;

        //Bit masks
        public const byte MASK_MIDI_EVENT_TYPE = 0xF0;
        public const byte MASK_MIDI_EVENT_CHANNEL = 0x0F;

    }
}
