using UnityEngine;

public class InteractWithObjects : MonoBehaviour
{
    public ControllerScript CtrlScript;
    public Camera Camera;
    public LayerMask LayerMask;
    public RectTransform ItemImageRoot;
    private Item SelectedItem;

    void Update()
    {
        Ray ray = Camera.ViewportPointToRay(Vector3.one / 2f);
        Debug.DrawRay(ray.origin, ray.direction * 2f, Color.red);


        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 2f, LayerMask))
        {
            var hitItem = hitInfo.collider.GetComponent<Item>();
            if (hitItem != null)
            {
                SelectedItem = hitItem;
            }
        }
        else
        {
            SelectedItem = null;
        }

        if (SelectedItem != null)
        {
            ItemImageRoot.gameObject.SetActive(true);
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
                }

            }
        }
        else
        {
            ItemImageRoot.gameObject.SetActive(false);
        }
    }

}
