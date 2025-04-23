using System.Collections.Generic;
using System;

namespace NagaisoraFamework
{

	[Serializable]
	public class Program
	{
		public string ProgramName;
		public List<ProgramStep> ProgramSteps;
	}

	[Serializable]
	public class ProgramStep
	{
		public List<ProgramAction> Actions;
		public ProgramAction Transition;

		public Queue<ProgramAction> ActionsQueue;

		public void Init()
		{
			if (Actions == null)
			{
				return;
			}

			ActionsQueue = new Queue<ProgramAction>();

			foreach (ProgramAction action in Actions)
			{
				ActionsQueue.Enqueue(action);
			}
		}
	}

	[Serializable]
	public class ProgramAction
	{
		public uint Command;
		public List<string> CommandValue;
	}

}
