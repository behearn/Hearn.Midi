﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hearn.Midi.MidiConstants;

namespace Hearn.Midi.MidiData
{
    public class ControllerEvent : MidiEvent
    {

        public ControllerEvent(long delta)
           : base(delta, MidiEventTypes.Controller)
        {
        }

        public int Channel { get; set; }

        public ControlChangeTypes ControlChangeType { get; set; }

        public byte Value { get; set; }

        public override string ToString()
        {
            return $"Control change Delta {Delta} Channel {Channel} ControlChangeTy[r {ControlChangeType.ToString()} Value {Value}";
        }

    }
}
