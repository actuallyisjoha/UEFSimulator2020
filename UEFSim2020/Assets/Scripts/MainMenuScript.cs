using UnityEngine;
using UnityEngine.SceneManagement;

namespace UEFSimulator
{
    public class MainMenuScript : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("LaitosScene", LoadSceneMode.Single);
            }
        }
    }
}