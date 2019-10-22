using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UEFSimulator
{
    public class MainMenuScript : MonoBehaviour
    {
        bool screen2Shown = false;
        public Sprite menu2;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (screen2Shown) SceneManager.LoadScene("LaitosScene", LoadSceneMode.Single);
                else
                {
                    GetComponentInChildren<Image>().sprite = menu2;
                    screen2Shown = true;
                }
            }
        }
    }
}