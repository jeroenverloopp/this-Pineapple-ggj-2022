using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public Text text;
    // Start is called before the first frame update

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main");
    }
}
