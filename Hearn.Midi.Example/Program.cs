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
                    .WriteHeader(Formats.MultiSimultaneousTracks, 3);

                midiStreamWriter
                    .WriteStartTrack()
                        .WriteTimeSignature(3, 4)
                        .WriteTempo(104)
                        .WriteString(StringTypes.TrackName, "Minuet in G")
                        .WriteString(StringTypes.CopyrightNotice, "Johann Sebastian Bach")
                    .WriteEndTrack();

                midiStreamWriter
                    .WriteStartTrack()
                        //Measure 1
                        .WriteNoteAndWait(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 2
                        .WriteNoteAndWait(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNote(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Wait(NoteDurations.QuarterNote)
                        .WriteNote(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Wait(NoteDurations.QuarterNote)
                        //Measure 3
                        .WriteNoteAndWait(0, MidiNoteNumbers.E5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO + 10, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO + 20, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.E5, VELOCITY_MEZZO_PIANO + 30, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.FSGF5, VELOCITY_MEZZO_PIANO + 40, NoteDurations.EighthNote)
                        //Measure 4
                        .WriteNoteAndWait(0, MidiNoteNumbers.G5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNote(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Wait(NoteDurations.QuarterNote)
                        .WriteNote(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Wait(NoteDurations.QuarterNote)
                        //Measure 5
                        .WriteNoteAndWait(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 6
                        .WriteNoteAndWait(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 7
                        .WriteNoteAndWait(0, MidiNoteNumbers.FSGF4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 8
                        .WriteNoteAndWait(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNote)
                        //Measure 9
                        .WriteNoteAndWait(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 10
                        .WriteNoteAndWait(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNote(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Wait(NoteDurations.QuarterNote)
                        .WriteNote(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Wait(NoteDurations.QuarterNote)
                        //Measure 11
                        .WriteNoteAndWait(0, MidiNoteNumbers.E5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO + 10, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO + 20, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.E5, VELOCITY_MEZZO_PIANO + 30, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.FSGF5, VELOCITY_MEZZO_PIANO + 40, NoteDurations.EighthNote)
                        //Measure 12
                        .WriteNoteAndWait(0, MidiNoteNumbers.G5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNote(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Wait(NoteDurations.QuarterNote)
                        .WriteNote(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Wait(NoteDurations.QuarterNote)
                        //Measure 13
                        .WriteNoteAndWait(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.D5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 14
                        .WriteNoteAndWait(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.C5, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 15
                        .WriteNoteAndWait(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.B4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.A4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(0, MidiNoteNumbers.FSGF4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 16
                        .WriteNoteAndWait(0, MidiNoteNumbers.G4, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNoteDotted)
                    .WriteEndTrack();


                midiStreamWriter
                    .WriteStartTrack()
                        //Measure 1
                        .WriteNotes(1, new List<MidiNote>()
                        {
                            new MidiNote() { Note = MidiNoteNumbers.G3, Velocity = VELOCITY_MEZZO_PIANO, Duration = NoteDurations.HalfNote},
                            new MidiNote() { Note = MidiNoteNumbers.B3, Velocity = VELOCITY_MEZZO_PIANO, Duration = NoteDurations.HalfNote},
                            new MidiNote() { Note = MidiNoteNumbers.D3, Velocity = VELOCITY_MEZZO_PIANO, Duration = NoteDurations.HalfNote},
                        })
                        .Wait(NoteDurations.HalfNote)
                        .WriteNoteAndWait(1, MidiNoteNumbers.A3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 2
                        .WriteNoteAndWait(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNoteDotted)
                        //Measure 3
                        .WriteNoteAndWait(1, MidiNoteNumbers.C4, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNoteDotted)
                        //Measure 4
                        .WriteNoteAndWait(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNoteDotted)
                        //Measure 5
                        .WriteNoteAndWait(1, MidiNoteNumbers.A3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNoteDotted)
                        //Measure 6
                        .WriteNoteAndWait(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNoteDotted)
                        //Measure 7
                        .WriteNoteAndWait(1, MidiNoteNumbers.D3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndWait(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndWait(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 8
                        .WriteNoteAndWait(1, MidiNoteNumbers.D3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndWait(1, MidiNoteNumbers.D2, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(1, MidiNoteNumbers.C4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(1, MidiNoteNumbers.A3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 9
                        .WriteNoteAndWait(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNote)
                        .WriteNoteAndWait(1, MidiNoteNumbers.A3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 10
                        .WriteNoteAndWait(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNote(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Wait(NoteDurations.QuarterNote)
                        .WriteNote(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Wait(NoteDurations.QuarterNote)
                        //Measure 11
                        .WriteNoteAndWait(1, MidiNoteNumbers.C4, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNoteDotted)
                        //Measure 12
                        .WriteNoteAndWait(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndWait(1, MidiNoteNumbers.C4, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(1, MidiNoteNumbers.A3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .WriteNoteAndWait(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        //Measure 13
                        .WriteNoteAndWait(1, MidiNoteNumbers.A3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNote)
                        .WriteNoteAndWait(1, MidiNoteNumbers.FSGF3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 14
                        .WriteNoteAndWait(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNote)
                        .WriteNoteAndWait(1, MidiNoteNumbers.B3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 15
                        .WriteNoteAndWait(1, MidiNoteNumbers.C4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndWait(1, MidiNoteNumbers.D4, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        .WriteNoteAndWait(1, MidiNoteNumbers.D3, VELOCITY_MEZZO_PIANO, NoteDurations.QuarterNote)
                        //Measure 16
                        .WriteNoteAndWait(1, MidiNoteNumbers.G3, VELOCITY_MEZZO_PIANO, NoteDurations.HalfNote)
                        .WriteNote(1, MidiNoteNumbers.G2, VELOCITY_MEZZO_PIANO, NoteDurations.EighthNote)
                        .Wait(NoteDurations.QuarterNote)
                    .WriteEndTrack();


            }

        }
    }
}
