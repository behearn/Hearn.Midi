#Hearn.Midi

**Work in progress**

So far only test with the examples in the MIDI specification itself


##MidiStreamWriter

A fluent stream writer to create .mid files

Instead of explicitly supplying NoteOn and NoteOff events, notes are created with their duration and automatically turned off.

Check the Format_X_Specification.cs unit tests for usage examples