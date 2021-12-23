using System;
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
                    .WriteHeader(Formats.MultiSimultaneousTracks, 2);

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
                    .WriteEndTrack();

            }

        }
    }
}
