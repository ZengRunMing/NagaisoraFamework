using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace NagaisoraFamework
{
	public class MessageBox : CommMonoScriptObject
	{
		public Animation Anim;
		public Text Title;
		public Text Text;

		public GameObject ButtonOK;
		public GameObject ButtonCanel;
		public GameObject ButtonYes;
		public GameObject ButtonNo;
		public GameObject ButtonAbort;
		public GameObject ButtonRetry;
		public GameObject ButtonIgnore;

		List<GameObject> Buttons;

		public enum ButtonStyle
		{
			OK = 0,
			OKCanel = 1,
			AbortRetryIgnore = 2,
			YesNoCancel = 3,
			YesNo = 4,
			RetryCancel = 5
		}

		public enum IconStyle
		{
			None = 0,
			Hand = 1,
			Question = 2,
			Exclamation = 3,
			Asterisk = 4,
			Stop = 5,
			Error = 6,
			Warning = 7,
			Information = 8
		}

		public void Awake()
		{
			Buttons = new List<GameObject>();
			Buttons.Add(ButtonOK);      //0
			Buttons.Add(ButtonCanel);   //1
			Buttons.Add(ButtonYes);     //2
			Buttons.Add(ButtonNo);      //3
			Buttons.Add(ButtonAbort);   //4
			Buttons.Add(ButtonRetry);   //5
			Buttons.Add(ButtonIgnore);  //6
		}

		public void Show(string Title, string Text, ButtonStyle buttonStyle, IconStyle iconStyle)
		{
			this.Title.text = Title;
			this.Text.text = Text;

			switch (buttonStyle)
			{
				case ButtonStyle.OK:
					SetButtons(new int[] { 0 });
					break;
				case ButtonStyle.OKCanel:
					SetButtons(new int[] { 0, 1 });
					break;
				case ButtonStyle.AbortRetryIgnore:
					SetButtons(new int[] { 4, 5, 6 });
					break;
				case ButtonStyle.YesNoCancel:
					SetButtons(new int[] { 1, 2, 3 });
					break;
				case ButtonStyle.YesNo:
					SetButtons(new int[] { 2, 3 });
					break;
				case ButtonStyle.RetryCancel:
					SetButtons(new int[] { 1, 5 });
					break;
			}

			Anim.Play("Show");
		}

		public void SetButtons(int[] Index)
		{
			for (int i = 0; i < Buttons.Count; i++)
			{
				Buttons[i].SetActive(false);
				if (Index.Contains(i))
				{
					Buttons[i].SetActive(true);
				}
			}
		}

		public void Hide()
		{
			Anim.Play("Hide");
		}
	}
}
