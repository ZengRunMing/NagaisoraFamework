using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NagaisoraFamework
{
	[Serializable]
	public class StoryData
	{
		public List<StoryProgram> StoryProgram;

		public List<StoryFaceInfo> StoryFaceInfos;
		public List<SceneInfo> StorySceneInfos;
	}

	[Serializable]
	public class StoryProgram
	{
		public string StoryName;
		public bool IsEndingStory;
		public Program Program;
	}

	[Serializable]
	public struct SceneInfo
	{
		public string SceneName;
		public Image SceneImage;
	}
}
