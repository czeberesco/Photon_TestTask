using Player.Spawning.Data;
using UnityEngine;

namespace Player.Spawning
{
	public abstract class SpawnStrategy : MonoBehaviour
	{
		#region PublicMethods

		public abstract PlayerSpawnData GetSpawnData();

		#endregion
	}
}
