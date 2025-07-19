using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [Header("Buttons Elements")]
    // Example button field, replace or add your actual button references here
    public Button successRestartButton;
    public Button successExitButton;
    public Button failedRestartButton;
    public Button failedExitButton;

    // Start is called before the first frame update
    void Start()
    {
        successExitButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainMenu");
        });
        successRestartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        failedRestartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        failedExitButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainMenu");
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
