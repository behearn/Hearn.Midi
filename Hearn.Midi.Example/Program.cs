using Hearn.Midi.Models;
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
        const byte VELOCITY_SOFT = 32;

        static void Main(string[] args)
        {

            if (File.Exists(FILENAME))
            {
                File.Delete(FILENAME);
            }

            Console.WriteLine($"Writing {FILENAME}...");

            WriteMidiFile();

            Console.WriteLine($"{FILENAME} written");

            Console.WriteLine("Press any key to read file");
            Console.ReadKey();

            ReadMidiFile();

            Console.WriteLine($"{FILENAME} read");

        }

        static void WriteMidiFile()
        {
            var midiStream = new FileStream(FILENAME, FileMode.OpenOrCreate);

            using (var midiStreamWriter = new MidiStreamWriter(midiStream))
            {

                midiStreamWriter
                    .WriteHeader(Formats.MultiSimultaneousTracks, 5);

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

                WriteChords(midiStreamWriter);

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

        static void WriteChords(MidiStreamWriter midiStreamWriter)
        {

            midiStreamWriter
                 .WriteStartTrack()
                 .WriteChangeInstrument(3, Instruments.StringEnsemble1)
                     //Measures 1 & 2
                     .WriteNotes(3, GetChordGMajor(NoteDurations.WholeNoteDotted))
                     .Tick(NoteDurations.WholeNoteDotted)
                     //Measures 3
                     .WriteNotes(3, GetChordCMajor(NoteDurations.HalfNoteDotted))
                     .Tick(NoteDurations.HalfNoteDotted)
                     //Measures 4
                     .WriteNotes(3, GetChordGMajor(NoteDurations.HalfNoteDotted))
                     .Tick(NoteDurations.HalfNoteDotted)
                     //Measures 5
                     .WriteNotes(3, GetChordAMinor(NoteDurations.HalfNoteDotted))
                     .Tick(NoteDurations.HalfNoteDotted)
                     //Measures 6
                     .WriteNotes(3, GetChordGMajor(NoteDurations.HalfNoteDotted))
                     .Tick(NoteDurations.HalfNoteDotted)
                     //Measures 7
                     .WriteNotes(3, GetChordDMajor(NoteDurations.QuarterNote))
                     .Tick(NoteDurations.QuarterNote)
                     .WriteNotes(3, GetChordGMajor(NoteDurations.HalfNote))
                     .Tick(NoteDurations.HalfNote)
                     //Measures 8
                     .WriteNotes(3, GetChordDMajor7(NoteDurations.HalfNoteDotted))
                     .Tick(NoteDurations.HalfNoteDotted)
                     //Measures 9 & 10
                     .WriteNotes(3, GetChordGMajor(NoteDurations.WholeNoteDotted))
                     .Tick(NoteDurations.WholeNoteDotted)
                     //Measures 11
                     .WriteNotes(3, GetChordCMajor(NoteDurations.HalfNoteDotted))
                     .Tick(NoteDurations.HalfNoteDotted)
                     //Measures 12
                     .WriteNotes(3, GetChordGMajor(NoteDurations.HalfNoteDotted))
                     .Tick(NoteDurations.HalfNoteDotted)
                     //Measures 13
                     .WriteNotes(3, GetChordAMinor(NoteDurations.HalfNote))
                     .Tick(NoteDurations.HalfNote)
                     .WriteNotes(3, GetChordDMajor7(NoteDurations.QuarterNote))
                     .Tick(NoteDurations.QuarterNote)
                     //Measures 14
                     .WriteNotes(3, GetChordGMajor(NoteDurations.HalfNoteDotted))
                     .Tick(NoteDurations.HalfNoteDotted)
                     //Measures 15
                     .WriteNotes(3, GetChordAMinor(NoteDurations.QuarterNote))
                     .Tick(NoteDurations.QuarterNote)
                     .WriteNotes(3, GetChordDMajor7(NoteDurations.HalfNote))
                     .Tick(NoteDurations.HalfNote)
                     //Measures 16 & 17
                     .WriteNotes(3, GetChordGMajor(NoteDurations.WholeNoteDotted))
                     .Tick(NoteDurations.WholeNoteDotted)
                     //Measures 18
                     .WriteNotes(3, GetChordDMajor7(NoteDurations.HalfNoteDotted))
                     .Tick(NoteDurations.HalfNoteDotted)
                     //Measures 19
                     .WriteNotes(3, GetChordCMajor(NoteDurations.HalfNoteDotted))
                     .Tick(NoteDurations.HalfNoteDotted)
                     //Measures 20 & 21
                     .WriteNotes(3, GetChordAMajor(NoteDurations.WholeNoteDotted))
                     .Tick(NoteDurations.WholeNoteDotted)
                     //Measures 22
                     .WriteNotes(3, GetChordGMajor(NoteDurations.QuarterNote))
                     .Tick(NoteDurations.QuarterNote)
                     .WriteNotes(3, GetChordDMajor(NoteDurations.QuarterNote))
                     .Tick(NoteDurations.QuarterNote)
                     .WriteNotes(3, GetChordAMajor(NoteDurations.QuarterNote))
                     .Tick(NoteDurations.QuarterNote)
                     //Measures 23
                     .WriteNotes(3, GetChordDMajor(NoteDurations.HalfNote))
                     .Tick(NoteDurations.HalfNote)
                     .WriteNotes(3, GetChordAMajor(NoteDurations.QuarterNote))
                     .Tick(NoteDurations.QuarterNote)
                     //Measures 24
                     .WriteNotes(3, GetChordDMajor(NoteDurations.HalfNote))
                     .Tick(NoteDurations.HalfNote)
                     .WriteNotes(3, GetChordDMajor7(NoteDurations.QuarterNote))
                     .Tick(NoteDurations.QuarterNote)
                     //Measures 25
                     .WriteNotes(3, GetChordGMajor(NoteDurations.HalfNoteDotted))
                     .Tick(NoteDurations.HalfNoteDotted)
                     //Measures 26
                     .WriteNotes(3, GetChordCMajor(NoteDurations.HalfNoteDotted))
                     .Tick(NoteDurations.HalfNoteDotted)
                     //Measures 27
                     .WriteNotes(3, GetChordGMajor(NoteDurations.QuarterNote))
                     .Tick(NoteDurations.QuarterNote)
                     .WriteNotes(3, GetChordAMinor(NoteDurations.QuarterNote))
                     .Tick(NoteDurations.QuarterNote)
                     .WriteNotes(3, GetChordGMajor(NoteDurations.QuarterNote))
                     .Tick(NoteDurations.QuarterNote)
                     //Measures 28 & 29
                     .WriteNotes(3, GetChordDMajor(NoteDurations.WholeNoteDotted))
                     .Tick(NoteDurations.WholeNoteDotted)
                     //Measures 30
                     .WriteNotes(3, GetChordCMajor(NoteDurations.QuarterNote))
                     .Tick(NoteDurations.QuarterNote)
                     .WriteNotes(3, GetChordGMajor(NoteDurations.QuarterNote))
                     .Tick(NoteDurations.QuarterNote)
                     .WriteNotes(3, GetChordDMajor(NoteDurations.QuarterNote))
                     .Tick(NoteDurations.QuarterNote)
                     //Measures 31
                     .WriteNotes(3, GetChordGMajor(NoteDurations.HalfNote))
                     .Tick(NoteDurations.HalfNote)
                     .WriteNotes(3, GetChordDMajor7(NoteDurations.QuarterNote))
                     .Tick(NoteDurations.QuarterNote)
                     //Measures 32
                     .WriteNotes(3, GetChordGMajor(NoteDurations.HalfNoteDotted))
                     .Tick(NoteDurations.HalfNoteDotted)
                .WriteEndTrack();

        }

        static List<MidiNote>GetChordGMajor(NoteDurations duration)
        {
            return new List<MidiNote>()
            {
                new MidiNote() { Note = MidiNoteNumbers.G3, Velocity = VELOCITY_SOFT, Duration = duration },
                new MidiNote() { Note = MidiNoteNumbers.B3, Velocity = VELOCITY_SOFT, Duration = duration },
                new MidiNote() { Note = MidiNoteNumbers.D4, Velocity = VELOCITY_SOFT, Duration = duration }
            };
        }

        static List<MidiNote> GetChordCMajor(NoteDurations duration)
        {
            return new List<MidiNote>()
            {
                new MidiNote() { Note = MidiNoteNumbers.C4, Velocity = VELOCITY_SOFT, Duration = duration },
                new MidiNote() { Note = MidiNoteNumbers.E4, Velocity = VELOCITY_SOFT, Duration = duration },
                new MidiNote() { Note = MidiNoteNumbers.G3, Velocity = VELOCITY_SOFT, Duration = duration }
            };
        }

        static List<MidiNote> GetChordAMinor(NoteDurations duration)
        {
            return new List<MidiNote>()
            {
                new MidiNote() { Note = MidiNoteNumbers.A3, Velocity = VELOCITY_SOFT, Duration = duration },
                new MidiNote() { Note = MidiNoteNumbers.C4, Velocity = VELOCITY_SOFT, Duration = duration },
                new MidiNote() { Note = MidiNoteNumbers.E4, Velocity = VELOCITY_SOFT, Duration = duration }
            };
        }

        static List<MidiNote> GetChordDMajor(NoteDurations duration)
        {
            return new List<MidiNote>()
            {
                new MidiNote() { Note = MidiNoteNumbers.D4, Velocity = VELOCITY_SOFT, Duration = duration },
                new MidiNote() { Note = MidiNoteNumbers.FSGF3, Velocity = VELOCITY_SOFT, Duration = duration },
                new MidiNote() { Note = MidiNoteNumbers.A3, Velocity = VELOCITY_SOFT, Duration = duration }
            };
        }

        static List<MidiNote> GetChordDMajor7(NoteDurations duration)
        {
            return new List<MidiNote>()
            {
                new MidiNote() { Note = MidiNoteNumbers.D4, Velocity = VELOCITY_SOFT, Duration = duration },
                new MidiNote() { Note = MidiNoteNumbers.FSGF3, Velocity = VELOCITY_SOFT, Duration = duration },
                new MidiNote() { Note = MidiNoteNumbers.A3, Velocity = VELOCITY_SOFT, Duration = duration },
                new MidiNote() { Note = MidiNoteNumbers.C4, Velocity = VELOCITY_SOFT, Duration = duration }
            };
        }

        static List<MidiNote> GetChordAMajor(NoteDurations duration)
        {
            return new List<MidiNote>()
            {
                new MidiNote() { Note = MidiNoteNumbers.A3, Velocity = VELOCITY_SOFT, Duration = duration },
                new MidiNote() { Note = MidiNoteNumbers.CSDF4, Velocity = VELOCITY_SOFT, Duration = duration },
                new MidiNote() { Note = MidiNoteNumbers.E3, Velocity = VELOCITY_SOFT, Duration = duration }
            };
        }

        static void ReadMidiFile()
        {

            var fileStream = new FileStream(FILENAME, FileMode.Open, FileAccess.Read);  

            using (var midiStreamReader = new MidiStreamReader(fileStream))
            {

                var midiData = midiStreamReader.Read();
                while(midiData != null)
                {
                    Console.WriteLine(midiData);
                    midiData = midiStreamReader.Read();
                }

            }
        }
    }
}
