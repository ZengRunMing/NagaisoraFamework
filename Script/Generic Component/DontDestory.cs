using NagaisoraFamework;

public class DontDestory : CommMonoScriptObject
{
	public void Awake()
	{
		DontDestroyOnLoad(this);
	}
}
