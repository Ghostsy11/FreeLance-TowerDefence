
using UnityEngine;
public class GameOverPanelHandler : MonoBehaviour
{
    [SerializeField] GameObject gameOverMainMenu;
    [SerializeField] GameObject mainMenu;
    private void Start()
    {
        gameOverMainMenu.SetActive(false);
    }


    public void SetGameObjectF()
    {
        GameManager.instance.LoadScene(0);
        gameOverMainMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);


    }

    public void SetGameObjectT()
    {
        gameOverMainMenu.gameObject.SetActive(true);


    }

}
