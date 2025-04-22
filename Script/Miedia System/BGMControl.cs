using UnityEngine;

namespace NagaisoraFamework.Miedia
{
	using Miedia;
	using System;

	public class BGMControl : MonoBehaviour
    {
		public AudioControl AudioControl;
		public MidiControl MidiControl;

		public WaveBGMData[] WaveBGMData;
		public MidiBGMData[] MidiBGMData;

		public int WaveIndex;
		public int MidiIndex;

		public bool WaveIsPlaying => AudioControl.AudioSource.isPlaying;

		public bool MidiIsPlaying => MidiControl.Playback != null && MidiControl.Playback.IsRunning;

		public int MidiOutputDeviceID { get => MidiControl.OutputDeviceID; set => MidiControl.OutputDeviceID = value; }

		public void Awake()
		{
			WaveBGMData = MainSystem.WaveBGMData;
			MidiBGMData = MainSystem.MidiBGMData;
			MainSystem.BGMControl = this;

			AudioControl = GetComponent<AudioControl>();
			MidiControl = GetComponent<MidiControl>();
		}

		public WaveBGMData SelectWaveBGMDataOfIndex(int index)
		{
			WaveIndex = index;
			return WaveBGMData[index];
		}

		public MidiBGMData SelectMidiBGMDataOfIndex(int index)
		{
			MidiIndex = index;
			return MidiBGMData[index];
		}

		public void WavePlay()
		{
			AudioControl.SetAudioData(WaveBGMData[WaveIndex]);
			AudioControl.Play();
		}

		public void MidiPlay()
		{
			MidiControl.SetMidiData(MidiBGMData[MidiIndex]);
			MidiControl.Play();
		}

		public void WaveStop()
		{
			AudioControl.Stop();
		}

		public void MidiStop()
		{
			MidiControl.Stop();
		}

		public void Dispose()
		{
			MidiControl.Dispose();
		}
	}
}
