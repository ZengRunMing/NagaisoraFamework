using System.Security;
using UnityEngine;
using UnityEngine.UI;

namespace NagaisoraFamework
{
	public class CanvasControl : CommMonoScriptObject
	{
		public Canvas canvas;

		[Header("窗体预制件")]
		public GameObject colorSettingForm;
		public GameObject Form;
		public Vector2 TowerPosition;

		public GameObject STMC;

		public ColorSettingForm NewColorSettingForm()
		{
			GameObject Obj = Instantiate(colorSettingForm);

			Obj.transform.SetParent(STMC.transform);

			ColorSettingForm colorsetting = Obj.GetComponent<ColorSettingForm>();
			colorsetting.Canvas = canvas;
			
			return colorsetting;
		}

		public Form NewEmptyForm()
		{
			GameObject Obj = Instantiate(Form);

			Obj.transform.SetParent(STMC.transform);

			Form TForm = Obj.GetComponent<Form>();
			TForm.Canvas = canvas;

			return TForm;
		}
	}
}