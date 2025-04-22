using System;
using System.Collections.Generic;
using System.IO;

namespace NagaisoraFamework
{
	public static class FileHelper
	{
		/// <summary>
		/// 获得目录下所有文件或指定文件类型文件(包含所有子文件夹)
		/// </summary>
		/// <param name="path">文件夹路径</param>
		/// <param name="extName">扩展名可以多个 例如 .mp3.wma.rm</param>
		/// <returns>List<FileInfo></returns>
		public static void getFile(string path, string extName, ref List<FileInfo> lst)
		{
			try
			{
				if (lst == null)
				{
					lst = new List<FileInfo>();
				}

				string[] dir = Directory.GetDirectories(path); //文件夹列表   
				DirectoryInfo fdir = new DirectoryInfo(path);
				FileInfo[] file = fdir.GetFiles();
				//FileInfo[] file = Directory.GetFiles(path); //文件列表   
				if (file.Length != 0 || dir.Length != 0) //当前目录文件或文件夹不为空                   
				{
					foreach (FileInfo f in file) //显示当前目录所有文件   
					{
						if (extName.ToLower().IndexOf(f.Extension.ToLower()) >= 0)
						{
							lst.Add(f);
						}
					}
					foreach (string d in dir)
					{
						getFile(d, extName, ref lst);//递归
					}
				}
				return;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static (string, string, string) getFileName(string path)
		{
			string FilePath = path; //文件路径
			string FileName = Path.GetFileName(FilePath); //全文件名
			string LSRCFileName = Path.GetExtension(FilePath); //拓展名

			return (FilePath, FileName, LSRCFileName);
		}
	}
}
