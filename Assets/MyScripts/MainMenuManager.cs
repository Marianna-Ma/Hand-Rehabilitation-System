using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : PanelManager {
    //这个要改
    public string SceneStart = "hands";
    public string UserName;
    public GameObject Preloader;
    
    
	// Use this for initialization
	void Start () {
        Debug.Log(Pages[0].name);	
	}
	
    public void Exit()
    {
        Application.Quit();
    }

    public void OpenPreviousPanel()
    {
        if(currentPanel&&currentPanel.PanelBefore)
        {
            CloseAllPanels();
            Animator anim = currentPanel.PanelBefore.GetComponent<Animator>();
            if(anim&&anim.isActiveAndEnabled)
            {
                anim.SetBool("Open", true);
            }
            currentPanel.PanelBefore.gameObject.SetActive(true);
            currentPanel = currentPanel.PanelBefore;
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
