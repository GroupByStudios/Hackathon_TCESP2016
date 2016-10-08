using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Hackatoon_TCE
{

	public class GameManager : MonoBehaviour {

        public Officer OfficerPreFab;
        public SpawnerManager[] SpawnManagers = null;
        public Player Player = null;
        public Transform OfficerSpawner;

        public Slider SliderCorrupcao;
        public Slider SliderCidadania;

        public int corrupcao;
        public int cidadania;
    
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


                        }
                    }
                }
            }
            AtualizarHUD();
        }

        public void AumentarCorrupcao()
        {
            corrupcao++;
            if (corrupcao >= 20)
            {
                // Perdeu o jogo
            }
            
            cidadania--;

            if (cidadania < 0)
                cidadania = 0;
        }

        public void AumtentarCidadania()
        {
            cidadania += 3;
            corrupcao--;

            if (corrupcao < 0)
                corrupcao = 0;

            if (cidadania > 20)
            {
                // navega para os creditos
            }
        }


        public void AtualizarHUD()
        {

            if (SliderCorrupcao != null)
            {
                SliderCorrupcao.maxValue = 20;
                SliderCorrupcao.value = corrupcao;
            }

            if (SliderCidadania != null)
            {
                SliderCidadania.maxValue = 20;
                SliderCidadania.value = cidadania;
            }

        }
	}

}
