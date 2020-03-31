using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Part5;
using Assets.Script.Auto;
using System.Runtime.InteropServices;
using System.IO;
public class Date : MonoBehaviour
{
    [DllImport("link 12")]
    private static extern void Func();
    public static List<Net> lineNets = new List<Net>();
    private List<Point> allBasePoint = new List<Point>();
    private List<GameObject> allBasePointInst = new List<GameObject>();
    public Dropdown dropdown1;
    public Dropdown dropdown2;
    public Dropdown dropdown3;
    public Dropdown dropdown4;
    public Button[] buttons;
    private string[] names = new string[] { "确认", "显示随机生成的引脚", "返回1", "返回2" };
    private ButtonEvent[] events;
    private GameObject PointPrefab;
    private GameObject PointContain;
    public GameObject basePanel;
    public int NowLineCount { get; private set; }
    public int AllMaxPoint { get { return allBasePoint.Count - 2 * (NowLineCount - 1); } private set { } }
    public int AreaLength { get; private set; }
    public int AreaWidth { get; private set; }
    public int PathwayCount { get; private set; }
    public int FloorCount { get;private set; }
    public File_Reader File_Reader { get; private set; } = new File_Reader();
    public File_Writer File_Writer { get; private set; } = new File_Writer();
    /// <summary>
    /// 线网数量
    /// </summary>
    public int PointCount { get; set; }
    /// <summary>
    /// 底层引脚间距
    /// </summary>
    public float Spacing { get; set; }

    public List<int> LinePointCountLis { get; private set; } = new List<int>();

    private float Mulsize { get; set; }

    private delegate void ButtonEvent();

    private void Awake()
    {
        PointPrefab = Resources.Load<GameObject>("ImportantPoint");
        PointContain = GameObject.Find("PointContain");
        events = new ButtonEvent[] { FirstSure,SecondSure,BackButton1,BackButton2};
        for (int i = 0; i < names.Length; i++)
        {
            buttons[i].transform.Find("Text").GetComponent<Text>().text = names[i];
            int a = i;
            buttons[i].onClick.AddListener(()=>events[a]());
            if (names[i]=="确认")
            {
                buttons[i].gameObject.SetActive(true);
            }
        }
        string []temp=dropdown1.captionText.text.Split('*');
        AreaLength = int.Parse(temp[0]);
        AreaWidth = int.Parse(temp[1]);
        PathwayCount = int.Parse(dropdown2.captionText.text);
        FloorCount = int.Parse(dropdown3.captionText.text);
        NowLineCount = int.Parse(dropdown4.captionText.text);
        AutoSinglePlane.lenght = Mathf.Abs((basePanel.transform.Find("leftnorth").transform.position - basePanel.transform.Find("rightnorth").transform.position).magnitude);
        AutoSinglePlane.width= Mathf.Abs((basePanel.transform.Find("leftnorth").transform.position - basePanel.transform.Find("leftsorth").transform.position).magnitude);
        Mulsize = AreaLength / AutoSinglePlane.lenght;
    }
    private void SecondSure()
    {
        //button1.gameObject.SetActive(false);
        foreach (var item in allBasePointInst)
        {
            item.SetActive(false);
        }
        foreach (var net in lineNets)
        {
            foreach (var point in net.Points)
            {
                net.PointsInstant.Add(Instantiate(PointPrefab, point.GetVector3() / Mulsize + basePanel.transform.Find("leftsorth").transform.position, Quaternion.AngleAxis(-90, Vector3.right), PointContain.transform));
            }
        }
        foreach (var item in buttons)
        {
            Text text = item.transform.Find("Text").GetComponent<Text>();
            if ( text.text == "返回2")
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }

        }
    }

    private void BackButton1()
    {
        
        foreach (var nets in lineNets)
        {
            foreach (var item in nets.PointsInstant)
            {
                Destroy(item);
            }
        }
        foreach (var item in allBasePointInst)
        {
            Destroy(item);
        }
        foreach (var item in buttons)
        {
            Text text = item.transform.Find("Text").GetComponent<Text>();
            if ( text.text == "确认")
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }

        }
        lineNets.Clear();
        allBasePointInst.Clear();
    }

    private void BackButton2()
    {

        foreach (var nets in lineNets)
        {
            foreach (var item in nets.PointsInstant)
            {
                Destroy(item);
            }
            nets.PointsInstant.Clear();
        }
        
        //button2.gameObject.SetActive(true);
        foreach (var item in allBasePointInst)
        {
            item.SetActive(true);
        }
        foreach (var item in buttons)
        {
            Text text = item.transform.Find("Text").GetComponent<Text>();
            if (text.text == "显示随机生成的引脚" || text.text == "返回1")
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }

        }
    }


    /// <summary>
    /// 确认按钮
    /// </summary>
    private void FirstSure()
    {
        foreach (var nets in lineNets)
        {
            foreach (var item in nets.PointsInstant )
            {
                Destroy(item);
            }
        }
        allBasePoint.Clear();
        lineNets.Clear();
        
        File_Writer.RouWriter(FloorCount, AreaLength, AreaWidth, PathwayCount);
        File_Writer.PinWriter(NowLineCount);
        Func();
        File_Reader.CandRead(allBasePoint);
        File_Reader.RanRead(lineNets);
        foreach (var point in allBasePoint)
        {
            Debug.Log(Mulsize);
            allBasePointInst.Add(Instantiate(PointPrefab, point.GetVector3() / Mulsize + basePanel.transform.Find("leftsorth").transform.position, Quaternion.AngleAxis(-90, Vector3.right), PointContain.transform));
        }
        foreach (var item in buttons)
        {
            Text text = item.transform.Find("Text").GetComponent<Text>();
            if (text.text=="显示随机生成的引脚"||text.text=="返回1")
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
    }
    public void AffrimButton ()
    {
        //part2.SetActive(true);
        //part1.SetActive(false);
        //根据他那边计算线网最大数量

        //初始化线网集合
        //lineNets.Clear();
        //for (int i = 0; i < NowLineCount; i++)
        //{
        //    lineNets.Add(new LineNet());
        //}
    }
    public void SizeChange()
    {
        string[] temp = dropdown1.captionText.text.Split('*');
        AreaLength = int.Parse(temp[0]);
        AreaWidth = int.Parse(temp[1]);
    }
    public void PathwayChange()
    {
        PathwayCount = int.Parse(dropdown2.captionText.text);
    }
    public void FloorChange()
    {
        FloorCount = int.Parse(dropdown3.captionText.text);
    }
    public void LineCountChange()
    {
        NowLineCount = int.Parse(dropdown4.captionText.text);
    }
}
