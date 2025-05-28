﻿using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NagaisoraFamework.DataFileSystem
{
	using static MainSystem;

	public static class ReplayDataSystem
	{
		public static ReplayData[] LoadReplayDatas(string Path)
		{
			if (Path == null || Path == "")
			{
				Path = $"{DataPath}\\Replay\\";
			}

			if (!Directory.Exists(Path))
			{
				throw new DirectoryNotFoundException("LoadReplayDatas() => 未找到Replay文件夹");
			}

			DirectoryInfo direction = new DirectoryInfo(Path);

			FileInfo[] file = direction.GetFiles($"{Name}_*.rpy", SearchOption.AllDirectories);

			List<ReplayData> ReplayDatas = new List<ReplayData>();

			for (int i = 0; i < file.Length; i++)
			{
				ReplayDatas.Add(LoadReplayData(file[i].FullName));
			}

			return ReplayDatas.ToArray();
		}

		public static ReplayData LoadReplayData(string Path)
		{
			return ReplayData.FromBinary(File.ReadAllBytes(Path));
		}

		public static void SaveReplay(string Path, ReplayData STL)
		{
			FileStream FS = new FileStream(Path, FileMode.CreateNew, FileAccess.Write);
			BinaryWriter BWF = new BinaryWriter(FS, Encoding.UTF8);

			byte[] binary = STL.ToBinary();
			BWF.Write(binary);

			BWF.Close();
			FS.Close();
		}
	}
}