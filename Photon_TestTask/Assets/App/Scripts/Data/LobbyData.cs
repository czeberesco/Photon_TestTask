using WebSocketSharp;

namespace Data
{
	public readonly struct LobbyData
	{
		#region Properties

		public string Name { get; }

		#endregion

		#region Constructors

		public LobbyData(string name)
		{
			Name = name;
		}

		#endregion

		#region PublicMethods

		public bool IsValid()
		{
			return !Name.IsNullOrEmpty();
		}

		#endregion
	}
}
