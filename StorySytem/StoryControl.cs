using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace NagaisoraFramework
{
    public class StoryControl : CommMonoScriptObject
    {
        public StoryData StoryData;

		public Queue<ProgramStep> StoryStep;

		public int ProgramIndex;

		public StoryTextViewer StoryTextViewer;
        public StoryFaceControl StoryFaceControl;
		public CanvasGroup CanvasGroup;

		public bool KeyEnable = false;

		public ushort lastkey = 0;

		public void Awake()
		{
			StoryFaceControl.FaceInfos = StoryData.StoryFaceInfos;
			
			if (!TryGetComponent(out CanvasGroup))
			{
				CanvasGroup = gameObject.AddComponent<CanvasGroup>();
			}
		}

		public void OnEnable()
		{
			Init();
			StoryCtrl();
		}

		public void OnDisable()
		{
			MainSystem.KeyDown -= KeyDown;
		}

		public void Init()
        {
			MainSystem.KeyDown += KeyDown;

			StoryStep = new Queue<ProgramStep>();
			foreach (var step in StoryData.StoryProgram[ProgramIndex].Program.ProgramSteps)
			{
				step.Init();
				StoryStep.Enqueue(step);
			}

			StoryFaceControl.Init();
		}

		public void StoryReset()
		{
			StoryTextViewer.Text = "";
			StoryFaceControl.Clear();
		}

		public void KeyDown(bool[] keys)
		{
			if (!KeyEnable)
			{
				return;
			}

			BitArray bitArray = new BitArray(keys);

			byte[] data = new byte[bitArray.Count / 8];
			bitArray.CopyTo(data, 0);
			ushort nowkey = BitConverter.ToUInt16(data, 0);

			if (nowkey != lastkey)
			{
				lastkey = nowkey;

				if (keys[4])
				{
					KeyEnable = false;
					StoryCtrl();
				}
			}
		}

		public void StoryCtrl()
        {
			if (StoryStep.Count == 0)
			{
				Debug.Log("StoryEnd");
				StoryFaceControl.SetFace(null, null);

				while (CanvasGroup.alpha > 0)
				{
					CanvasGroup.alpha-= 0.005f;
				}

				return;
			}

            StepExecute(StoryStep.Dequeue());
		}

		public void StepExecute(ProgramStep Step)
		{
			while (Step.ActionsQueue.Count > 0)
			{
				ActionExecute(Step.ActionsQueue.Dequeue());
			}
		}

		public void ActionExecute(ProgramAction action)
		{
			switch (action.Command)
			{
				case 0:
					StoryReset();
					break;
				case 1:
					StoryFaceControl.SetFace(action.CommandValue[0], action.CommandValue[1]);
					break;
				case 2:
					StoryTextViewer.Text = action.CommandValue[0];
					break;
				case 3:
					ShowBalloon(action.CommandValue[0],
						int.Parse(action.CommandValue[1]),
						int.Parse(action.CommandValue[2]),
						int.Parse(action.CommandValue[3]),
						new Vector2(float.Parse(action.CommandValue[4]),
						float.Parse(action.CommandValue[5])));
					break;
				case 4:
					KeyEnable = true;
					break;
			}
		}

		public void ShowBalloon(string text ,int width, int line, int type, Vector2 position)
		{
			if (line == 0)
			{
				throw new Exception("Balloon line is 0");
			}

			switch (type)
			{
				case 0://LT
					break;
				case 1://LB
					break;
				case 2://RT
					break;
				case 3://RB
					break;
				case 4://LTA
					break;
				case 5://LBA
					break;
				case 6://RTA
					break;
				case 7://RBA
					break;
				case 8://LTT
					break;
				case 9://LBT
					break;
				case 10://RTT
					break;
				case 11://RBT
					break;
				default:
					throw new Exception("Balloon type is not found");
			}


		}
	}
}
