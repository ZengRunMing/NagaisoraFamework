using System;
using System.Collections.Generic;
using System.Linq;

using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;

namespace NagaisoraFamework.Miedia
{
    public class MidiPianoFormControl : CommMonoScriptObject
    {
		public MidiControl MidiControl;

		public MidiPianoForm[] MidiPianoForms;

		public IEnumerable<Note> notes;

		public IEnumerable<TrackChunk> MidiChannels;

		public IDictionary<int, List<Note>> Notekeys = new Dictionary<int, List<Note>>();

		public void Init()
		{
			Notekeys.Clear();

			foreach (var form in MidiPianoForms)
			{
				Notekeys.Add(form.ID, new());
			}
			
			MidiControl.Playback.NotesPlaybackStarted += KeyDownEvent;
			MidiControl.Playback.NotesPlaybackFinished += KeyUpEnevt;
		}

		public void KeyDownEvent(object sender, NotesEventArgs e)
		{
			foreach(var note in e.Notes)
			{
				Notekeys[note.Channel].Add(note);
			}

			KeyUpdate();
		}

		public void KeyUpEnevt(object sender, NotesEventArgs e)
		{
			foreach (var note in e.Notes)
			{
				Notekeys[note.Channel].Remove(note);
			}

			KeyUpdate();
		}

		public void KeyUpdate()
		{
			foreach (MidiPianoForm midiPianoForm in MidiPianoForms)
			{
				if (!Notekeys.ContainsKey(midiPianoForm.ID))
				{
					continue;
				}

				midiPianoForm.notes = Notekeys[midiPianoForm.ID].ToArray();
			}
		}
	}
}