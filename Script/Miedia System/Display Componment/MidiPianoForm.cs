using UnityEngine.UI;

using Melanchall.DryWetMidi.Interaction;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Multimedia;

namespace NagaisoraFamework.Miedia
{
	public class MidiPianoForm : CommMonoScriptObject
	{
		public Text Name;

		public Text Message;

		public MidiPianoFormShow MidiPianoControl;

		public int ID;

		public string CTName;

		public Note[] notes;

		public bool KeyUpdate;

		public void Start()
		{
			MidiPianoControl.Channel = ID;
			Name.text = (ID + 1).ToString("CH00");
		}

		public void FixedUpdate()
		{
			Message.text = "";

			if (MidiPianoControl != null)
			{
				MidiPianoControl.Notes = notes;
			}

			if (notes == null || notes.Length == 0)
			{
				return;
			}

			foreach (Note note in notes)
			{
				if (Message.text != null & Message.text != "")
				{
					Message.text += " | ";
				}

				Message.text += note + " " + note.Velocity;
			}
		}
	}
}