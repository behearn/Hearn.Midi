﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.MidiData
{
    public class TempoEvent : MetaEvent
    {

        public TempoEvent(long delta)
            : base(delta, MetaEventTypes.TempoEvent)
        {
        }
        
        public long MicroSecondsPerQuarterNote { get; set; }

        public long Tempo{ get => MidiEventConstants.MICROSECONDS_PER_MINUTE / MicroSecondsPerQuarterNote; }

        public override string ToString()
        {
            return $"Temp Delta {Delta} {Tempo} bpm";
        }
    }
}
