using UnityEngine;
using System.Collections;

namespace Hackatoon_TCE
{

	public class GameManager : MonoBehaviour {

        public Officer OfficerPreFab;
        public SpawnerManager[] SpawnManagers = null;
        public Player Player = null;
        public Transform OfficerSpawner;

        void Update()
        {

            if (SpawnManagers == null)
                Debug.LogError("Nenhum Spawn Manager conectado");

            if (Player == null)
                Debug.LogError("Instancia do Jogador nao encontrada");

            SpawnerManager spawnerPtr = null;
            Enemy enemyPtr = null;

            // Percorre todos o Spawn Managers
            for (int i = 0; i < SpawnManagers.Length; i++)
            {
                // Verifica se existe algum inimigo reportado
                spawnerPtr = SpawnManagers[i];

                if (spawnerPtr != null && spawnerPtr.Enemies != null)
                {
                    for (int j = 0; j < spawnerPtr.Enemies.Length; j++)
                    {
                        enemyPtr = spawnerPtr.Enemies[j];
                        
                        if (enemyPtr != null && enemyPtr.Reported && !enemyPtr.Officer)
                        {
                            enemyPtr.Officer = true;
                            if (OfficerPreFab != null)
                            {
                                Officer officerObj = Instantiate(OfficerPreFab);
                                officerObj.transform.position = OfficerSpawner.position;
                                officerObj.navMeshAgent = officerObj.GetComponent<NavMeshAgent>();
                                officerObj.navMeshAgent.Warp(OfficerSpawner.position);
                                officerObj.Target = enemyPtr.transform.position;
                            }

                            // Spawn dos policiais
                            // Muda o estado do personagem para Pego
                        }
                    }
                }
            }
        }


	}

}
