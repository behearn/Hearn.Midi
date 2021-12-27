using System;
using System.Collections.Generic;
using System.IO;
using static Hearn.Midi.MidiConstants;
using static Hearn.Midi.MidiStreamWriter;

namespace Hearn.Midi.Example
{
    class Program
    {
        const string FILENAME = "MinuetInG.mid";
        const byte VELOCITY_MEZZO_PIANO = 64;

        static void Main(string[] args)
        {
             
            if (File.Exists(FILENAME))
            {
                File.Delete(FILENAME);
            }

            var midiStream = new FileStream(FILENAME, FileMode.OpenOrCreate);

            using (var midiStreamWriter = new MidiStreamWriter(midiStream))
            {

                midiStreamWriter
                    .WriteHeader(Formats.MultiSimultaneousTracks, 4);

                midiStreamWriter
                    .WriteStartTrack()
                        .WriteTimeSignature(3, 4)
                        .WriteTempo(104)
                        .WriteString(StringTypes.TrackName, "Minuet in G")
                        .WriteString(StringTypes.CopyrightNotice, "Christian Petzold")
                        .WriteString(StringTypes.ArbitraryText, "From Johann Sebastian Bach's Notebook for Anna Magdalena Bach")
                    .WriteEndTrack();

                WriteTrack1(midiStreamWriter);

                WriteTrack2(midiStreamWriter);

                WritePercussionTrack(midiStreamWriter);

                //using will call Dispose which automatically flushes and closes the stream
            }

        }

        static void WriteTrack1(MidiStreamWriter midiStreamWriter)
        {

            midiStreamWriter
                    .WriteStartTrack()
                        //Measure 1
                        .WriteNoteAndTick(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 2
                        .WriteNoteAndTick(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNote(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        .WriteNote(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        //Measure 3
                        .WriteNoteAndTick(0, MidiNoteNumbers.E5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO + 10, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO + 20, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.E5, VELOCITY_MEZZO_PIANO + 30, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.FSGF5, VELOCITY_MEZZO_PIANO + 40, NoteDurations.EighthNote)
                        //Measure 4
                        .WriteNoteAndTick(0, MidiNoteNumbers.G5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNote(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        .WriteNote(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        //Measure 5
                        .WriteNoteAndTick(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 6
                        .WriteNoteAndTick(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 7
                        .WriteNoteAndTick(0, MidiNoteNumbers.FSGF4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 8
                        .WriteNoteAndTick(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNote)
                        //Measure 9
                        .WriteNoteAndTick(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 10
                        .WriteNoteAndTick(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNote(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        .WriteNote(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        //Measure 11
                        .WriteNoteAndTick(0, MidiNoteNumbers.E5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO + 10, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO + 20, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.E5, VELOCITY_MEZZO_PIANO + 30, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.FSGF5, VELOCITY_MEZZO_PIANO + 40, NoteDurations.EighthNote)
                        //Measure 12
                        .WriteNoteAndTick(0, MidiNoteNumbers.G5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNote(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        .WriteNote(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        //Measure 13
                        .WriteNoteAndTick(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 14
                        .WriteNoteAndTick(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 15
                        .WriteNoteAndTick(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.FSGF4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 16
                        .WriteNoteAndTick(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNoteDotted)
                        //Measure 17
                        .WriteNoteAndTick(0, MidiNoteNumbers.B5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.G5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.A5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.G5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 18
                        .WriteNoteAndTick(0, MidiNoteNumbers.A5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.E5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.FSGF5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 19
                        .WriteNoteAndTick(0, MidiNoteNumbers.G5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.E5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.FSGF5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.G5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 20
                        .WriteNoteAndTick(0, MidiNoteNumbers.CSDF5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.CSDF5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNote(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        //Measure 21
                        .WriteNoteAndTick(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.CSDF5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.E5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.FSGF5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 22
                        .WriteNoteAndTick(0, MidiNoteNumbers.G5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.FSGF5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.E5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 23
                        .WriteNote(0, MidiNoteNumbers.FSGF5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        .WriteNote(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        .WriteNote(0, MidiNoteNumbers.CSDF5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        //Measure 24
                        .WriteNoteAndTick(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNoteDotted)
                        //Measure 25
                        .WriteNoteAndTick(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.G5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.FSGF5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNote(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        //Measure 26
                        .WriteNoteAndTick(0, MidiNoteNumbers.E5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.FSGF4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNote(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        //Measure 27
                        .WriteNoteAndTick(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 28
                        .WriteNoteAndTick(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.FSGF4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 29
                        .WriteNoteAndTick(0, MidiNoteNumbers.D4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.E4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.FSGF4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 30
                        .WriteNoteAndTick(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 31
                        .WriteNoteAndTick(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNote(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        .WriteNote(0, MidiNoteNumbers.FSGF4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        //Measure 32
                        .WriteNotes(0, new List<MidiNote>()
                        {
                            new MidiNote() { Note = MidiNoteNumbers.G4, Velocity = VELOCITY_MEZZO_PIANO, Duration = NoteDurations.HalfNoteDotted},
                            new MidiNote() { Note = MidiNoteNumbers.D4, Velocity = VELOCITY_MEZZO_PIANO, Duration = NoteDurations.HalfNoteDotted},
                            new MidiNote() { Note = MidiNoteNumbers.B3, Velocity = VELOCITY_MEZZO_PIANO, Duration = NoteDurations.HalfNoteDotted},
                        })
                        .Tick(NoteDurations.HalfNoteDotted)
                    .WriteEndTrack();

        }

        static void WriteTrack2(MidiStreamWriter midiStreamWriter)
        {
            midiStreamWriter
                    .WriteStartTrack()
                        //Measure 1
                        .WriteNotes(1, new List<MidiNote>()
                        {
                            new MidiNote() { Note = MidiNoteNumbers.G3, Velocity = VELOCITY_MEZZO_PIANO, Duration = NoteDurations.HalfNote},
                            new MidiNote() { Note = MidiNoteNumbers.B3, Velocity = VELOCITY_MEZZO_PIANO, Duration = NoteDurations.HalfNote},
                            new MidiNote() { Note = MidiNoteNumbers.D3, Velocity = VELOCITY_MEZZO_PIANO, Duration = NoteDurations.HalfNote},
                        })
                        .Tick(NoteDurations.HalfNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.A3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 2
                        .WriteNoteAndTick(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNoteDotted)
                        //Measure 3
                        .WriteNoteAndTick(1, MidiNoteNumbers.C4, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNoteDotted)
                        //Measure 4
                        .WriteNoteAndTick(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNoteDotted)
                        //Measure 5
                        .WriteNoteAndTick(1, MidiNoteNumbers.A3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNoteDotted)
                        //Measure 6
                        .WriteNoteAndTick(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNoteDotted)
                        //Measure 7
                        .WriteNoteAndTick(1, MidiNoteNumbers.D3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 8
                        .WriteNoteAndTick(1, MidiNoteNumbers.D4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.D3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.C4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.A3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 9
                        .WriteNoteAndTick(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.A3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 10
                        .WriteNoteAndTick(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNote(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        .WriteNote(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        //Measure 11
                        .WriteNoteAndTick(1, MidiNoteNumbers.C4, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNoteDotted)
                        //Measure 12
                        .WriteNoteAndTick(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.C4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.A3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 13
                        .WriteNoteAndTick(1, MidiNoteNumbers.A3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.FSGF3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 14
                        .WriteNoteAndTick(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 15
                        .WriteNoteAndTick(1, MidiNoteNumbers.C4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.D4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.D3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 16
                        .WriteNoteAndTick(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNote)
                        .WriteNote(1, MidiNoteNumbers.G2, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        //Measure 17
                        .WriteNoteAndTick(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNoteDotted)
                        //Measure 18
                        .WriteNoteAndTick(1, MidiNoteNumbers.FSGF3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNoteDotted)
                        //Measure 19
                        .WriteNoteAndTick(1, MidiNoteNumbers.E3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.E3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 20
                        .WriteNoteAndTick(1, MidiNoteNumbers.A3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNote)
                        .WriteNote(1, MidiNoteNumbers.A2, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        //Measure 21
                        .WriteNoteAndTick(1, MidiNoteNumbers.A3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNoteDotted)
                        //Measure 22
                        .WriteNoteAndTick(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.D3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.CSDF4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 23
                        .WriteNote(1, MidiNoteNumbers.D4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        .WriteNote(1, MidiNoteNumbers.FSGF3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        .WriteNote(1, MidiNoteNumbers.A3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        //Measure 24
                        .WriteNoteAndTick(1, MidiNoteNumbers.D4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.D3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.C4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 25
                        .WriteNote(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNote)
                        .Tick(NoteDurations.QuarterNote)
                        .WriteNote(1, MidiNoteNumbers.D4, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNote)
                        .Tick(NoteDurations.QuarterNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 26
                        .WriteNote(1, MidiNoteNumbers.C4, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNote)
                        .Tick(NoteDurations.QuarterNote)
                        .WriteNote(1, MidiNoteNumbers.E4, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNote)
                        .Tick(NoteDurations.QuarterNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.C4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 27
                        .WriteNoteAndTick(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.A3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 28
                        .WriteNoteAndTick(1, MidiNoteNumbers.D4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .Tick(NoteDurations.HalfNote)
                        //Measure 29
                        .WriteNote(1, MidiNoteNumbers.D3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNoteDotted)
                        .Tick(NoteDurations.HalfNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.FSGF3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 30
                        .WriteNoteAndTick(1, MidiNoteNumbers.E3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndTick(1, MidiNoteNumbers.FSGF3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 31
                        .WriteNoteAndTick(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNote(1, MidiNoteNumbers.B2, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        .WriteNote(1, MidiNoteNumbers.D3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        //Measure 32
                        .WriteNote(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        .WriteNote(1, MidiNoteNumbers.D3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                        .WriteNote(1, MidiNoteNumbers.G2, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Tick(NoteDurations.QuarterNote)
                    .WriteEndTrack();
        }

        static void WritePercussionTrack(MidiStreamWriter midiStreamWriter)
        {

            midiStreamWriter.WriteStartTrack();
            for (var i = 0; i < 32; i++)
            {
                if (i == 15)
                {
                    //Fill half waythrough
                    midiStreamWriter
                        .WritePercussion(Percussion.HighTom, 80)
                        .Tick(NoteDurations.Triplet)
                        .WritePercussion(Percussion.HighTom, 80)
                        .Tick(NoteDurations.Triplet)
                        .WritePercussion(Percussion.HighTom, 80)
                        .Tick(NoteDurations.Triplet)
                        .WritePercussion(Percussion.HiMidTom, 80)
                        .Tick(NoteDurations.Triplet)
                        .WritePercussion(Percussion.HiMidTom, 80)
                        .Tick(NoteDurations.Triplet)
                        .WritePercussion(Percussion.HiMidTom, 80)
                        .Tick(NoteDurations.Triplet)
                        .WritePercussion(Percussion.LowTom, 80)
                        .Tick(NoteDurations.Triplet)
                        .WritePercussion(Percussion.LowTom, 80)
                        .Tick(NoteDurations.Triplet)
                        .WritePercussion(Percussion.LowTom, 80)
                        .Tick(NoteDurations.Triplet);
                }
                else
                {
                    if (i % 4 == 0)
                    {
                        //Crash every 4th bar
                        midiStreamWriter.WritePercussion(Percussion.CrashCymbal1, 64);
                    }
                    for (var j = 0; j < 9; j++)
                    {
                        //Hihat + bass + snare pattern
                        midiStreamWriter.WritePercussion(Percussion.ClosedHihat, 64);
                        if (j == 0)
                        {
                            midiStreamWriter.WritePercussion(Percussion.AcousticBassDrum, 64);
                        }
                        else if (j == 3 || j == 6)
                        {
                            midiStreamWriter.WritePercussion(Percussion.AcousticSnare, 64);
                        }
                        midiStreamWriter.Tick(NoteDurations.Triplet);
                    }
                }
            }
            midiStreamWriter.WriteEndTrack();
        }

    }
}
