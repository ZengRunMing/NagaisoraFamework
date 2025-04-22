using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine;

using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;

namespace NagaisoraFamework
{
    public class MidiPianoFormShow : CommMonoScriptObject
    {
		public Note[] Notes;

		public int Channel;

		public List<Image> NoteKeys;

		[SerializeField]
		private List<Color> KeyNormalColors;

		public Color KeyDownColor;

		public float Alahpa = 1f;

		public void Awake()
		{
			KeyNormalColors = new List<Color>();

			foreach(Image i in NoteKeys)
			{
				KeyNormalColors.Add(new Color(i.color.r, i.color.g, i.color.b, Alahpa));
			}
		}

		public void FixedUpdate()
		{
			for(int i = 0; i < NoteKeys.Count; i++)
			{
				NoteKeys[i].color = KeyNormalColors[i];
			}

			if (Notes == null || Notes.Length == 0)
			{
				return;
			}

			foreach (Note note in Notes)
			{
				NoteKeys[note.NoteNumber].color = new Color(KeyDownColor.r, KeyDownColor.g, KeyDownColor.b, Alahpa);
			}
		}
	}
}
