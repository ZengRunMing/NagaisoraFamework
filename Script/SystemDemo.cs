using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Diagnostics = System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;

using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Common;

using UnityEngine;
using UnityEngine.UI;

namespace NagaisoraFamework
{
	using Miedia;
	using Cryptography;
	using DataFileSystem;
	using Underlyingsystem;

	using static MainSystem;
	using NagaisoraFamework.NetworkSystem;
	using System.Net;
	using System.Net.Sockets;
	using NagaisoraFamework.STGSystem;

	public class NagaisoraSystemDemo : CommMonoScriptObject
	{
		public Animation Animation;

		public DiskInfo[] physicalDiskInfos;

		public GameObject RPYATContent;
		public GameObject ActionDataViewer;

		public GameObject ConsoleObj;

		public MessageBox MessageBox;

		public BGMControl BGMControl;
		public MidiPianoFormControl MidiPianoFormControl;

		public int Start_Index = 1;

		public Text audioTimeText;

		public Dropdown AudioTCI;
		public Dropdown MidiTCI;

		public string MidiReadPath;

		public WaveBGMData[] AudioData;
		public WaveBGMData[] ReadAudioData;

		public MidiBGMData[] MidiData;
		public MidiBGMData[] ReadMidiData;

		public MemoryStream SRMDI;

		public Color BackColor;
		public Color FogeColor;
		public GameObject[] MAINObject;
		public Image[] BUTTObject;

		public InputField AudioSave;
		public InputField MIDISave;
		public InputField AudioRead;
		public InputField MIDIRead;

		public InputField Replay_SavePath;
		public InputField ReplayL;
		public InputField ReplayIndex;

		public Dropdown MidiOutDevicesSet;
		public Dropdown MidiInDevicesSet;

		public Image IMG;

		public Text RDKText;

		public Text MIDIRDKText;

		public Text MidiCTL;

		public Text ByteCITUI;

		public OutputDevice MDO;
		public InputDevice MDI;

		public Text AT;
		public Text RS;

		public List<ITimeSpan> timeSpans = new List<ITimeSpan>();
		public List<ITimeSpan> timeSpanc = new List<ITimeSpan>();

		public InputField EXEPATH;
		public Text EXEINFO;

		public Dropdown DriverID;
		public InputField SectorID;

		public Text DiskState;
		public Text DiskCurrentExecution;

		public InputField DriverSavePath;
		public InputField DriverReadPath;
		public InputField RDataSize;

		public Text InputSystemNames;

		public Text XYD;
		public Text IKD;

		DiskStream Disk;

		public InputField ServerIP;
		public InputField ServerPort;
		public InputField ServerListenCount;

		public InputField ClientIP;
		public InputField ClientPort;

		public NetworkServer NetworkServer;
		public NetworkClient NetworkClient;

		public InputField N45ServerIP;
		public InputField N45ServerPort;
		public InputField N45ServerListenCount;

		public InputField N45ClientIP;
		public InputField N45ClientPort;

		public N45Server N45Server;
		public N45Client N45Client;

		public int currentHour;
		public int currentMinute;
		public int currentSecond;
		public int clipHour;
		public int clipMinute;
		public int clipSecond;

		public bool MidiStart = false;

		public bool Packs;

		public bool DiskWriteData;
		public bool DiskReadData;
		public float DPosition;
		public float DLength;

		public RASMActuator RASMActuator;

		Diagnostics.Stopwatch sw;

		public InputModule InputModule;

		public void Awake()
		{
			NetworkServer = new();
			NetworkClient = new();
		}

		public void Start()
		{
			CateList(Start_Index);

			string output = "";

			using (Diagnostics.Process p = new Diagnostics.Process())
			{
				p.StartInfo.FileName = $"{DataPath}\\GetManagentObject.exe";
				p.StartInfo.Arguments = "-d Win32_DiskDrive Win32_DiskPartition";
				p.StartInfo.UseShellExecute = false;
				p.StartInfo.CreateNoWindow = true;
				p.StartInfo.RedirectStandardOutput = true;
				p.Start();

				output = p.StandardOutput.ReadToEnd();

				p.WaitForExit();
			}

			string[] strings = output.Split(new char[] { '\n' });

			List<NagaisoraFameworkPublicClass.ManagementObject> lines = new List<NagaisoraFameworkPublicClass.ManagementObject>();

			foreach (string s in strings)
			{
				if (s == "")
				{
					continue;
				}

				MemoryStream memoryStream = new(Convert.FromBase64String(s));

				BinaryFormatter bf = new();

				lines.Add(bf.Deserialize(memoryStream) as NagaisoraFameworkPublicClass.ManagementObject);
			}

			List<DiskInfo> infos = new();

			DriverID.options.Clear();

			List<Dropdown.OptionData> optionDatas = new();

			foreach (NagaisoraFameworkPublicClass.ManagementObject drive in lines)
			{
				DiskInfo info;

				if (drive.GetValue("CreationClassName").ToString() == WMIPath.Win32_DiskDrive.ToString())
				{
					info = new DiskInfo(
						(string)drive.GetValue("Name"), (string)drive.GetValue("Model"), drive.GetValue("SerialNumber").ToString().TrimEnd('.'),
						(string)drive.GetValue("interfaceType"), (string)drive.GetValue("Description"), Convert.ToInt64(drive.GetValue("Size")), (string)drive.GetValue("Status"));
				}
				else
				{
					info = new DiskInfo((string)drive.GetValue("Name"), (string)drive.GetValue("Description"), Convert.ToInt64(drive.GetValue("Size")), (string)drive.GetValue("Status"));
				}
				infos.Add(info);

				Dropdown.OptionData A0 = new Dropdown.OptionData
				{
					text = info.ToString(),
				};

				optionDatas.Add(A0);
			}

			DriverID.options = optionDatas;

			physicalDiskInfos = infos.ToArray();

			string[] MOT = Miedia.MidiControl.ListMidiOutDevices();

			Dropdown.OptionData OPT0 = new()
			{
				text = "None",
			};
			MidiOutDevicesSet.options.Add(OPT0);

			for (int MT = 0; MT < MOT.Length; MT++)
			{
				Dropdown.OptionData OPT = new()
				{
					text = MOT[MT]
				};
				MidiOutDevicesSet.options.Add(OPT);
			}

			string[] MIN = Miedia.MidiControl.ListMidiInDevices();

			for (int MT = 0; MT < MIN.Length; MT++)
			{
				Dropdown.OptionData OPT = new Dropdown.OptionData();
				OPT.text = MIN[MT];
				MidiInDevicesSet.options.Add(OPT);
			}

			MidiCTL.text = "状态 : 无文件播放";

			//StartCoroutine(TimeUpdate());

			string[] JoystickNames = Input.GetJoystickNames();

			InputSystemNames.text = "";

			foreach (string JoystickName in JoystickNames)
			{
				InputSystemNames.text += $"{JoystickName}\n";
			}
		}

		public void Update()
		{
			Vector2 vector = InputModule.AxisVector;

			XYD.text = $"X:{vector.x} Y:{vector.y}";

			IKD.text = "";

			if (DiskWriteData)
			{
				if (DPosition >= DLength - 1)
				{
					DiskCurrentExecution.text = string.Format("当前执行 : 写入数据 当前{0}% 完成", (100f).ToString("00.000"));
					DiskWriteData = false;
					sw.Stop();
					TimeSpan ts = sw.Elapsed;
					Debug.Log($"写入数据 耗时 {ts.TotalMilliseconds} ms");
				}
				else
				{
					DiskCurrentExecution.text = string.Format("{0}% {1} {2}", ((DPosition / DLength) * 100f).ToString("00.000"), DPosition.ToString("0"), DLength.ToString("0"));
				}
			}

			if (DiskReadData)
			{
				if (DPosition >= DLength - 1)
				{
					DiskCurrentExecution.text = string.Format("当前执行 : 读取数据 当前{0}% 完成", (100f).ToString("00.000"));
					DiskWriteData = false;
					sw.Stop();
					TimeSpan ts = sw.Elapsed;
					Debug.Log($"读取数据 耗时 {ts.TotalMilliseconds} ms");
				}
				else
				{
					DiskCurrentExecution.text = string.Format("{0}% {1} {2}", ((DPosition / DLength) * 100f).ToString("00.000"), DPosition.ToString("0"), DLength.ToString("0"));
				}
			}

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (Time.timeScale != 0)
				{
					Time.timeScale = 0;
				}
				else
				{
					Time.timeScale = 1;
				}
			}

			if (BGMControl.MidiControl.Playback != null)
			{
				if (AT != null)
				{
					AT.text = $"{BGMControl.MidiControl.CurrentTime(TimeSpanType.Metric)} / {BGMControl.MidiControl.DurationTime(TimeSpanType.Metric)}";
				}
			}

			if (BGMControl.MidiIsPlaying)
			{
				MidiCTL.text = "状态 : 播放中";
			}
			else
			{
				MidiCTL.text = "状态 : 播放停止";
			}

			if (BGMControl.AudioControl.AudioSource.isPlaying)
			{
				clipHour = (int)BGMControl.AudioControl.AudioSource.clip.length / 3600;
				clipMinute = (int)(BGMControl.AudioControl.AudioSource.clip.length - clipHour * 3600) / 60;
				clipSecond = (int)(BGMControl.AudioControl.AudioSource.clip.length - clipHour * 3600 - clipMinute * 60);
				currentHour = (int)BGMControl.AudioControl.AudioSource.time / 3600;
				currentMinute = (int)(BGMControl.AudioControl.AudioSource.time - currentHour * 3600) / 60;
				currentSecond = (int)(BGMControl.AudioControl.AudioSource.time - currentHour * 3600 - currentMinute * 60);
				audioTimeText.text = string.Format("{0:D2}:{1:D2}:{2:D2} / {3:D2}:{4:D2}:{5:D2}", currentHour, currentMinute, currentSecond, clipHour, clipMinute, clipSecond);
			}
			else
			{
				audioTimeText.text = string.Format("{0:D2}:{1:D2}:{2:D2} / {3:D2}:{4:D2}:{5:D2}", 0, 0, 0, 0, 0, 0);
			}
		}

		public void FixedUpdate()
		{
			RASMActuator?.Time_CLK(FixedGameTime);

			GolbalReplaySystem?.OnUpdate();
		}

		public void StartReplayRecord()
		{
			GolbalReplaySystem.RecordStart();
		}

		public void StopReplayRecord()
		{
			GolbalReplaySystem.RecordStop();

			int count = RPYATContent.transform.childCount;

			for (int i = 0; i < count; i++)
			{
				Destroy(RPYATContent.transform.GetChild(i).gameObject);
			}

			foreach (ReplayActionData data in GolbalReplaySystem.ActionDatas)
			{
				GameObject Object = Instantiate(ActionDataViewer);
				Object.SetActive(false);

				Object.transform.SetParent(RPYATContent.transform);

				RectTransform rect = Object.GetComponent<RectTransform>();
				
				rect.localScale = Vector3.one;

				ActionDataViwer actionDataViwer = Object.GetComponent<ActionDataViwer>();

				actionDataViwer.data = data;

				Object.SetActive(true);
			}
		}

		public void SaveReplayButtonOnclick()
		{
			if (GolbalReplaySystem.ActionDatas == null || GolbalReplaySystem.ActionDatas.Length == 0)
			{
				Debug.LogError(new ArgumentNullException("ActionData为空, 无法保存Replay文件"));
				return;
			}

			ReplayData ReplayData = new()
			{
				Name = "System",
				SaveTime = DateTime.Now,
				Score = int.MaxValue,
				Player = 0,
				Livel = 1,
				EndStage = 2,
				User = "Default",
				EndLife = 2,
				EndBomb = 3,
				Power = 4,
				Point = 10000,
				Graze = 5000,
				GetSpellCardCount = 5,
				DeathCount = 1
			};

			List<StageReplayData> actionDataCollections = new();

			StageReplayData actionDataCollection = new("St?", RandomInt());

			foreach (ReplayActionData actionData in GolbalReplaySystem.ActionDatas)
			{
				actionDataCollection.AddAction("default", actionData);
			}

			actionDataCollections.Add(actionDataCollection);

			ReplayData.StageReplayDatas = actionDataCollections.ToArray();

			ReplayDataSystem.SaveReplay($"{GetDataPath().TrimEnd(new char[] { '/', '\\' })}/Replay/{SCLENAME}_Data.RPY", ReplayData);
		}

		public void SetData()
		{
			AudioData = MainSystem.WaveBGMData;
		}

		public void AudioFilePacket()
		{
			if (File.Exists(AudioSave.text + "\\thbgm.dat"))
			{

			}
			else
			{
				StartCoroutine(WaveBGMDataPack.WriteWaveBGMData(AudioData, AudioSave.text, (value) => RDKText.text = value));
			}
		}

		public void MidiFilePacket()
		{
			if (File.Exists(DataPath + "\\" + MIDISave.text))
			{

			}
			else
			{
				for (int i = 0; i < MidiData.Length; i++)
				{
					FileStream FS = new(MidiData[i].DataPath, FileMode.Open, FileAccess.Read);
					BinaryReader BR = new(FS);

					BR.BaseStream.Seek(0x00, SeekOrigin.Begin);

					byte[] vs = BR.ReadBytes(Convert.ToInt32(BR.BaseStream.Length));

					MidiData[i].Data = new(vs);

					BR.Close();
					FS.Close();
				}

				StartCoroutine(MidiBGMDataPack.WriteMidiBGMData(MidiData, DataPath + "\\" + MIDISave.text, (value) => MIDIRDKText.text = value));
			}
		}

		public void SetPhysicalDriver()
		{
			try
			{
				Disk = new DiskStream(physicalDiskInfos[DriverID.value], 2048);
				DiskState.text = "状态 : 打开并已实例化";
			}
			catch (Exception e)
			{
				MessageBox.Show("错误", e.Message, MessageBox.ButtonStyle.OK, MessageBox.IconStyle.None);
			}
		}

		public void ClosePhysicalDriver()
		{
			Disk.Close();
			DiskState.text = "状态 : 关闭";
		}

		public void ReadSector()
		{
			string Path = DataPath + "\\" + DriverReadPath.text;

			long Sti = Convert.ToInt64(SectorID.text);
			long Size = Convert.ToInt64(RDataSize.text);

			sw = new Diagnostics.Stopwatch();

			sw.Start();

			Task.Run(() =>
			{
				byte[] mf;
				mf = Disk.ReadData(Sti, Size, out DPosition);
				FileStream fileStream = new(Path, FileMode.Create, FileAccess.Write);
				fileStream.Write(mf, 0x00, mf.Length);
				fileStream.Close();
			});

			DLength = Size;

			DiskReadData = true;
		}

		public void WriteSector()
		{
			FileStream fileStream = new(DataPath + "\\" + DriverSavePath.text, FileMode.Open, FileAccess.Read);
			byte[] mf = new byte[fileStream.Length];
			fileStream.Read(mf, 0x00, mf.Length);
			fileStream.Close();

			long Sti = Convert.ToInt64(SectorID.text);

			Task.Run(() =>
			{
				Disk.WriteData(Sti, mf, out DPosition);
				//Disk.WriteData(mf, out DPosition);
			});

			DLength = (float)mf.Length;

			DiskWriteData = true;
		}

		public void CateList(int Index)
		{
			for (int i = 0; i < MAINObject.Length; i++)
			{
				if (i == Index)
				{
					MAINObject[i].SetActive(true);
					BUTTObject[i].color = FogeColor;
				}
				else
				{
					MAINObject[i].SetActive(false);
					BUTTObject[i].color = BackColor;
				}
			}
		}

		public void AudioFileRead()
		{
			ReadAudioData = MainSystem.WaveBGMData;

			AudioTCI.options.Clear();

			List<Dropdown.OptionData> optionDatas = new();

			for (int i = 0; i < ReadAudioData.Length; i++)
			{
				Dropdown.OptionData A0 = new Dropdown.OptionData
				{
					text = ReadAudioData[i].Data.name,
				};

				optionDatas.Add(A0);
			}

			AudioTCI.AddOptions(optionDatas);

			AudioTCI.value = 0;
		}

		public void MidiFileRead()
		{
			if (MIDIRead.text == "")
			{
				MidiPackRead();
				return;
			}

			FileStream FS = new(MIDIRead.text, FileMode.Open, FileAccess.Read);
			BinaryReader binaryReader = new(FS, Encoding.UTF8);

			string PT = Path.GetExtension(FS.Name);

			if (PT == ".dat")
			{
				MidiPackRead();
			}
			else if (PT == ".mid")
			{
				int length = Convert.ToInt32(binaryReader.BaseStream.Length);
				SRMDI = new(binaryReader.ReadBytes(length));
				Packs = false;
				MidiCTL.text = "状态 : 读取完成";
			}
			else
			{
				Debug.Log("未知文件, 无法加载");
			}
		}

		public void MidiPackRead()
		{
			//TextAsset BTC = Resources.Load("NagaisoraNativeReset") as TextAsset;

			//SRMDI = new(BTC.bytes);

			//PlayMidi(false);

			ReadMidiData = MidiBGMDataPack.ReadMidiBGMData(MIDIRead.text);

			MidiTCI.options.Clear();

			List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();

			for (int i = 0; i < ReadMidiData.Length; i++)
			{
				Dropdown.OptionData A0 = new Dropdown.OptionData
				{
					text = ReadMidiData[i].Name,
				};

				optionDatas.Add(A0);
			}

			MidiTCI.AddOptions(optionDatas);

			BGMControl.MidiBGMData = ReadMidiData;

			MidiCTL.text = "状态 : 读取完成";
			Packs = true;
			MidiTCI.value = 0;
		}

		public void EXELOAD()
		{
			if (EXEPATH.text != "")
			{
				FileStream FS = new(EXEPATH.text, FileMode.Open, FileAccess.Read);
				BinaryReader binaryReader = new(FS, Encoding.UTF8);
				long Len = binaryReader.BaseStream.Length;
				RASMActuator = new RASMActuator(binaryReader.ReadBytes(Convert.ToInt32(Len)));
				EXEINFO.text = RASMActuator.executable.Name + " | " + RASMActuator.executable.Type;
				RASMActuator.RUN(gameObject);
			}
		}

		public void ShowPianoWindow()
		{
			Animation.Play("Show");
		}

		public void HidePianoWindow()
		{
			Animation.Play("Hide");
		}

		public void AudioStart()
		{
			BGMControl.SelectWaveBGMDataOfIndex(AudioTCI.value);
			BGMControl.WavePlay();
		}

		public void AudioStop()
		{
			BGMControl.WaveStop();
		}

		public void PlayMidi()
		{
			PlayMidi(Packs);
		}

		public void PlayMidi(bool packs)
		{
			if (packs)
			{
				if (ReadMidiData[MidiTCI.value] != null)
				{
					BGMControl.SelectMidiBGMDataOfIndex(MidiTCI.value);
				}
				else
				{
					Debug.LogError("ReadMidiData Is Null");
				}
			}
			else
			{
				BGMControl.MidiControl.SetMidiData(SRMDI);
			}

			try
			{
				StopMidi();

				SetMidiDevice();

				BGMControl.MidiControl.SetOutputDevice(BGMControl.MidiOutputDeviceID);

				BGMControl.MidiPlay();

				MidiPianoFormControl.Init();

				MidiStart = true;
			}
			catch (Exception E)
			{
				Debug.LogError(E);
			}
		}

		public void StopMidi()
		{
			BGMControl.MidiStop();
			MidiStart = false;
		}

		public void StartMidiListen()
		{
			SetMidiDevice();
			MDI.EventReceived += OnEventReceived;
			MDI.StartEventsListening();
		}

		public void StopMidiListen()
		{
			Miedia.MidiControl.DisposeOutputDevice(MDO);
			Miedia.MidiControl.DisposeInputDevice(MDI);
		}

		public void OnApplicationQuit()
		{
			Miedia.MidiControl.DisposeOutputDevice(MDO);
			Miedia.MidiControl.DisposeInputDevice(MDI);
		}

		public void Quit()
		{
			MainSystem.Quit();
		}

		public void SetMidiDevice()
		{
			MDO = Miedia.MidiControl.SetMidiOutputDevice(MidiOutDevicesSet.value - 1);
			BGMControl.MidiOutputDeviceID = MidiOutDevicesSet.value - 1;

			MDI = Miedia.MidiControl.SetMidiInputDevice(MidiInDevicesSet.value);
		}

		public void OnEventReceived(object sender, MidiEventReceivedEventArgs e)
		{
			MidiEvent Event = e.Event;

			if (Event.EventType == MidiEventType.NoteOn)
			{
				string[] ma = Event.ToString().Split(new char[] { ' ', '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);

				Note note = new ((SevenBitNumber)Convert.ToInt32(ma[3]))
				{
					Velocity = (SevenBitNumber)Convert.ToInt32(ma[4]),
					Channel = new FourBitNumber(0),
				};

				foreach (string s in ma)
				{
					Debug.Log(s.ToString());
				}
			}

			MDO.SendEvent(Event);
		}

		public void ServerStart()
		{
			NetworkServer.IPAddress = IPAddress.Parse(ServerIP.text);
			NetworkServer.Port = Convert.ToInt32(ServerPort.text);
			NetworkServer.ListenCount = Convert.ToInt32(ServerListenCount.text);

			NetworkServer.Accept += (Socket) => { Debug.Log($"[Server] 连接对象数据 {Socket.RemoteEndPoint} "); };
			NetworkServer.Receive += (Socket, buffer) => { Debug.Log($"[Server] {BitConverter.ToString(buffer).Replace('-', ' ')}"); };

			NetworkServer.StartSocket();
		}

		public void ServerStop()
		{
			NetworkServer.CloseSocket();
		}


		public void N45ServerStart()
		{
			Debug.Log($"[N45Server] 开始监听");

			N45Server = new()
			{
				IPAddress = IPAddress.Parse(N45ServerIP.text),
				Port = Convert.ToInt32(N45ServerPort.text),
				ListenCount = Convert.ToInt32(N45ServerListenCount.text),
				BufferSize = 1250000
			};

			N45Server.Accept += (message) => { Debug.Log($"[N45Server] {message}"); };
			//N45Server.Receive += (Socket, buffer) => { Debug.Log($"[N45Server] {BitConverter.ToString(buffer).Replace('-', ' ')}"); };

			N45Server.Start();
		}

		public void N45ServerStop()
		{
			N45Server.Close();
		}

		public void ClientStart()
		{
			NetworkClient.IPAddress = IPAddress.Parse(ClientIP.text);
			NetworkClient.Port = Convert.ToInt32(ClientPort.text);
			NetworkClient.Timeout = 1000;

			NetworkClient.Receive += (Socket, buffer) => { Debug.Log($"[Client] {BitConverter.ToString(buffer).Replace('-', ' ')}"); };

			NetworkClient.StartSocket();

			if (!NetworkClient.IsConnected)
			{
				Debug.Log("连接失败");
				return;
			}

			Debug.Log("连接成功");

			NetworkClient.Socket.Send(Encoding.UTF8.GetBytes("HELLO WORLD!"));
		}

		public void ClientStop()
		{
			NetworkClient.CloseSocket();
		}

		public void N45ClientStart()
		{
			N45Client = new()
			{
				IPAddress = IPAddress.Parse(N45ClientIP.text),
				Port = Convert.ToInt32(N45ClientPort.text),
				Timeout = 1000,
				BufferSize = 1250000
			};

			N45Client.Receive += (message) => { Debug.Log($"[N45Client] {message}"); };

			N45Client.Start();
		}

		public void N45ClientStop()
		{
			N45Client.Close();
		}

		public void ConsoleShow()
		{
			ConsoleObj.SetActive(true);
		}

		public void ConsoleHide()
		{
			ConsoleObj.SetActive(false);
		}

		public IEnumerator TimeUpdate()
		{
			while (true)
			{

				yield return null;
			}
		}
	}
}
