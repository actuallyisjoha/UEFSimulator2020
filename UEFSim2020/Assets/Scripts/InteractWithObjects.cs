using UnityEngine;
using UnityEngine.UI;

public class InteractWithObjects : MonoBehaviour
{
    public ControllerScript CtrlScript;
    public Camera Camera;
    public LayerMask LayerMask;
    public Text ItemImage;
    private Item SelectedItem;

    void Update()
    {
        Ray ray = Camera.ViewportPointToRay(Vector3.one / 2f);
        Debug.DrawRay(ray.origin, ray.direction * 2f, Color.red);


        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 3f, LayerMask))
        {
            var hitItem = hitInfo.collider.GetComponent<Item>();
            if (hitItem != null)
            {
                SelectedItem = hitItem;
                switch (hitInfo.transform.tag)
                {
                    case "Computer":
                        ItemImage.text = "Opiskele";
                        break;

                    case "Food":
                        ItemImage.text = "Syö Kemerissä";
                        break;

                    case "Game":
                        ItemImage.text = "Pelaa peliä";
                        break;
                }
            }
        }
        else
        {
            SelectedItem = null;
        }

        if (SelectedItem != null)
        {
            ItemImage.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                switch (hitInfo.transform.tag)
                {
                    case "Computer":
                        CtrlScript.Study();
                        break;

                    case "Food":
                        CtrlScript.Eat();
                        break;

                    case "Game":
                        CtrlScript.Game();
                        break;
                }

            }
        }
        else
        {
            ItemImage.gameObject.SetActive(false);
        }
    }

}
