using System;
using UnityEngine;
using UnityEngine.UI;

namespace NagaisoraFamework
{
	public class TextSlider : CommMonoScriptObject
	{
		public Slider textslider;
		public InputField text;

		public float value
		{
			get
			{
				return textslider.value;
			}

			set
			{
				textslider.value = value;
			}
		}

		public void SliderClick()
		{
			text.text = textslider.value.ToString("0.000");
		}

		public void InputClick()
		{
			textslider.value = Convert.ToSingle(text.text);
		}
	}
}
