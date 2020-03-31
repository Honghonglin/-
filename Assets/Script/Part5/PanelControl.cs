using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PanelControl : MonoBehaviour
{
    public GameObject MainScene;
    private Vector3 basePanelPosition=new Vector3(5,5,5);
    public Dropdown dropdown;
    private string[] str;
    public static List<GameObject> panelList = new List<GameObject>();
    public static List<Vector3> vector3s = new List<Vector3>();
    public GameObject prefab;
    public InputField inputField;
    private int maxpath_count;
    public Material material;
    private bool isInCheck = false;
    //标志是否已经生成多层
    private bool isChange = false;
    public Button HideButton;
    public Button ShowButton;
    public Button changebutton;
    readonly private int floor_count=9;
    private GameObject hitGameObject;

    /// <summary>
    /// 得到大小
    /// </summary>
    public int Get_Size()
    {
        string[] vs=dropdown.captionText.text.Split('*');
        return int.Parse(vs[0]) / 90;
    }
    private void Awake()
    {
        str = dropdown.captionText.text.Split('*');
        maxpath_count = 5;
        Produce(maxpath_count,true,basePanelPosition,Get_Size(),ConstClass.nowfloor);
        ConstClass.nowfloor++;
        panelList[0].GetComponent<SinglePanel>().IsAppearance = true;
        panelList[0].GetComponent<SinglePanel>().IsHave = false;
        panelList[0].GetComponent<SinglePanel>().IsSelet = false;
        HideButton.gameObject.SetActive(false);
        ShowButton.gameObject.SetActive(false);
    }
    private void Update()
    {
        string[] temp = dropdown.captionText.text.Split('*');
        if(temp[0].Equals(str[0]))
        {}
        //重新生成单层
        else
        {
            #region Dropdown改变时
            ConstClass.nowfloor = 1;
            foreach (var item in
            panelList[0].GetComponent<SinglePanel>().Lis)
            {
                Destroy(item);
            }
            panelList[0].GetComponent<SinglePanel>().Lis.Clear();
            foreach (GameObject item in panelList)
            {
                Destroy(item);
            }
            panelList.Clear();
            vector3s.Clear();
            str= dropdown.captionText.text.Split('*');
            maxpath_count = 5;
            inputField.textComponent.text = "";
            Produce(maxpath_count, true, basePanelPosition, Get_Size(),ConstClass.nowfloor);
            ConstClass.nowfloor++;
            isChange = false;
            panelList[0].GetComponent<SinglePanel>().IsHave = false;
            panelList[0].GetComponent<SinglePanel>().IsAppearance = true;
            panelList[0].GetComponent<SinglePanel>().IsSelet = false;
            return;
            #endregion
        }
        try
        {
            #region Input改变时
            //改变Input时执行
            int i = int.Parse(inputField.textComponent.text);
            if(i!=maxpath_count&&i!=1)
            {
                maxpath_count = i;
                panelList[0].GetComponent<SinglePanel>().Pathway_count = maxpath_count;
                //重置为单层
                foreach (var item in
               panelList[0].GetComponent<SinglePanel>().Lis)
                {
                    Destroy(item);
                }
                panelList[0].GetComponent<SinglePanel>().Lis.Clear();
                panelList[0].GetComponent<SinglePanel>().IsAppearance = true;
                panelList[0].GetComponent<SinglePanel>().IsHave = false;
                panelList[0].GetComponent<SinglePanel>().IsSelet = false;
                GameObject t = panelList[0];
                for (int k = 1; k < panelList.Count; k++)
                {
                    Destroy(panelList[k]);
                }
                panelList.Clear();
                vector3s.Clear();
                panelList.Add(t);
                vector3s.Add(panelList[0].transform.position);
                isChange = false;
                Debug.Log(maxpath_count);
            }
            #endregion
        }
        catch
        {
            #region 最大轨道数改变时
            if (maxpath_count!=5)
            {
                Debug.Log("最大轨道数改变时");
                maxpath_count = 5;
                panelList[0].GetComponent<SinglePanel>().Pathway_count = maxpath_count;
                foreach (var item in
                   panelList[0].GetComponent<SinglePanel>().Lis)
                {
                    Destroy(item);
                }
                panelList[0].GetComponent<SinglePanel>().Lis.Clear();
                panelList[0].GetComponent<SinglePanel>().IsAppearance = true;
                panelList[0].GetComponent<SinglePanel>().IsHave = false;
                panelList[0].GetComponent<SinglePanel>().IsSelet = false;
                GameObject t = panelList[0];
                for (int k = 1; k < panelList.Count; k++)
                {
                    Destroy(panelList[k]);
                }
                panelList.Clear();
                vector3s.Clear();
                panelList.Add(t);
                vector3s.Add(panelList[0].transform.position);
                isChange = false;
                return;
            }
            #endregion
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50f,LayerMask.GetMask("Panel"))&&Input.GetMouseButtonDown(0)&&!isInCheck&&isChange)
        {
            #region 进入单层观察模式
            Debug.Log("进入单层观察模式");
            hitGameObject = hit.collider.gameObject;
            foreach (GameObject item in panelList)
            {
                if(item!= hitGameObject)
                {
                    item.SetActive(false);
                }
            }
            hitGameObject.GetComponent<SinglePanel>().IsSelet = true;
            inputField.enabled = false;
            dropdown.enabled = false;
            isInCheck = true;
            #endregion 
        }
        else if(isInCheck&&Input.GetMouseButtonDown(0))
        {
            #region 退出单层观察模式
            foreach (GameObject item in panelList)
            {
                item.SetActive(true);
            }
            hitGameObject.GetComponent<SinglePanel>().IsSelet = false;
            hitGameObject.GetComponent<SinglePanel>().IsAppearance = false;
            hitGameObject.GetComponent<SinglePanel>().IsHave = true;
            ShowButton.gameObject.SetActive(false);
            HideButton.gameObject.SetActive(false);
            isInCheck = false;
            hit = new RaycastHit();
            #endregion
        }
        #region 是否生成多层判断
        if (isChange)
        {
            changebutton.enabled = false;
        }
        else
        {
            changebutton.enabled = true;
        }
        #endregion

        #region 观察模式下按钮显示
        if (hitGameObject != null)
        {
            SinglePanel Hit_singlePanel = hitGameObject.GetComponent<SinglePanel>();
            if (isInCheck & Hit_singlePanel.IsSelet && Hit_singlePanel.IsHave && Hit_singlePanel.IsAppearance)
            {
                HideButton.gameObject.SetActive(true);
                ShowButton.gameObject.SetActive(false);
            }
            else if (isInCheck && Hit_singlePanel.IsSelet && Hit_singlePanel.IsHave && !Hit_singlePanel.IsAppearance)
            {
                HideButton.gameObject.SetActive(true);
                ShowButton.gameObject.SetActive(false);
            }
        }
        #endregion

    }


    /// <summary>
    /// 生成单层
    /// </summary>
    public void Produce(int Pathway_count,bool is_across,Vector3 position,float mul,int floor)
    {
        GameObject _gameObject = Instantiate(prefab, position, Quaternion.identity, MainScene.transform);
        _gameObject.transform.localScale= new Vector3(mul, 1, mul);
        SinglePanel singlePanel = _gameObject.GetComponent<SinglePanel>();
        singlePanel.Floor = floor;
        singlePanel.Length = int.Parse(str[0]);
        singlePanel.Width = int.Parse(str[1]);
        singlePanel.Pathway_count = Pathway_count;
        singlePanel.Is_across = is_across;
        singlePanel.Mat = material;
        panelList.Add(_gameObject);
        vector3s.Add(_gameObject.transform.position);
    }

    /// <summary>
    /// 生成多层
    /// </summary>
    public void ChangeButton()
    {
        for (int i = 0; i < floor_count-1; i++)
        {
            Produce(maxpath_count - (int)((i + 1) / 3), i % 2 == 0 ? false : true, basePanelPosition + new Vector3(0, (i + 1) * 1.2f, 0), Get_Size(), ConstClass.nowfloor++);
        }
        isChange = true;
        panelList[0].GetComponent<SinglePanel>().IsSelet = false;
        panelList[0].GetComponent<SinglePanel>().IsHave = true;
        panelList[0].GetComponent<SinglePanel>().IsAppearance = false;
    }

    
    public void HideButtonFun()
    {
        if(hitGameObject!=null)
        {
            hitGameObject.GetComponent<SinglePanel>().IsAppearance = false;
        }
    }
    public void ShowButtonFun()
    {
        if(hitGameObject!=null)
        {
            hitGameObject.GetComponent<SinglePanel>().IsAppearance = true;
        }
    }
    
}
