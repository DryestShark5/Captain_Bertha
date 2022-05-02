using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

    private void Awake()
    {
        if (SceneManager.instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void GameOver()
    {
        UIManager _ui = GetComponent<UIManager>();
        if (_ui != null)
        {
            _ui.ToggleDeathPanel();
        }
    }
}
