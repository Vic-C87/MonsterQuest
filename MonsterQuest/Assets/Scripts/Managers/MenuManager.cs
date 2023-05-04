using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MonsterQuest
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        Button myContinueButton;
        [SerializeField]
        Button myNewGameButton;        

        void Start()
        {
            if (SaveGameHelper.SaveFileExists)
            {
                myContinueButton.gameObject.SetActive(true);
            }
            else
            {
                myContinueButton.gameObject.SetActive(false);

            }
        }

        public void ContinueGame()
        {
            SceneManager.LoadScene(1);
        }

        public void NewGame() 
        {
            SaveGameHelper.Delete();
            SceneManager.LoadScene(1);
        }
    }
}
