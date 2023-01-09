using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public List<GameObject> views;

    public void LoadScene()
    {
        SceneManager.LoadScene("Andrea");
    }

    public void OpenView(GameObject view)
    {
        view.SetActive(true);
        views.ForEach(x => x.SetActive(view == x));
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
