using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Hackatoon_TCE
{

    public class Loja : MonoBehaviour
    {

        public Player Jogador;
        public float CharacterMinDistance = 2f;
        public Text Aperte_E_Text;
        public string textoAperte_E = "Aperte a tecla 'E' para entrar na loja";

        public float UIOffsetY;
        public bool isShowing = false;
        public GameObject LojaCanvas = null;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            if (Jogador != null)
            {
                Vector3 distance = Jogador.transform.position - transform.position;

                if (distance.magnitude < CharacterMinDistance)
                {

                    if (LojaCanvas != null)
                    {
                        if (!isShowing)
                        {

                            LojaCanvas.SetActive(true);
                            isShowing = true;

                            Vector3 textPosition = Camera.main.WorldToScreenPoint(this.transform.position);
                            textPosition.z = 0;
                            textPosition.y += UIOffsetY * Screen.height / 720;
                            Aperte_E_Text.rectTransform.position = textPosition;
                            Aperte_E_Text.gameObject.SetActive(true);
                        }
                        else
                        {
                            Aperte_E_Text.gameObject.SetActive(false);
                        }
                    }

                }
                else
                {

                    if (isShowing)
                    {
                        LojaCanvas.SetActive(false);
                        isShowing = false;
                    }
                }
            }
            
        }
    }

}
