using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public string gameSceneName;

    public GameObject PineApple, Menu;

    public float logoTimer;
    private float _logoTimer;
    // Start is called before the first frame update
    void Start()
    {
        PineApple.SetActive(true);
        Menu.SetActive(false);
        _logoTimer = logoTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (_logoTimer > 0)
        {
            _logoTimer -= Time.deltaTime;
            if (_logoTimer <= 0)
            {
                PineApple.SetActive(false);
                Menu.SetActive(true);
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
