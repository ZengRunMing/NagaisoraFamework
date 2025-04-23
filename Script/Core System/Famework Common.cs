using System.ComponentModel;

namespace NagaisoraFamework
{
	public enum BlendMode
	{
		[Description("Alpha混合")]
		AlphaBlend,
		[Description("叠加（高光）")]
		Additive
	}

	//按需求添加
	public enum SEType
	{
		AsName,
		Select,
		Ok,
		Cancel,
		Warning,
		Pause,
		Big,
		Bonus,
		Bonus2,
		Bonus4,
		Boon00,
		Boon01,
		CardGet,
		Cat00,
		Ch00,
		Ch01,
		Ch02,
		Ch03,
		Danmege00,
		Danmege01,
		Don00,
		Enep00,
		Enep01,
		Enep02,
		Etbreak,
		Extend,
		Extend2,
		Fault,
		Graze,
		Gun00,
		Heal,
		Invalid,
		Item00,
		Item01,
		Kira00,
		Kira01,
		Kira02,
		Lazer00,
		Lazer01,
		Lazer02,
		Lgods1,
		Lgods2,
		Lgods3,
		Lgods4,
		LgodsGet,
		Msl,
		Msl2,
		Msl3,
		Nep00,
		Nodamage,
		Noise,
		Pin00,
		Pin01,
		Pidead00,
		Pidead01,
		Plst00,
		Power0,
		Power1,
		Powerup,
		Slash,
		Tan00,
		Tan01,
		Tan02,
		Tan03,
		Timeout,
		Timeout2,
		Wolf,
	}

	public enum LaserType
	{
		Long,
		Segmental,
	}

	public enum LSLogType
	{
		Info,
		Warning,
		Error,
		Default,
	}

	public enum IMMType
	{
		Int = 0,
		Long = 1,
		Float = 2,
		Double = 3,
		Bool = 4,
		String = 5,
		Char = 6,
		Time = 7,
		Vector2 = 8,
		Vector3 = 9,
		Vector6 = 10,
	}

	public enum ShowFormType
	{
		CenterScene = 0,
		WindowsDefaultLocation = 1,

		Top = 2,
		Bottom = 3,
		Left = 4,
		Right = 5,

		LeftBottom = 6,
		LeftTop = 7,

		RightBottom = 8,
		RightTop = 9,
	}
}