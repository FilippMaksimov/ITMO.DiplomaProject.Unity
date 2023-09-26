using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public bool _isPaused;
    [SerializeField]
    private GameObject pausePanel;
    private bool _isResumeCliked;
    // Start is called before the first frame update
    void Start()
    {
        _isPaused = false;
        _isResumeCliked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || _isResumeCliked)
        {
            _isPaused = !_isPaused;
            if (_isPaused)
            {
                pausePanel.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }
            else
            {
                pausePanel.SetActive(false);
                Cursor.visible = false;
                Time.timeScale = 1;
                _isResumeCliked = false;
            }
        }
    }
    public void ExitGame()
    {
        GetComponent<PlayerDataController>()._isClosed = true;
        GetComponent<PlayerDataController>().SpentPotions();
    }
    public void Resume()
    {
        _isResumeCliked = true;
        _isPaused = false;
    }
}
