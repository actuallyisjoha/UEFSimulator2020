using UnityEngine;
using UnityEngine.UI;

namespace UEFSimulator
{
    public class InteractWithObjects : MonoBehaviour
    {
        public ControllerScript CtrlScript;
        public Camera Camera;
        public LayerMask LayerMask;
        public Text ItemImage;
        private Item SelectedItem;

        void Update()
        {
            if (ControllerScript.GameOver) return;
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

                        case "Beer":
                            ItemImage.text = "Juo kaljaa Jolenessa";
                            break;

                        case "Work":
                            ItemImage.text = "Mene töihin ja hajoa intialaisiin koodareihin";
                            break;

                        case "Vending":
                            ItemImage.text = "Osta energiajuomaa";
                            break;

                        case "Bottle":
                            ItemImage.text = "Palauta pullo kauppaan";
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
                if (Input.GetKeyDown(KeyCode.E) && !ControllerScript.popupActive)
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

                        case "Beer":
                            CtrlScript.Drink();
                            break;

                        case "Work":
                            CtrlScript.Work();
                            break;

                        case "Vending":
                            CtrlScript.Vending();
                            break;

                        case "Bottle":
                            CtrlScript.Bottle();
                            Debug.Log(hitInfo.transform.gameObject);
                            Destroy(hitInfo.transform.gameObject);
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
}