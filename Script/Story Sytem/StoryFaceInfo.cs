using System;
using System.Collections.Generic;
using UnityEngine;

namespace NagaisoraFamework
{
	[Serializable]
	public class StoryFaceInfo
	{
		public string FaceName;

		public bool IsLeft;

		public Vector2 MainSize;
		public Sprite MainImage;

		public Vector2 FacePosition;
		public Vector2 NormalFaceSize;
		public List<FaceImageInfo> FaceImageInfos;

		public Dictionary<string, FaceImageInfo> FaceImages;

		public void Init()
		{
			FaceImages = new Dictionary<string, FaceImageInfo>();

			foreach (var faceInfo in FaceImageInfos)
			{
				FaceImages.Add(faceInfo.Name, faceInfo);
			}
		}
	}

	[Serializable]
	public struct FaceImageInfo
	{
		public string Name;
		public Vector2 Size;
		public Sprite Image;
	}
}
