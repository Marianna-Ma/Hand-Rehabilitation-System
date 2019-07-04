using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour {
    public PanelInstance[] Pages;
    public PanelInstance currentPanel;

    // Use this for initialization
    void Start() {
        /*
        for (int i = 0; i < Pages.Length; i++)
        {
            Pages[i].gameObject.AddComponent<PanelInstance>();
        }
        */
        //OpenPanel(Pages[0]);
        for (int i = 0; i < Pages.Length; i++)
        {
            Pages[i].gameObject.SetActive(true);
            Debug.Log("Start:面板" + Pages[i].gameObject.name + "已经打开");
        }

    }

    private void Awake()
    {
        for (int i = 0; i < Pages.Length; i++)
        {
            Pages[i].gameObject.SetActive(true);
            Debug.Log("Awake:面板" + Pages[i].gameObject.name + "已经打开");
        }
        OpenPanel(Pages[0]);
    }
    //打开面板
    public void OpenPanel(PanelInstance page)
    {
        if (page == null)
        {
            return;
        }
        page.PanelBefore = currentPanel;
        currentPanel = page;
        CloseAllPanels();
        Animator anim = page.GetComponent<Animator>();
        if (anim && anim.isActiveAndEnabled)
        {
            anim.SetBool("Open", true);
        }
        
        page.gameObject.SetActive(true);
        Debug.Log("面板" + page.gameObject.name + "已经打开");
        CloseAllPanels();
        page.gameObject.SetActive(true);
    }
    //通过名字打开面板
    public void OpenPanelByName(string name)
    {
        PanelInstance page = null;
        for (int i = 0; i < Pages.Length; i++)
        {
            if (Pages[i].name == name)
            {
                page = Pages[i];
                break;
            }
        }
        if (page == null)
        {
            Debug.Log("未找到该界面");
            return;
        }
        page.PanelBefore = currentPanel;
        currentPanel = page;
        CloseAllPanels();
        Debug.Log("面板" + page.gameObject.name + "已经打开");
        page.gameObject.SetActive(true);
    }

    //关闭所有面板
    public void CloseAllPanels()
    {
        if (Pages.Length <= 0)
        {
            return;
        }
        Debug.Log(Pages.Length);
        for (int i = 0; i < Pages.Length; i++)
        {
             Animator anim = Pages[i].gameObject.GetComponent<Animator>();
             if (anim && anim.isActiveAndEnabled)
             {
                 anim.SetBool("Open", false);
             }
            //if (Pages[i].isActiveAndEnabled)
            //{
                StartCoroutine(DisablePanelDeleyed(Pages[i]));
                //DisablePanelDeleyed(Pages[i]);
            //}
        }
    }
    //禁用面板
    
    public IEnumerator DisablePanelDeleyed(PanelInstance page)
    {
        
        bool closeStateReached = false;
        bool wantToClose = true;
        Animator anim = page.gameObject.GetComponent<Animator>();
        if (anim && anim.enabled)
        {
            Debug.Log("anim~");
            while(!closeStateReached&&wantToClose)
            {
                if(anim.isActiveAndEnabled&&!anim.IsInTransition(0))
                {
                    closeStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName("Closing");
                }
                //yield return new WaitForEndOfFrame();
                yield return null;
            }
            
            if(wantToClose)
            {
                anim.gameObject.SetActive(false);
            }
            
        }
        page.gameObject.SetActive(false);
        Debug.Log("面板" + page.gameObject.name + "已经关闭");
    }
    
    /*
    public void DisablePanelDeleyed(PanelInstance page)
    {
        Debug.Log("面板" + page.gameObject.name + "已经关闭");
        page.gameObject.SetActive(false);
        
    }
    */
    // Update is called once per frame
    void Update () {
		
	}
}
