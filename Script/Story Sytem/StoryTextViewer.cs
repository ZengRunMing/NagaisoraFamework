using UnityEngine;
using UnityEngine.UI;

namespace NagaisoraFamework
{
    public class StoryTextViewer : CommMonoScriptObject
	{
        public string Text; 

        public Text TextBox;

        void Start()
        {
            TextBox = GetComponent<Text>();
		}

        void Update()
        {
            TextBox.text = Text;
        }
    }
}