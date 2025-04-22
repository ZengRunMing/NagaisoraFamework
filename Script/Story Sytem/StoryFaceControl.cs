using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace NagaisoraFamework
{
    public class StoryFaceControl : CommMonoScriptObject
	{
        public List<StoryFaceInfo> FaceInfos;

		public Dictionary<string, StoryFaceBox> FaceBoxs;

		public GameObject Prefabricate;

		public GameObject LeftFaceMain;
		public GameObject RightFaceMain;

		public void Init()
        {
            FaceBoxs = new Dictionary<string, StoryFaceBox>();
        }

		public void SetFace(string first, string last)
		{
			if (FaceBoxs.Count != 0)
			{
				foreach (StoryFaceBox box in FaceBoxs.Values)
				{
					//box.SetImageColor(DisableColor);

					if (!box.Disabled)
					{
						box.Disable();
					}
					//Vector2 DisablePosition = box.FaceInfo.IsLeft ? LeftDisablePosition : RightDisablePosition;
					//StartCoroutine(MainSystem.MoveToPosition(box.gameObject, DisablePosition, 1000f));
				}
			}

			if (first == null || first == "")
			{
				return;
			}

			StoryFaceInfo info = null;

			foreach(StoryFaceInfo i in FaceInfos)
			{
				if (i.FaceName == first)
				{
					info = i;
					goto Contains;
				}
			}
			throw new Exception($"Face not found: {first}");

			Contains:
			StoryFaceBox faceBox;

			if (FaceBoxs.ContainsKey(info.FaceName))
			{
				faceBox = FaceBoxs[info.FaceName];
			}
			else
			{
				GameObject obj = Instantiate(Prefabricate);
				obj.SetActive(false);
				faceBox = obj.GetComponent<StoryFaceBox>();
				faceBox.FaceInfo = info;

				if (info.IsLeft)
				{
					faceBox.transform.SetParent(LeftFaceMain.transform);
				}
				else
				{
					faceBox.transform.SetParent(RightFaceMain.transform);
				}

				FaceBoxs.Add(info.FaceName, faceBox);
				faceBox.Init(last);

				//Vector2 Position = faceBox.FaceInfo.IsLeft ? LeftDisablePosition : RightDisablePosition;

				//faceBox.transform.localPosition = Position;
			}

			faceBox.gameObject.SetActive(true);

			faceBox.SetFaceImage(last);
			
			if (faceBox.Disabled)
			{
				faceBox.Enable();
			}
			//Vector2 EnablePosition = faceBox.FaceInfo.IsLeft ? LeftEnablePosition : RightEnablePosition;
			//StartCoroutine(MainSystem.MoveToPosition(faceBox.gameObject, EnablePosition, 1000f));
		}

		public void Clear()
		{
			foreach(var box in FaceBoxs.Values)
			{
				Destroy(box.gameObject);
			}

			FaceBoxs.Clear();
		}
	}
}