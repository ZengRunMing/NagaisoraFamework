using System;
using UnityEngine;
using UnityEngine.UI;

namespace NagaisoraFamework
{
    public class StoryFaceBox : CommMonoScriptObject
    {
        public StoryFaceInfo FaceInfo;

		public bool Disabled = true;

		public Animation Animation;
		public Image MainImageBox;
        public Image FaceImageBox;

		public void Init(string i)
		{
			FaceInfo?.Init();

			SetMainImage();
			SetFaceImage(i);
			Enable();
		}

		public void SetMainImage()
        {
            MainImageBox.sprite = FaceInfo.MainImage;
            MainImageBox.rectTransform.sizeDelta = FaceInfo.MainSize;
        }

		public void SetFaceImage(string i)
		{
			FaceImageBox.sprite = FaceInfo.FaceImages[i].Image;
			
			Vector2 size = FaceInfo.FaceImages[i].Size == Vector2.zero ? FaceInfo.NormalFaceSize : FaceInfo.FaceImages[i].Size;
			FaceImageBox.rectTransform.sizeDelta = size;
			FaceImageBox.transform.localPosition = FaceInfo.FacePosition;
		}

        public void SetOrderTop()
        {
            transform.SetAsLastSibling();
		}

		public void SetImageColor(Color color)
		{
			MainImageBox.color = color;
			FaceImageBox.color = color;
		}

		public void Enable()
		{
			SetOrderTop();
			if (FaceInfo.IsLeft)
			{
				Animation.Play("LeftFaceEnable");
			}
			else
			{
				Animation.Play("RightFaceEnable");
			}

			Disabled = false;
		}

		public void Disable()
		{
			if (FaceInfo.IsLeft)
			{
				Animation.Play("LeftFaceDisable");
			}
			else
			{
				Animation.Play("RightFaceDisable");
			}

			Disabled = true;
		}
	}
}