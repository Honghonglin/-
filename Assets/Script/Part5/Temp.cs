using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Part5;
/// <summary>
/// 没用
/// </summary>
public class Temp : MonoBehaviour
{
    public Dropdown dropdown3;
    public Dropdown dropdown2;
    public GameObject t;
    // Start is called before the first frame update
    void Start()
    {
        //dropdown2.ClearOptions();
        //List<string> vs = new List<string>() { "线网5" };
        //dropdown2.AddOptions(vs);
        GameObject k=Instantiate(t,gameObject.transform);
        k.GetComponent<ImportantPoint_number>().Number = 5;
    }
    private void Update()
    {
        //print(int.Parse(dropdown2.captionText.text.Replace("线网", "")));
        //dropdown3.onValueChanged.AddListener((int v) => OnValue(5));
        
    }
    public void OnValue(int v)
    {
        Debug.Log(v);
    }
    public void Pro()
    {
        List<string> vs = new List<string>();
        for (int i = 1; i <= 5; i++)
        {
            vs.Add(i.ToString());
        }
        dropdown3.AddOptions(vs);
    }


}
