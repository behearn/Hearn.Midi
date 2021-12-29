# Hearn.Midi

A .net implementation of the MIDI file standard without using Windows API wrappers.

Standard MIDI Files Specification [RP-001_v1-0_Standard_MIDI_Files_Specification_96-1-4.pdf
](https://www.midi.org/specifications/file-format-specifications/standard-midi-files/rp-001-v1-0-standard-midi-files-specification-96-1-4-pdf)

## MidiStreamWriter

A fluent stream writer to create .mid files.

Call `.WriteHeader()` and specify the file format (single or multi track), and the number of tracks.

For each track wrap the content between `.WriteStartTrack()` and `.WriteEndTrack()`.

Unless overridden using `.WriteTimeSignature()` and `.WriteTempo` tracks will be 4:4 time 120bpm.

Notes are created using `.WriteNote()` supplying
- `channel` - One of 16 channels (zero based) [`0`..`15`].  Channel 10 [`9`] is reserved for percussion.
- `note` - The value `MidiNoteNumbers.C4` is Middle C
- `velocity`- Soft to loud (`0`..`127`)
- `duration` - length of the note (`NoteDurations.QuarterNote`,  `NoteDurations.Crotchet` and `96` are all equivalent)
  
As more notes are written they are stacked at the same starting point.  

Notes are turned off by calling  `.Tick()` to progress through the track as the `NoteDurations` value for the note is ticked over by the `NoteDurations` value for the tick.

`.WritePercussion()`  writes to channel 10 using the note mappings from the `Percussion` enum.

### Example Usage

The following example will write a C Major I-V-vi-IV chord progression.

```
var midiStream = new FileStream("music.mid", FileMode.OpenOrCreate);

using (var midiStreamWriter = new MidiStreamWriter(midiStream))
{
    midiStreamWriter
        .WriteHeader(Formats.MultiSimultaneousTracks, 2);

    midiStreamWriter
        .WriteStartTrack()
            .WriteTimeSignature(4, 4)
            .WriteTempo(120)
            .WriteString(StringTypes.TrackName, "Example Track")
        .WriteEndTrack();
        
    midiStreamWriter
        .WriteStartTrack()
            .WriteNote(0, MidiNoteNumbers.C4, 127, NoteDurations.WholeNote)
            .WriteNote(0, MidiNoteNumbers.E4, 127, NoteDurations.WholeNote)
            .WriteNote(0, MidiNoteNumbers.G4, 127, NoteDurations.WholeNote)
            .Tick(NoteDurations.WholeNote)
            .WriteNote(0, MidiNoteNumbers.G4, 127, NoteDurations.WholeNote)
            .WriteNote(0, MidiNoteNumbers.B4, 127, NoteDurations.WholeNote)
            .WriteNote(0, MidiNoteNumbers.D5, 127, NoteDurations.WholeNote)
            .Tick(NoteDurations.WholeNote)
            .WriteNote(0, MidiNoteNumbers.A4, 127, NoteDurations.WholeNote)
            .WriteNote(0, MidiNoteNumbers.C5, 127, NoteDurations.WholeNote)
            .WriteNote(0, MidiNoteNumbers.E5, 127, NoteDurations.WholeNote)
            .Tick(NoteDurations.WholeNote)
            .WriteNote(0, MidiNoteNumbers.F4, 127, NoteDurations.WholeNote)
            .WriteNote(0, MidiNoteNumbers.A4, 127, NoteDurations.WholeNote)
            .WriteNote(0, MidiNoteNumbers.C5, 127, NoteDurations.WholeNote)
            .Tick(NoteDurations.WholeNote)
        .WriteEndTrack();
}
```

### Convenience methods

Multiple notes can be written using `.WriteNotes()`

The following example will write a C Major chord with a single method call

```
midiStreamWriter.WriteNotes(0, new List<MidiNote>()
{
    new MidiNote() { Note = MidiNoteNumbers.C4, Velocity = 127, Duration = NoteDurations.WholeNote },
    new MidiNote() { Note = MidiNoteNumbers.E4, Velocity = 127, Duration = NoteDurations.WholeNote },
    new MidiNote() { Note = MidiNoteNumbers.G4, Velocity = 127, Duration = NoteDurations.WholeNote }
});
```

Notes can be written and automatically ticked over using `.WriteNoteAndTick()`.  

The following example will write a C Major arpeggio over a single (4:4) bar

```
midiStreamWriter
    .WriteNoteAndTick(0, MidiNoteNumbers.C4, 127, NoteDurations.QuarterNote)
    .WriteNoteAndTick(0, MidiNoteNumbers.E4, 127, NoteDurations.QuarterNote)
    .WriteNoteAndTick(0, MidiNoteNumbers.G4, 127, NoteDurations.QuarterNote)
    .WriteNoteAndTick(0, MidiNoteNumbers.C5, 127, NoteDurations.QuarterNote);
```

## Worked example

The project **Hearn.Midi.Example** is an example console application that creates an example .mid (saved to bin folder) file with 5 tracks
1. Track meta data (3:4 time, 104 bpm & track name)
2. Right hand piano
3. Left hand piano
4. Chords
5. Percussion

## Known issues

The "running status" concept in MIDI files is not strictly adhered to.  Extra (but passive) note on (0x90) and note off (0x80) events are written.