using UnityEngine;
using System.Collections;

namespace Hackatoon_TCE
{

	public class SpawnerManager : MonoBehaviour {

		public GameObject EnemyPrefab;
		public Transform[] SpawnPoints;
		public int MaxEnemies;
		public float SpawnCooldown;

		float currentSpawnCooldown;
		GameObject[] Enemies;

		// Use this for initialization
		void Start () {
		
			Enemies = new GameObject[MaxEnemies];

			for(int i = 0; i < Enemies.Length; i++)
			{
				Enemies[i] = null;
			}
		}
		
		// Update is called once per frame
		void Update () {
		

			currentSpawnCooldown += Time.deltaTime;

			if (currentSpawnCooldown > SpawnCooldown && EnemyPrefab != null)
			{
				int enemyFreeSlot = GetFreeSlot();

				if (enemyFreeSlot > -1)
				{
					int spawnPointIndex = Random.Range(0, SpawnPoints.Length);

					GameObject enemy = Instantiate(EnemyPrefab);
					enemy.transform.position = SpawnPoints[spawnPointIndex].position;
					enemy.transform.rotation = SpawnPoints[spawnPointIndex].rotation;

					Destroy(enemy, 10);

					Enemies[enemyFreeSlot] = enemy;
				}

				currentSpawnCooldown = 0;
			}
		}


		int GetFreeSlot()
		{
			for (int i = 0; i < Enemies.Length; i++)
			{
				if (Enemies[i] == null)
					return i;	
			}

			return -1;
		}
	}
}
