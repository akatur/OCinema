using UnityEngine;

public class menuUiControll : MonoBehaviour
{
    public GameObject PanelUI;
    public GameObject PanelButtonInput;
    public bool isOpened;

    void Start()
    {
        Cursor.visible = true;
        PanelUI.SetActive(false);
        PanelButtonInput.SetActive(false);
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.E))
        {
            isOpened = !isOpened;

            if (isOpened)
            {
                PanelUI.SetActive(true);
                PanelButtonInput.SetActive(true);
            }
            else
            {
                PanelUI.SetActive(false);
                PanelButtonInput.SetActive(false);
            }
        }
    }
}
