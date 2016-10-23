using UnityEngine;
using System.Collections;

public class Turorial : MonoBehaviour
{

    public GameObject Tutorial1;
    public GameObject Tutorial2;
    public GameObject Tutorial3;
    public GameObject btnProximo;
    public GameObject btnAnterior;
	public GameObject btnFechar;
	public GameObject btnAbrir;

    public bool aberto = true;

    public float keyCoolDown = 1f;

    public float currentKeyCooldown = 0f;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentKeyCooldown += Time.deltaTime;

        if (currentKeyCooldown > keyCoolDown)
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                currentKeyCooldown = 0f;

                if (!aberto)
                {
                    btn_Abrir();
                }
                else
                {
                    btn_Fechar();
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentKeyCooldown = 0f;
                btn_Proximo();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentKeyCooldown = 0f;
                btn_Anterior();
            }

        }

    }

    public void btn_Proximo()
    {
        if (Tutorial1.activeInHierarchy)
        {
            Tutorial1.SetActive(false);
            Tutorial2.SetActive(true);
            Tutorial3.SetActive(false);
        }
        else if (Tutorial2.activeInHierarchy)
        {
            Tutorial2.SetActive(false);
            Tutorial1.SetActive(false);
            Tutorial3.SetActive(true);
        }
    }


    public void btn_Anterior()
    {
        if (Tutorial3.activeInHierarchy)
        {
            Tutorial3.SetActive(false);
            Tutorial1.SetActive(false);
            Tutorial2.SetActive(true);
        }
        else if (Tutorial2.activeInHierarchy)
        {
            Tutorial2.SetActive(false);
            Tutorial3.SetActive(false);
            Tutorial1.SetActive(true);
        }

    }

    public void btn_Abrir()
    {
		if (!aberto)
		{
	        aberto = true;
	        Tutorial1.SetActive(true);
	        Tutorial2.SetActive(false);
	        Tutorial3.SetActive(false);
	        btnProximo.SetActive(true);
	        btnAnterior.SetActive(true);
			btnAbrir.SetActive(false);
			btnFechar.SetActive(true);
		}
    }

    public void btn_Fechar()
    {
        aberto = false;
        Tutorial1.SetActive(false);
        Tutorial2.SetActive(false);
        Tutorial3.SetActive(false);
        btnProximo.SetActive(false);
        btnAnterior.SetActive(false);
		btnAbrir.SetActive(true);
		btnFechar.SetActive(false);
    }

}
