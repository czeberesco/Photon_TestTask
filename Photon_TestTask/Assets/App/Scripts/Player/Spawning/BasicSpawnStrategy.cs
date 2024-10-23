using System.Collections.Generic;
using Player.Spawning.Data;
using UnityEngine;

namespace Player.Spawning
{
	public class BasicSpawnStrategy : SpawnStrategy
	{
		#region SerializeFields

		[SerializeField] private List<Transform> m_spawnPoints = new();

		#endregion

		#region PublicMethods

		public override PlayerSpawnData GetSpawnData()
		{
			if (m_spawnPoints.Count == 0)
			{
				Transform selfTransform = transform;

				return new PlayerSpawnData
				{
					Position = selfTransform.position,
					Rotation = selfTransform.rotation
				};
			}

			Transform spawnPoint = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)];

			return new PlayerSpawnData
			{
				Position = spawnPoint.position,
				Rotation = spawnPoint.rotation
			};
		}

		#endregion
	}
}
