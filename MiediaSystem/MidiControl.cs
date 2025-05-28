﻿using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

namespace NagaisoraFamework.Miedia
{
	public class MidiControl : CommMonoScriptObject
	{
		//public int OutputDeviceID = 0;

		public MidiFile MidiFile;
		public Playback Playback;
		public OutputDevice OutputDevice;

		public TimeSpan PlayStartTime;
		public TimeSpan LoopStartTime;
		public TimeSpan LoopEndTime;

		public ICollection<Note> MidiNotes;

		public TimeSpan CurrentTime(TimeSpanType type)
		{
			if (Playback == null)
			{
				return TimeSpan.Zero;
			}

			return Playback.GetDuration(type) as MetricTimeSpan;
		}

		public TimeSpan DurationTime(TimeSpanType type)
		{
			if (Playback == null)
			{
				return TimeSpan.Zero;

			}

			return Playback.GetDuration(type) as MetricTimeSpan;
		}

		public void Awake()
		{
			MainSystem.MidiControl = this;
		}

		public void Update()
		{
			if (Playback != null)
			{
				TimeSpan Time = CurrentTime(TimeSpanType.Metric);

				if (LoopEndTime.Ticks < LoopStartTime.Ticks)
				{
					Playback.Loop = true;
				}
				else
				{
					Playback.Loop = false;
					if (LoopEndTime.Ticks != 0)
					{
						if (Time.Ticks >= LoopEndTime.Ticks)
						{
							Playback.Stop();
							Playback.MoveToTime((MetricTimeSpan)LoopStartTime);
							Playback.Start();
						}
					}
				}
			}
		}

		public static InputDevice SetMidiInputDevice(int DeviceID)
		{
			InputDevice inputDevice = InputDevice.GetByIndex(DeviceID);
			return inputDevice;
		}

		public static OutputDevice SetMidiOutputDevice(int DeviceID)
		{
			if (DeviceID < 0)
			{
				return null;
			}

			OutputDevice outputDevice = OutputDevice.GetByIndex(DeviceID);
			return outputDevice;
		}

		public void SetOutputDevice(int DeviceID)
		{
			OutputDevice?.Dispose();

			if (DeviceID < 0)
			{
				OutputDevice = null;
				return;
			}

			OutputDevice = SetMidiOutputDevice(DeviceID);
		}

		public void SetOutputDevice(string name)
		{
			OutputDevice?.Dispose();

			if (name == "")
			{
				OutputDevice = null;
				return;
			}

			OutputDevice = OutputDevice.GetByName(name);
		}

		public static void DisposeOutputDevice(OutputDevice device)
		{
			device?.Dispose();
		}

		public static void DisposeInputDevice(InputDevice device)
		{
			device?.Dispose();
		}

		public static string[] ListMidiInDevices()
		{
			List<string> MIN = new List<string>();
			foreach (InputDevice inputDevice in InputDevice.GetAll())
			{
				MIN.Add(inputDevice.Name);
			}
			return MIN.ToArray();
		}

		public static OutputDevice[] ListMidiOutDevices()
		{
			List<OutputDevice> MOT = new List<OutputDevice>();
			foreach (OutputDevice outputDevice in OutputDevice.GetAll())
			{
				MOT.Add(outputDevice);
			}
			return MOT.ToArray();
		}

		public static string[] ListMidiOutDeviceNames()
		{
			List<string> MOT = new List<string>();
			foreach (OutputDevice outputDevice in OutputDevice.GetAll())
			{
				MOT.Add(outputDevice.Name);
			}
			return MOT.ToArray();
		}

		public void SetMidiFile(string Name)
		{
			MidiFile = null;
			MidiFile = MidiFile.Read(Name);
			MidiNotes = MidiFile.GetNotes();
		}

		public void SetMidiData(MidiBGMData data)
		{
			try
			{
				PlayStartTime = data.StartTime;
				LoopStartTime = data.LoopStartTime;
				LoopEndTime = data.LoopEndTime;
				SetMidiData(data.Data);
			}
			catch (Exception Err)
			{
				throw Err;
			}
		}

		public void SetMidiData(MemoryStream Stream)
		{
			MidiFile = null;

			ReadingSettings readingSettings = new ReadingSettings()
			{
				InvalidChannelEventParameterValuePolicy = InvalidChannelEventParameterValuePolicy.SnapToLimits,
				TextEncoding = Encoding.GetEncoding("Shift-JIS"),
			};

			MemoryStream memory = new MemoryStream(Stream.ToArray());
			MidiFile = MidiFile.Read(memory, readingSettings);

			MidiNotes = MidiFile.GetNotes();
		}

		public void Play()
		{
			CreatePlayback();

			Playback?.Start();
		}

		public void CreatePlayback()
		{
			if (Playback == null)
			{
				Playback = MidiFile.GetPlayback();
				Playback.OutputDevice = OutputDevice;
				Playback.Speed = 1f;
				Playback.Loop = false;
			}
		}

		public void Stop()
		{
			Playback?.Stop();
			Playback?.Dispose();
			Playback = null;

			OutputDevice?.Dispose();
		}

		public void Pause()
		{
			Playback?.Stop();
		}

		public void Dispose()
		{
			Playback?.Stop();
			Playback?.Dispose();
			Playback = null;
			OutputDevice?.Dispose();
			MidiFile = null;
		}
	}
}
