using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UEFSimulator
{
    public class MainMenuScript : MonoBehaviour
    {
        bool screen2Shown = false;
        public Sprite menu2;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
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