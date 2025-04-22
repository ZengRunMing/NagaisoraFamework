using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

namespace NagaisoraFamework
{
	public class Form : CommMonoScriptObject
	{
		public Text Title;
		public GameObject FormMain;

		public Canvas Canvas;
		public ImageDrap ImageDrap;

		public long time;
		public long fixedtime;

		public delegate void STL();
		public delegate void ESTL(long time);

		public event STL OnAwake;
		public event STL OnStart;
		public event STL Enableing;
		public event STL Disableing;
		public event STL Closeing;

		public event ESTL OnUpdate;
		public event ESTL OnFixedUpdate;

		public void Select(Canvas canvas, string Text, ShowFormType type, Vector2 size)
		{
			if (size == Vector2.zero)
			{
				size = new(640, 480);
			}

			size += new Vector2(10, 45);

			ImageDrap.canvas = Canvas = canvas;
			Title.text = Text;

			gameObject.transform.localScale = new(1, 1, 1);

			RectTransform rect = GetComponent<RectTransform>();

			rect.sizeDelta = size;

			switch (type)
			{
				case ShowFormType.CenterScene:
					rect.anchorMin = new(0.5f, 0.5f);
					rect.anchorMax = new(0.5f, 0.5f);
					rect.pivot = new(0.5f, 0.5f);
					rect.anchoredPosition = new(0, 0);
					break;
				case ShowFormType.WindowsDefaultLocation:
					rect.anchorMin = new(0f, 1f);
					rect.anchorMax = new(0f, 1f);
					rect.pivot = new(0f, 1f);
					rect.anchoredPosition = new(50, -50);
					break;

				case ShowFormType.Top:
					rect.anchorMin = new(0.5f, 1f);
					rect.anchorMax = new(0.5f, 1f);
					rect.pivot = new(0.5f, 1f);
					rect.anchoredPosition = new(0, 0);
					break;
				case ShowFormType.Bottom:
					rect.anchorMin = new(0.5f, 0f);
					rect.anchorMax = new(0.5f, 0f);
					rect.pivot = new(0.5f, 0f);
					rect.anchoredPosition = new(0, 0);
					break;
				case ShowFormType.Left:
					rect.anchorMin = new(0f, 0.5f);
					rect.anchorMax = new(0f, 0.5f);
					rect.pivot = new(0f, 0.5f);
					rect.anchoredPosition = new(0, 0);
					break;
				case ShowFormType.Right:
					rect.anchorMin = new(1f, 0.5f);
					rect.anchorMax = new(1f, 0.5f);
					rect.pivot = new(1f, 0.5f);
					rect.anchoredPosition = new(0, 0);
					break;

				case ShowFormType.LeftTop:
					rect.anchorMin = new(0f, 1f);
					rect.anchorMax = new(0f, 1f);
					rect.pivot = new(0f, 1f);
					rect.anchoredPosition = new(0, 0);
					break;
				case ShowFormType.RightTop:
					rect.anchorMin = new(1f, 1f);
					rect.anchorMax = new(1f, 1f);
					rect.pivot = new(1f, 1f);
					rect.anchoredPosition = new(0, 0);
					break;
				case ShowFormType.LeftBottom:
					rect.anchorMin = new(0f, 0f);
					rect.anchorMax = new(0f, 0f);
					rect.pivot = new(0f, 0f);
					rect.anchoredPosition = new(0, 0);
					break;
				case ShowFormType.RightBottom:
					rect.anchorMin = new(1f, 0f);
					rect.anchorMax = new(1f, 0f);
					rect.pivot = new(1f, 0f);
					rect.anchoredPosition = new(0, 0);
					break;
			}
		}

		public virtual void Show()
		{
			gameObject.SetActive(true);
		}

		public virtual void Hide()
		{
			gameObject.SetActive(false);
		}

		public virtual void Close()
		{
			Destroy(gameObject);
		}

		public virtual void Awake()
		{
			OnAwake?.Invoke();
		}

		public virtual void Start()
		{
			OnStart?.Invoke();
		}

		public virtual void OnEnable()
		{
			Enableing?.Invoke();
		}

		public virtual void OnDisable()
		{
			Disableing?.Invoke();
		}

		public virtual void OnDestroy()
		{
			Closeing?.Invoke();
		}

		public virtual void Update()
		{
			OnUpdate?.Invoke(time);
			time++;
		}

		public virtual void FixedUpdate()
		{
			OnFixedUpdate?.Invoke(fixedtime);
			fixedtime++;
		}
	}
}