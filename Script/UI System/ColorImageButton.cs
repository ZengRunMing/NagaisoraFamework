using UnityEngine;
using UnityEngine.UI;

namespace NagaisoraFamework
{
	public class ColorImageButton : CommMonoScriptObject
	{
		public CanvasControl CanvasControl;
		public Text Title;
		public Image image;
		public string title;
		public string Text;

		public void Start()
		{
			if (title != "")
			{
				Title.text = title;
			}
		}

		public void Click()
		{
			if (Text == "")
			{
				Text = "Default Form (Color Setting)";
			}

			ColorSettingForm colorform = CanvasControl.NewColorSettingForm();
			colorform.Show(Text, ShowFormType.WindowsDefaultLocation, new Vector2(0, 0), image);

		}
	}
}
