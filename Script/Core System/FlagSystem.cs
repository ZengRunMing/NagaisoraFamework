
namespace NagaisoraFamework
{
	using STGSystem;

	public interface IFlag
	{
		string FlagName { get; }
		bool MultipleExecutions { get; }

		bool Condition();
		void Action();
	}

	public interface ISTGComponentFlag : IFlag
	{
		STGComponent Component { get; set; }
	}

}
