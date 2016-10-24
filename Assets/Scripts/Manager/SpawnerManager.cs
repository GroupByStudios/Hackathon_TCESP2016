using UnityEngine;
using System.Collections;

namespace Hackatoon_TCE
{

	public class SpawnerManager : MonoBehaviour {

		public GameObject EnemyPrefab;
		public Transform[] SpawnPoints;
		public int MaxEnemies;
		public float SpawnCooldown;

		[Range(1, 10)]
		public int CorruptChance = 4;

		private int CitizenCount = 0;

		float currentSpawnCooldown;
        [HideInInspector]
		public Enemy[] Enemies;

        public GameManager gameManager;

		// Use this for initialization
		void Start () {
		
			Enemies = new Enemy[MaxEnemies];

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

					int corruptRandom = Random.Range(1, 10);

					Enemy enemy = Instantiate(EnemyPrefab).GetComponent<Enemy>();
					enemy.IsCorrupt = corruptRandom <= CorruptChance || CitizenCount > 15;

					if (enemy.IsCorrupt)
						enemy.name = "Corrupt";
					else
					{
						enemy.name = "Citizen";
						CitizenCount++;
					}

					enemy.EnemyIndex = enemyFreeSlot;
                    enemy.gameManager = gameManager;
					enemy.navmeshAgent = enemy.GetComponent<NavMeshAgent>();
					enemy.navmeshAgent.Warp(SpawnPoints[spawnPointIndex].position);
					enemy.transform.position = SpawnPoints[spawnPointIndex].position;
					enemy.transform.rotation = SpawnPoints[spawnPointIndex].rotation;
					enemy.spawnerManager = this;

					//Destroy(enemy, 10);
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

		public void FreeSlot(int index)
		{
			Enemies[index] = null;
		}
	}
}
