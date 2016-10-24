using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.SceneManagement;

namespace Hackatoon_TCE
{

	public class GameManager : MonoBehaviour {

        public static int StaticSelectedAvatar;

        public Officer OfficerPreFab;
        public SpawnerManager[] SpawnManagers = null;
        public Player Player = null;
        public Transform OfficerSpawner;

        public Slider SliderCorrupcao;
        public Slider SliderCidadania;

        public int corrupcao;
        public int cidadania;

		public GameObject HUD_Jogo_Ganhou;
		public GameObject HUD_Jogo_Perdeu;
    
        void Update()
        {

            if (SpawnManagers == null)
                Debug.LogError("Nenhum Spawn Manager conectado");

            if (Player == null)
                Debug.LogError("Instancia do Jogador nao encontrada");

            SpawnerManager spawnerPtr = null;
            Enemy enemyPtr = null;

			/*
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
			*/

            AtualizarHUD();
        }

		public void SpawnOfficer(Vector3 position)
		{
			if (OfficerPreFab != null)
			{
				Officer officerObj = Instantiate(OfficerPreFab);
				officerObj.transform.position = OfficerSpawner.position;
				officerObj.navMeshAgent = officerObj.GetComponent<NavMeshAgent>();
				officerObj.navMeshAgent.Warp(OfficerSpawner.position);
				officerObj.Target = position;
			}			
		}

        public void AumentarCorrupcao()
        {
            corrupcao++;
            if (corrupcao >= 20)
            {
                // Perdeu o jogo
				HUD_Jogo_Perdeu.SetActive(true);
            }
			else
			{
	            cidadania--;

	            if (cidadania < 0)
	                cidadania = 0;
			}
        }

        public void AumtentarCidadania()
        {
            cidadania++;
            corrupcao--;

            if (corrupcao < 0)
                corrupcao = 0;

            if (cidadania > 30)
            {
				HUD_Jogo_Ganhou.SetActive(true);
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
                SliderCidadania.maxValue = 30;
                SliderCidadania.value = cidadania;
            }

        }
			
		public void btn_ComecarDenovo_Click()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
		}
	}

}
