﻿using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;

namespace NagaisoraFramework.Miedia
{
	public class SEManager : CommMonoScriptObject
	{
		public AudioMixerGroup Mixer;

		public float Volume;

		public SEAudio[] SEAudio;

		public IDictionary<string, SEAudio> ACD = new Dictionary<string, SEAudio>();
		public List<AudioSource> AS;

		void Awake()
		{
			MainSystem.SEManager = this;

			Initialization();

			if (SEAudio == null)
			{
				Debug.LogWarning("参数 SEAudio 为空，将无法播放音效");
				return;
			}

			foreach (SEAudio audio in SEAudio)
			{
				if (audio.SEType == SEType.AsName)
				{
					ACD.Add(audio.Name, audio);
					continue;
				}

				ACD.Add(Enum.GetName(typeof(SEType), audio.SEType), audio);
			}
		}

		public void PlaySE(string name)
		{
			if (!ACD.ContainsKey(name))
			{
				return;
			}

			AudioClip clip = ACD[name].AudioClip;

			foreach (AudioSource audio in AS)
			{
				if (!audio.isPlaying)
				{
					audio.clip = clip;
					MainSystem.SetAudioSourceScale(audio, 1f);
					if (ACD[name].TimeScale)
					{
						MainSystem.SetAudioSourceScale(audio, Time.timeScale);
					}

					audio.volume = Volume;
					audio.Play();
					return;
				}
			}

			AddAudioScource(AS.Count - 1);
		}

		public void AddAudioScource(int a)
		{
			GameObject obj = new GameObject()
			{
				name = "SE-" + a.ToString("00")
			};

			AudioSource audio = obj.AddComponent<AudioSource>();

			audio.outputAudioMixerGroup = Mixer;

			AS.Add(audio);
		}

		public void Initialization()
		{
			AS = new List<AudioSource>();

			for (int a = 0; a < 16; a++)
			{
				AddAudioScource(a);
			}
		}

		public void SEReset()
		{
			for (int a = 0; a < 16; a++)
			{
				Destroy(AS[a].gameObject);
			}
			AS.Clear();
			Initialization();
		}
	}

	[Serializable]
	public struct SEAudio
	{
		public SEType SEType;
		public string Name;
		public bool TimeScale;
		public AudioClip AudioClip;

		public SEAudio(AudioClip audioClip, string name = "", bool timescale = false, SEType type = SEType.AsName)
		{
			if (type == SEType.AsName)
			{
				Name = name;
				SEType = SEType.AsName;
			}
			else
			{
				Name = Enum.GetName(typeof(SEType), type);
				SEType = type;
			}
			TimeScale = timescale;
			AudioClip = audioClip;
		}
	}
}