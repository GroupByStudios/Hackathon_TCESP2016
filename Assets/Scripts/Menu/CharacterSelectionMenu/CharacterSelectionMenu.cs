using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Hackatoon_TCE
{
	public class CharacterSelectionMenu : MonoBehaviour {

        public GameObject sobreImage;

		public GameObject[] Characters;
		public int CharacterSelectedIndex;
		public float CharacterSelectionInputCooldown = 1f;
		public Text Label_CharacterName;
		public Text Label_CharacterDescription;

		private float CurrentCharacterSelectionInputCooldown = 0f;

		#region Unity Methods

		// Use this for initialization
		void Start () {

			CharacterSelectedIndex = -1;

			for (int i = 0; i < Characters.Length; i++)
			{
				if (Characters[i].activeInHierarchy)
				{
					CharacterSelectedIndex = i;
					ChangeSelectedCharacterLabel(CharacterSelectedIndex);
					break;
				}
			}
		}
		
		// Update is called once per frame
		void Update () {

			CurrentCharacterSelectionInputCooldown += Time.deltaTime;
			float horizontalInput = Input.GetAxisRaw("Horizontal");

			if (horizontalInput != 0 && CurrentCharacterSelectionInputCooldown > CharacterSelectionInputCooldown)
			{
				bool ascend = horizontalInput == 1;
				ChangeSelectedCharacter(ascend);
				CurrentCharacterSelectionInputCooldown = 0f;
			}
		}

		#endregion

		#region Private Methods

		private void ChangeSelectedCharacter(bool ascend)
		{
			if (ascend)
			{
				if (CharacterSelectedIndex == Characters.Length - 1)
					CharacterSelectedIndex = 0;
				else
					CharacterSelectedIndex++;				
			}
			else
			{
				if (CharacterSelectedIndex == 0)
					CharacterSelectedIndex = Characters.Length -1;
				else
					CharacterSelectedIndex--;				
			}

			for (int i = 0; i <Characters.Length; i++)
			{
				Characters[i].SetActive(false);
			}

			Characters[CharacterSelectedIndex].SetActive(true);
			ChangeSelectedCharacterLabel(CharacterSelectedIndex);
		}

		private void ChangeSelectedCharacterLabel(int index)
		{
			CharacterMenu characterMenu = Characters[CharacterSelectedIndex].GetComponent<CharacterMenu>();
			if (characterMenu != null && Label_CharacterName != null && Label_CharacterDescription != null)
			{
				Label_CharacterName.text = characterMenu.Name;
				Label_CharacterDescription.text = characterMenu.Description;
			}
		}

		#endregion

		#region Public Methods

		public void Btn_PriorCharacterSelection_Click()
		{
			ChangeSelectedCharacter(false);
		}

		public void Btn_NextCharacterSelection_Click()
		{
			ChangeSelectedCharacter(true);
		}

        public void Btn_Jogar_Click()
        {
            GameManager.StaticSelectedAvatar = CharacterSelectedIndex;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void Btn_Sobre_Click()
        {
            sobreImage.SetActive(!sobreImage.activeInHierarchy);

        }

		#endregion




	}
}
