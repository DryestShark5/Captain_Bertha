using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject deathPanel;
    private void Start()
    {
        
    }
    public void ToggleDeathPanel()
    {
        deathPanel.SetActive(!deathPanel.activeSelf);
    }
}
