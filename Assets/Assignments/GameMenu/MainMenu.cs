using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    public GameObject quitPopup;

    public TextMeshProUGUI textLabelName;
    public Slider sliderName;
    public Toggle toggleName;
    TMP_Dropdown dropdownName;

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        quitPopup.SetActive(false);
    }

    public void ShowOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        creditsMenu.SetActive(false);
        quitPopup.SetActive(false);
    }

    public void ShowCredits()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(true);
        quitPopup.SetActive(false);
    }

    public void ShowQuit()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        quitPopup.SetActive(true);
    }

    

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game has been quit!");
    }
}
