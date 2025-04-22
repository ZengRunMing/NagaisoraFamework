using UnityEngine;
using UnityEngine.UI;
using System;
using NagaisoraFamework;

namespace NagaisoraFamework
{
	public class ColorSettingForm : CommMonoScriptObject
	{
		public Image colorImg;

		public Canvas Canvas;
		public ImageDrap ImageDrap;

		public Image SImg;
		public Text HiderText;

		public InputField RI;
		public InputField GI;
		public InputField BI;
		public InputField AI;

		public Slider RS;
		public Slider GS;
		public Slider BS;
		public Slider AS;
		public void Show(string Text, ShowFormType type, Vector2 size, Image Colorimage)
		{
			if (size == new Vector2(0, 0))
			{
				size = gameObject.GetComponent<RectTransform>().sizeDelta;
			}
			else
			{
				size += new Vector2(10, 45);
			}

			colorImg = Colorimage;
			ImageDrap.canvas = Canvas;
			HiderText.text = Text;

			RS.value = colorImg.color.r;
			GS.value = colorImg.color.g;
			BS.value = colorImg.color.b;
			AS.value = colorImg.color.a;

			RS_Click();
			GS_Click();
			BS_Click();
			AS_Click();

			gameObject.transform.localScale = new Vector3(1, 1, 1);

			RectTransform rect = GetComponent<RectTransform>();

			rect.sizeDelta = size;

			switch (type)
			{
				case ShowFormType.CenterScene:
					rect.anchorMin = new Vector2(0.5f, 0.5f);
					rect.anchorMax = new Vector2(0.5f, 0.5f);
					rect.pivot = new Vector2(0.5f, 0.5f);
					rect.anchoredPosition = new Vector3(0, 0, 0);
					break;
				case ShowFormType.WindowsDefaultLocation:
					rect.anchorMin = new Vector2(0f, 1f);
					rect.anchorMax = new Vector2(0f, 1f);
					rect.pivot = new Vector2(0f, 1f);
					rect.anchoredPosition = new Vector3(50, -50, 0);
					break;

				case ShowFormType.Top:
					rect.anchorMin = new Vector2(0.5f, 1f);
					rect.anchorMax = new Vector2(0.5f, 1f);
					rect.pivot = new Vector2(0.5f, 1f);
					rect.anchoredPosition = new Vector3(0, 0, 0);
					break;
				case ShowFormType.Bottom:
					rect.anchorMin = new Vector2(0.5f, 0f);
					rect.anchorMax = new Vector2(0.5f, 0f);
					rect.pivot = new Vector2(0.5f, 0f);
					rect.anchoredPosition = new Vector3(0, 0, 0);
					break;
				case ShowFormType.Left:
					rect.anchorMin = new Vector2(0f, 0.5f);
					rect.anchorMax = new Vector2(0f, 0.5f);
					rect.pivot = new Vector2(0f, 0.5f);
					rect.anchoredPosition = new Vector3(0, 0, 0);
					break;
				case ShowFormType.Right:
					rect.anchorMin = new Vector2(1f, 0.5f);
					rect.anchorMax = new Vector2(1f, 0.5f);
					rect.pivot = new Vector2(1f, 0.5f);
					rect.anchoredPosition = new Vector3(0, 0, 0);
					break;

				case ShowFormType.LeftTop:
					rect.anchorMin = new Vector2(0f, 1f);
					rect.anchorMax = new Vector2(0f, 1f);
					rect.pivot = new Vector2(0f, 1f);
					rect.anchoredPosition = new Vector3(0, 0, 0);
					break;
				case ShowFormType.RightTop:
					rect.anchorMin = new Vector2(1f, 1f);
					rect.anchorMax = new Vector2(1f, 1f);
					rect.pivot = new Vector2(1f, 1f);
					rect.anchoredPosition = new Vector3(0, 0, 0);
					break;
				case ShowFormType.LeftBottom:
					rect.anchorMin = new Vector2(0f, 0f);
					rect.anchorMax = new Vector2(0f, 0f);
					rect.pivot = new Vector2(0f, 0f);
					rect.anchoredPosition = new Vector3(0, 0, 0);
					break;
				case ShowFormType.RightBottom:
					rect.anchorMin = new Vector2(1f, 0f);
					rect.anchorMax = new Vector2(1f, 0f);
					rect.pivot = new Vector2(1f, 0f);
					rect.anchoredPosition = new Vector3(0, 0, 0);
					break;
			}

			gameObject.SetActive(true);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		public void Close()
		{
			Destroy(gameObject);
		}

		public void Save()
		{
			colorImg.color = new Color(RS.value, GS.value, BS.value, AS.value);
		}

		public void SImgUpdate()
		{
			SImg.color = new Color(RS.value, GS.value, BS.value, AS.value);
		}

		public void RI_Click()
		{
			RS.value = Convert.ToSingle(RI.text) / 255f;
		}

		public void GI_Click()
		{
			GS.value = Convert.ToSingle(GI.text) / 255f;
		}

		public void BI_Click()
		{
			BS.value = Convert.ToSingle(BI.text) / 255f;
		}

		public void AI_Click()
		{
			AS.value = Convert.ToSingle(AI.text) / 255f;
		}

		public void RS_Click()
		{
			RI.text = (RS.value * 255f).ToString("0");
			SImgUpdate();
		}

		public void GS_Click()
		{
			GI.text = (GS.value * 255f).ToString("0");
			SImgUpdate();
		}

		public void BS_Click()
		{
			BI.text = (BS.value * 255f).ToString("0");
			SImgUpdate();
		}

		public void AS_Click()
		{
			AI.text = (AS.value * 255f).ToString("0");
			SImgUpdate();
		}
	}
}