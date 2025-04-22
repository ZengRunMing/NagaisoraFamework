using System.Collections;
using System.Text;
using System.IO;
using System;

using AOT;

using UnityEngine;

namespace NagaisoraFamework.Miedia
{
    using static MainSystem;

    public static class AudioClipConvert
    {
        public static byte[] ConvertClipToBytes(AudioClip audioClip)
        {
            float[] samples = new float[audioClip.samples * 2];
            audioClip.GetData(samples, 0);

            short[] intData = new short[samples.Length];
            byte[] bytesData = new byte[samples.Length * 2];

            for (int i = 0; i < samples.Length; i++)
            {
                intData[i] = (short)(samples[i] * (short.MaxValue / 1.5));
                byte[] byteArr = BitConverter.GetBytes(intData[i]);
                byteArr.CopyTo(bytesData, i * 2);
            }

            return bytesData;
        }

        public static AudioClip ConvertBytesToClip(byte[] rawData, string WaweName, int samplerate)
        {
            float[] samples = new float[rawData.Length / 2];
            AudioClip audioClip = AudioClip.Create(WaweName, samples.Length / 2, 2, samplerate, false);

            for (int i = 0; i < samples.Length; i++)
            {
                short st = BitConverter.ToInt16(rawData, i * 2);
                float ft = st / (float)(short.MaxValue / 1.5);
                samples[i] = ft;
            }

            audioClip.SetData(samples, 0);

            return audioClip;
        }
    }

    public class WaveBGMDataPack
	{
        public static IEnumerator WriteWaveBGMData(WaveBGMData[] datas, string Path, Action<string> CallBack)
        {
            if (Path == null || Path == "")
            {
                Path =DataPath + "\\thwav.dat";
            }

            CallBack("CreateFile");
            yield return null;

            FileStream FS = new(Path, FileMode.Create, FileAccess.Write);
            MemoryStream MS = new();
            MemoryStream MS2 = new();

            BinaryWriter BW = new(FS);
            BinaryWriter BWM = new(MS);
            BinaryWriter BWM2 = new(MS2);

            CallBack("SetPoint");
            yield return null;

            BWM.BaseStream.Seek(0x00, SeekOrigin.Begin);
            BWM2.BaseStream.Seek(0x00, SeekOrigin.Begin);

            BWM2.Write(Encoding.UTF8.GetBytes(string.Format("{0}\\WAVE", SCLENAME)));

            BWM2.Write(datas.Length);

            for (long i = 0; i < datas.Length; i++)
            {
                CallBack($"Write {i+1}/{datas.Length}");

                byte[] ASPBL = AudioClipConvert.ConvertClipToBytes(datas[i].Data);

				BWM2.Write($"{SCLENAME}_{i:00}.wav");
                BWM2.Write(datas[i].Name);
                
                BWM2.Write(datas[i].Text);
				BWM2.Write(Convert.ToInt32(BWM.BaseStream.Position));
				BWM2.Write(ASPBL.Length);
				BWM2.Write(datas[i].Data.frequency);
				BWM2.Write(datas[i].StartTime.Ticks);
				BWM2.Write(datas[i].LoopStartTime.Ticks);
				BWM2.Write(datas[i].LoopEndTime.Ticks);

				BWM.Write(ASPBL);
                yield return null;
            }

            BW.Write(MS2.ToArray());
            BW.Write(MS.ToArray());

            BW.Close();
            FS.Close();
            BWM2.Close();
            BWM.Close();
            MS2.Close();
            MS.Close();

            BW.Dispose();
			FS.Dispose();
			BWM2.Dispose();
			BWM.Dispose();
			MS2.Dispose();
			MS.Dispose();

			CallBack("Done");

            yield break;
        }

        public static WaveBGMData[] ReadWaveBGMData(string Path)
        {
            if (Path == null || Path == "")
            {
                Path = DataPath + "\\thwav.dat";
            }

            if (!File.Exists(Path))
            {
                throw new FileNotFoundException($"文件{Path}不存在");
            }

            string[] ECM;
			int[] Posion;
            int[] Size;
            int[] udi;

            FileStream FS = new(Path, FileMode.Open, FileAccess.Read);
            BinaryReader BR = new(FS, Encoding.UTF8);

            try
            {
                BR.BaseStream.Seek(0x00, SeekOrigin.Begin);
            }
            catch (Exception E)
            {
                throw E;
            }
            string HIDE = Encoding.UTF8.GetString(BR.ReadBytes(SCLENAME.Length + 5));

            if (HIDE != string.Format("{0}\\WAVE", SCLENAME))
            {
                throw new Exception("文件头不正确");
            }

            int Length = BR.ReadInt32();

            WaveBGMData[] wavBGMDatas = new WaveBGMData[Length];

            ECM = new string[Length];
			Posion = new int[Length];
            Size = new int[Length];
            udi = new int[Length];

            for (long i = 0; i < Length; i++)
            {
                BR.ReadString();
				ECM[i] = BR.ReadString();

                wavBGMDatas[i] = new()
                {
                    Name = ECM[i],
                    Text = BR.ReadString(),
                };

				Posion[i] = BR.ReadInt32();
                Size[i] = BR.ReadInt32();
                udi[i] = BR.ReadInt32();

                wavBGMDatas[i].StartTime = TimeSpan.FromTicks(BR.ReadInt64());
                wavBGMDatas[i].LoopStartTime = TimeSpan.FromTicks(BR.ReadInt64());
                wavBGMDatas[i].LoopEndTime = TimeSpan.FromTicks(BR.ReadInt64());
            }

            long MWPosion = BR.BaseStream.Position;
            for (long i = 0; i < Length; i++)
            {
                BR.BaseStream.Seek((MWPosion + Posion[i]), SeekOrigin.Begin);
                byte[] AM = BR.ReadBytes(Size[i]);
                wavBGMDatas[i].Data = AudioClipConvert.ConvertBytesToClip(AM, ECM[i], udi[i]);
            }

            BR.Close();
            FS.Close();
            return wavBGMDatas;
        }

        public static FileInfo[] ListDirectoryAllFile(string Path)
        {
            DirectoryInfo direction = new DirectoryInfo(Path);
            return direction.GetFiles("*.wav", SearchOption.AllDirectories);
        }
    }

    public class MidiBGMDataPack
	{
        public static IEnumerator WriteMidiBGMData(MidiBGMData[] datas, string Path, Action<string> CallBack)
        {
			if (Path == null || Path == "")
			{
				Path = DataPath + "\\thmid.dat";
			}

			CallBack("CreateFile");
			yield return null;

            FileStream FS = new(Path, FileMode.Create, FileAccess.Write);
            MemoryStream MS = new();
            MemoryStream MS2 = new();

            BinaryWriter BW = new(FS);
            BinaryWriter BWM = new(MS);
            BinaryWriter BWM2 = new(MS2);

			CallBack("SetPoint");
			yield return null;

            BWM.BaseStream.Seek(0x00, SeekOrigin.Begin);
            BWM2.BaseStream.Seek(0x00, SeekOrigin.Begin);

            BWM2.Write(Encoding.UTF8.GetBytes(string.Format("{0}\\MIDI", SCLENAME)));

            BWM2.Write(Convert.ToInt32(datas.Length));

            for (long i = 0; i < datas.Length; i++)
            {
				CallBack($"Write {i + 1}/{datas.Length}");
				byte[] ASPBL = datas[i].Data.ToArray();
                BWM2.Write("THSSS_" + i.ToString("00") + ".mid");
                BWM2.Write(datas[i].Name);
                BWM2.Write(datas[i].Text);
                BWM2.Write(Convert.ToInt32(BWM.BaseStream.Position));
                BWM.Write(ASPBL);
                BWM2.Write(Convert.ToInt32(ASPBL.Length));

                BWM2.Write(datas[i].StartTime.Ticks);
                BWM2.Write(datas[i].LoopStartTime.Ticks);
                BWM2.Write(datas[i].LoopEndTime.Ticks);

                yield return null;
            }

            BW.Write(MS2.ToArray());
            BW.Write(MS.ToArray());

            BW.Close();
            FS.Close();
            BWM2.Close();
            BWM.Close();
            MS2.Close();
            MS.Close();

            CallBack("Done");

            yield break;
        }

        public static MidiBGMData[] ReadMidiBGMData(string Path)
        {
            if (Path == null || Path == "")
            {
                Path = DataPath + "\\thmid.dat";
            }

			if (!File.Exists(Path))
			{
				throw new FileNotFoundException($"文件{Path}不存在");
			}

			FileStream FS = new(Path, FileMode.Open, FileAccess.Read);
            BinaryReader FBR = new(FS);
            FBR.BaseStream.Seek(0x00, SeekOrigin.Begin);

            string HIDE = Encoding.UTF8.GetString(FBR.ReadBytes(SCLENAME.Length + 5));
            if (HIDE != string.Format("{0}\\MIDI", SCLENAME))
            {
                throw new Exception("文件头不正确");
            }

            string UC;

            int Length = FBR.ReadInt32();

            int[] MDIPosion = new int[Length];
            int[] MDISize = new int[Length];

            MidiBGMData[] midiBGMDatas = new MidiBGMData[Length];


            for (long i = 0; i < Length; i++)
            {
                UC = FBR.ReadString();

                midiBGMDatas[i] = new MidiBGMData();

                midiBGMDatas[i].Name = FBR.ReadString();
				midiBGMDatas[i].Text = FBR.ReadString();

				MDIPosion[i] = FBR.ReadInt32();
                MDISize[i] = FBR.ReadInt32();

                midiBGMDatas[i].StartTime = new TimeSpan(FBR.ReadInt64());
				midiBGMDatas[i].LoopStartTime = new TimeSpan(FBR.ReadInt64());
				midiBGMDatas[i].LoopEndTime = new TimeSpan(FBR.ReadInt64());
			}

            MemoryStream DataMemory = new(FBR.ReadBytes(Convert.ToInt32(FBR.BaseStream.Length)));
            BinaryReader DBR = new(DataMemory);

			FBR.Close();
			FS.Close();

			for (int i = 0; i < Length; i++)
            {
                DBR.BaseStream.Seek(MDIPosion[i], SeekOrigin.Begin);
				midiBGMDatas[i].Data = new(DBR.ReadBytes(Convert.ToInt32(MDISize[i])));
            }

            DBR.Close();
            DataMemory.Close();

            return midiBGMDatas;
        }
    }
}