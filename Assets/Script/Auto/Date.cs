//金属片宽度0.2spacing  导线宽度0.1spacing
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Auto;
using System.Runtime.InteropServices;
using System.IO;
public class Date : MonoBehaviour
{
  //  private static float lineX = 0.362777f;
    private static float lineY = 0.78f;
  //  private static float wifth = 0.1f;
    private static float lenghtmul=0;
    private static float widthtmul = 0;
    [DllImport("link 14")]
    private static extern void Func();
    [DllImport("link 14")]
    private static extern void CreateFunc();
    public static List<Net> lineNets = new List<Net>();
    public static List<Point> allBasePoint = new List<Point>();
    public static List<GameObject> Panels = new List<GameObject>();//层
    private List<GameObject> allBasePointInst = new List<GameObject>();
    private List<Pair> Pairs = new List<Pair>();
    private List<Pair> VerticalPairs = new List<Pair>();
    private List<Pair> RecoverPairs = new List<Pair>();


    //前期没写好，不然这一部分都应该在AutoGUIControl中比较符合
    public GameObject CreatNetsUIPanel;
    private string[] NetsPanelname = new string[] { "生成线网", "返回" };
    private Button[] buttons1;
    public Button[] buttons;
    private string[] names = new string[] { "确认", "显示随机生成的引脚", "返回1", "返回2" };



    public GameObject tartget;
    public Dropdown dropdown1;
    public Dropdown dropdown2;
    public Dropdown dropdown3;
    public Dropdown dropdown4;


    private GameObject PointPrefab;
    private GameObject PointContain;
    private GameObject PanelPrefab;
    private GameObject PanelContain;
    private GameObject Mental;
    private GameObject LinePrefab;
    private GameObject NetContain;


    //public GameObject basePanel;

    public const  float FloorSpacing  = 5;
    public int NowLineCount { get; private set; }
    public int AllMaxPoint { get { return allBasePoint.Count - 2 * (NowLineCount - 1); } private set { } }
    private int AreaLength { get; set; }
    private int AreaWidth { get; set; }
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

    public static float Mulsize { get; set; } = 10;

    public delegate void ButtonEvent();
    
    private void Awake()
    {
        ButtonEvent[] events1 = new ButtonEvent[] { CreatNets, BackButton3 };
        buttons1 = CreatNetsUIPanel.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < NetsPanelname.Length; i++)
        {
            buttons1[i].gameObject.transform.Find("Text").GetComponent<Text>().text = NetsPanelname[i];
            int a = i;
            buttons1[i].onClick.AddListener(() => events1[a]());
            if(NetsPanelname[i]==NetsPanelname[0])
            {
                buttons1[i].gameObject.SetActive(true);
            }
        }
        LinePrefab = Resources.Load<GameObject>("Line");
        Mental = Resources.Load<GameObject>("Mental");
        PanelContain = GameObject.Find("PanelContain");
        PanelPrefab = Resources.Load<GameObject>("AutoBasePanel");
        PointPrefab = Resources.Load<GameObject>("AutoImportantPoint");
        PointContain = GameObject.Find("PointContain");
        NetContain = GameObject.Find("NetContain");
        ButtonEvent[] events = new ButtonEvent[] { FirstSure,SecondSure,BackButton1,BackButton2};
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
        lenghtmul = AreaLength / 85f;
        widthtmul = AreaWidth / 40f;

        PanelPrefab.transform.localScale = new Vector3(lenghtmul, 1, widthtmul);

        PathwayCount = int.Parse(dropdown2.captionText.text);
        FloorCount = int.Parse(dropdown3.captionText.text);
        NowLineCount = int.Parse(dropdown4.captionText.text);

        Panels.Add(Instantiate(PanelPrefab, tartget.transform.position, Quaternion.identity, PanelContain.transform));
        Panels[0].GetComponent<AutoSinglePlane>().PathCount = PathwayCount;


        AutoSinglePlane.lenght = Mathf.Abs((Panels[0].transform.Find("leftnorth").transform.position - Panels[0].transform.Find("rightnorth").transform.position).magnitude);
        AutoSinglePlane.width= Mathf.Abs((Panels[0].transform.Find("leftnorth").transform.position - Panels[0].transform.Find("leftsorth").transform.position).magnitude);
        Mulsize = AreaLength / AutoSinglePlane.lenght;
    }
    private void BackButton3()
    {
        buttons[3].gameObject.SetActive(true);
        buttons1[1].gameObject.SetActive(false);
        buttons1[0].gameObject.SetActive(true);
        foreach (var net in lineNets)
        {
            net.PathPair.Clear();
        }
        for (int i = 1; i < Panels.Count; i++)
        {
            Destroy(Panels[i]);
        }
        Panels.RemoveRange(1, Panels.Count-1);

        
    }
    private void SecondSure()
    {
        //button1.gameObject.SetActive(false);
        foreach (var item in allBasePointInst)
        {
            item.SetActive(false);
        }
        lineNets.ForEach(
            net =>
            {
                for (int i = 0; i < net.Points.Count; i++)
                {
                    int k= allBasePoint.IndexOf(net.Points[i]);
                    if(k!=-1)
                    {
                        allBasePointInst[k].SetActive(true);
                        net.PointsInstant.Add(allBasePointInst[k]);
                    }
                }
            }
            );
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
        CreatNetsUIPanel.SetActive(true);
        foreach (var button in buttons1)
        {
            if(button.gameObject.GetComponentInChildren<Text>().text==NetsPanelname[0])
            {
                button.gameObject.SetActive(true);
            }
        }

    }

    private void BackButton1()
    {
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
        foreach (var panel in Panels)
        {
            Destroy(panel);
        }
        lineNets.Clear();
        allBasePointInst.Clear();
        allBasePoint.Clear();
        Panels.Clear();
        Panels.Add(Instantiate(PanelPrefab, tartget.transform.position, Quaternion.identity, PanelContain.transform));
        Panels[0].GetComponent<AutoSinglePlane>().PathCount = PathwayCount;
        dropdown1.enabled = true;
        dropdown2.enabled = true;
        dropdown3.enabled = true;
        dropdown4.enabled = true;
    }

    private void BackButton2()
    {
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
        CreatNetsUIPanel.SetActive(false);
        buttons1[0].gameObject.SetActive(false);
    }




    /// <summary>
    /// 生成线网
    /// </summary>
    private void CreatNets()
    {
        try
        {
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(false);
            }
            if (Panels.Count >= 1)
            {
                //生成层
                for (int i = 0; i < FloorCount - 1; i++)
                {
                    GameObject single = Instantiate(PanelPrefab, Panels[0].transform.position+(i+1)*new Vector3(0,1,0), Quaternion.identity, PanelContain.transform);
                    single.GetComponent<AutoSinglePlane>().Floor = i + 1;
                    if (i % 2 == 0)
                    {
                        single.GetComponent<AutoSinglePlane>().Type = AutoSinglePlane.PathType.Vertical;
                    }
                    else
                    {
                        single.GetComponent<AutoSinglePlane>().Type = AutoSinglePlane.PathType.Horizontal;
                    }
                    Panels.Add(single);
                }

                Debug.Log("--------------------------");
                //设置每层的pathway
                int tempPathway = PathwayCount;
                for (int i = 0; i < Panels.Count; i++)
                {
                    if (i % 3 == 0 && i != 0)
                    {
                        tempPathway--;
                    }
                    Panels[i].GetComponent<AutoSinglePlane>().PathCount = tempPathway;
                }


                Debug.Log("--------------------------");


                //生成所有通孔
                for (int i = 1; i < FloorCount; i++)
                {
                    AutoSinglePlane nowautoSinglePlane = Panels[i].GetComponent<AutoSinglePlane>();
                    AutoSinglePlane formerauroPlane = Panels[i - 1].GetComponent<AutoSinglePlane>();
                    #region 水平
                    if (nowautoSinglePlane.Type == AutoSinglePlane.PathType.Horizontal)
                    {
                        if (nowautoSinglePlane.PathCount == formerauroPlane.PathCount)
                        {
                            int t = formerauroPlane.points[0].Number + formerauroPlane.points.Count;
                            for (int k = 0; k < formerauroPlane.PathCount; k++)
                            {
                                for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                                {
                                    Point point = new Point(
                                        formerauroPlane.Spacing * 0.5 + k * formerauroPlane.Spacing,
                                        nowautoSinglePlane.Spacing * 0.5 + j * nowautoSinglePlane.Spacing,
                                        i, t++);
                                    nowautoSinglePlane.points.Add(point);
                                }
                            }
                        }
                        if (nowautoSinglePlane.PathCount != formerauroPlane.PathCount)
                        {
                            int t = formerauroPlane.points[0].Number + formerauroPlane.points.Count;
                            for (int k = 0; k < formerauroPlane.PathCount; k++)
                            {
                                for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                                {
                                    Point point = new Point(
                                        formerauroPlane.Spacing * 0.5 + k * formerauroPlane.Spacing,
                                        nowautoSinglePlane.Spacing * 0.5 + j * nowautoSinglePlane.Spacing,
                                        i - 1, t++);
                                    formerauroPlane.points.Add(point);
                                }
                            }
                            t = formerauroPlane.points[0].Number + formerauroPlane.points.Count;
                            for (int k = 0; k < formerauroPlane.PathCount; k++)
                            {
                                for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                                {
                                    Point point = new Point(
                                        formerauroPlane.Spacing * 0.5 + k * formerauroPlane.Spacing,
                                        nowautoSinglePlane.Spacing * 0.5 + j * nowautoSinglePlane.Spacing,
                                        i, t++);
                                    nowautoSinglePlane.points.Add(point);
                                }
                            }

                            if (i != FloorCount - 1)
                            {
                                AutoSinglePlane afterautoSinglePlane = Panels[i + 1].GetComponent<AutoSinglePlane>();
                                t = nowautoSinglePlane.points.Count + nowautoSinglePlane.points[0].Number;
                                for (int k = 0; k < nowautoSinglePlane.PathCount; k++)
                                {
                                    for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                                    {
                                        Point point = new Point(
                                            afterautoSinglePlane.Spacing * 0.5 + k * afterautoSinglePlane.Spacing,
                                            nowautoSinglePlane.Spacing * 0.5 + j * nowautoSinglePlane.Spacing,
                                            i, t++);
                                        nowautoSinglePlane.points.Add(point);
                                    }
                                }
                            }

                        }
                    }
                    #endregion

                    Debug.Log("------");
                    #region 垂直
                    if (nowautoSinglePlane.Type == AutoSinglePlane.PathType.Vertical)
                    {
                        if (nowautoSinglePlane.PathCount == formerauroPlane.PathCount)
                        {
                            int t = formerauroPlane.points[0].Number + formerauroPlane.points.Count;
                            for (int k = 0; k < nowautoSinglePlane.PathCount; k++)
                            {
                                for (int j = 0; j < formerauroPlane.PathCount; j++)
                                {
                                    Point point = new Point(
                                        nowautoSinglePlane.Spacing * 0.5 + k * nowautoSinglePlane.Spacing,
                                        formerauroPlane.Spacing * 0.5 + j * formerauroPlane.Spacing,
                                        i, t++);
                                    nowautoSinglePlane.points.Add(point);
                                }
                            }
                        }
                        if (nowautoSinglePlane.PathCount != formerauroPlane.PathCount)
                        {
                            int t = formerauroPlane.points[0].Number + formerauroPlane.points.Count;
                            for (int k = 0; k < nowautoSinglePlane.PathCount; k++)
                            {
                                for (int j = 0; j < formerauroPlane.PathCount; j++)
                                {
                                    Point point = new Point(
                                        nowautoSinglePlane.Spacing * 0.5 + k * nowautoSinglePlane.Spacing,
                                        formerauroPlane.Spacing * 0.5 + j * formerauroPlane.Spacing,
                                        i - 1, t++);
                                    formerauroPlane.points.Add(point);
                                }
                            }
                            t = formerauroPlane.points[0].Number + formerauroPlane.points.Count;
                            for (int k = 0; k < nowautoSinglePlane.PathCount; k++)
                            {
                                for (int j = 0; j < formerauroPlane.PathCount; j++)
                                {
                                    Point point = new Point(
                                        nowautoSinglePlane.Spacing * 0.5 + k * nowautoSinglePlane.Spacing,
                                        formerauroPlane.Spacing * 0.5 + j * formerauroPlane.Spacing,
                                        i, t++);
                                    nowautoSinglePlane.points.Add(point);
                                }
                            }

                            if (i != FloorCount - 1)
                            {
                                AutoSinglePlane afterautoSinglePlane = Panels[i + 1].GetComponent<AutoSinglePlane>();
                                t = nowautoSinglePlane.points.Count + nowautoSinglePlane.points[0].Number;
                                for (int k = 0; k < nowautoSinglePlane.PathCount; k++)
                                {
                                    for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                                    {
                                        Point point = new Point(
                                            nowautoSinglePlane.Spacing * 0.5 + k * nowautoSinglePlane.Spacing,
                                              afterautoSinglePlane.Spacing * 0.5 + j * afterautoSinglePlane.Spacing,
                                            i, t++);
                                        nowautoSinglePlane.points.Add(point);
                                    }
                                }
                            }



                        }
                    }
                    #endregion
                }

                Debug.Log("--------------------------");
                #region 调试程序
                foreach (var item in Panels)
                {
                    Debug.Log(item.GetComponent<AutoSinglePlane>());
                }
                #endregion


                //设置Pairs对
                for (int i = 1; i < Panels.Count - 1; i++)
                {
                    AutoSinglePlane nowautoSinglePlane = Panels[i].GetComponent<AutoSinglePlane>();
                    AutoSinglePlane fowardautoSinglePlane = Panels[i - 1].GetComponent<AutoSinglePlane>();
                    AutoSinglePlane afterautoSinglePlane = Panels[i + 1].GetComponent<AutoSinglePlane>();
                    if (nowautoSinglePlane.PathCount == fowardautoSinglePlane.PathCount &&
                       afterautoSinglePlane.PathCount == nowautoSinglePlane.PathCount)
                    {
                        ///竖直
                        foreach (var point in nowautoSinglePlane.points)
                        {
                            Pair pair = new Pair(point.Number - nowautoSinglePlane.points.Count,
                                point.Number);
                            Pairs.Add(pair);
                            VerticalPairs.Add(pair);
                        }
                        int start = nowautoSinglePlane.points[0].Number;
                        if (nowautoSinglePlane.Type == AutoSinglePlane.PathType.Horizontal)
                        {
                            for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                            {
                                for (int k = 0; k < nowautoSinglePlane.PathCount - 1; k++)
                                {
                                    //水平
                                    Pair pair = new Pair(start + k * nowautoSinglePlane.PathCount + j,
                                        start + (k + 1) * nowautoSinglePlane.PathCount + j);
                                    Pairs.Add(pair);
                                }
                            }
                        }
                        else
                        {


                            for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                            {
                                for (int k = 0; k < nowautoSinglePlane.PathCount - 1; k++)
                                {
                                    //垂直
                                    Pair pair = new Pair(start + k + j * nowautoSinglePlane.PathCount,
                                        start + k + 1 + j * nowautoSinglePlane.PathCount);
                                    Pairs.Add(pair);
                                }
                            }


                        }

                    }
                    else if (nowautoSinglePlane.PathCount == fowardautoSinglePlane.PathCount &&
                        afterautoSinglePlane.PathCount != nowautoSinglePlane.PathCount)
                    {
                        for (int k = 0; k < nowautoSinglePlane.PathCount * fowardautoSinglePlane.PathCount; k++)
                        {
                            //竖直
                            int second = nowautoSinglePlane.points[k].Number;
                            int first = nowautoSinglePlane.points[k].Number - nowautoSinglePlane.PathCount * fowardautoSinglePlane.PathCount;
                            Pair pair = new Pair(first, second);
                            Pairs.Add(pair);
                            VerticalPairs.Add(pair);

                        }
                        if (nowautoSinglePlane.Type == AutoSinglePlane.PathType.Horizontal)
                        {
                            for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                            {
                                int start = nowautoSinglePlane.points[0].Number + j;
                                int t = 0;
                                int s = 0;
                                for (int k = 0; k < nowautoSinglePlane.PathCount + afterautoSinglePlane.PathCount - 1; k++)
                                {
                                    //水平
                                    if (k % 2 == 0)
                                    {
                                        Pair pair = new Pair(start + s * nowautoSinglePlane.PathCount,
                                            start + fowardautoSinglePlane.points.Count + t * nowautoSinglePlane.PathCount);
                                        Pairs.Add(pair);
                                    }
                                    else
                                    {
                                        Pair pair = new Pair(start + fowardautoSinglePlane.points.Count + (t++) * nowautoSinglePlane.PathCount,
                                            start + (++s) * nowautoSinglePlane.PathCount);
                                        Pairs.Add(pair);
                                    }
                                }
                            }
                        }
                        else
                        {
                            int plus = fowardautoSinglePlane.points.Count;
                            for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                            {
                                int start = nowautoSinglePlane.points[0].Number + j * nowautoSinglePlane.PathCount;
                                int t = 0;
                                int s = 0;
                                for (int k = 0; k < nowautoSinglePlane.PathCount + afterautoSinglePlane.PathCount - 1; k++)
                                {
                                    //垂直
                                    if (k % 2 == 0)
                                    {
                                        Pair pair = new Pair(start + s, start + plus + t);
                                        Pairs.Add(pair);
                                    }
                                    else
                                    {
                                        Pair pair = new Pair(start + plus + t++, start + (++s));
                                        Pairs.Add(pair);
                                    }
                                }
                                plus--;
                            }
                        }

                    }
                    else if (nowautoSinglePlane.PathCount != fowardautoSinglePlane.PathCount)
                    {
                        for (int k = 0; k < nowautoSinglePlane.PathCount * fowardautoSinglePlane.PathCount; k++)
                        {
                            //竖直
                            int second = nowautoSinglePlane.points[k].Number;
                            int first = nowautoSinglePlane.points[k].Number - nowautoSinglePlane.PathCount * fowardautoSinglePlane.PathCount;
                            Pair pair = new Pair(first, second);
                            Pairs.Add(pair);
                            VerticalPairs.Add(pair);

                        }


                        if (nowautoSinglePlane.Type == AutoSinglePlane.PathType.Horizontal)
                        {
                            for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                            {//水平
                                int start = nowautoSinglePlane.points[0].Number + j;
                                int t = 0;
                                int s = 0;
                                for (int k = 0; k < nowautoSinglePlane.PathCount + fowardautoSinglePlane.PathCount - 1; k++)
                                {

                                    if (k % 2 == 0)
                                    {
                                        Pair pair = new Pair(start + s * nowautoSinglePlane.PathCount,
                                            start + nowautoSinglePlane.PathCount * fowardautoSinglePlane.PathCount +
                                            t * nowautoSinglePlane.PathCount);
                                        Pairs.Add(pair);
                                    }
                                    else
                                    {
                                        Pair pair = new Pair(start + nowautoSinglePlane.PathCount * fowardautoSinglePlane.PathCount +
                                            (t++) * nowautoSinglePlane.PathCount,
                                            start + (++s) * nowautoSinglePlane.PathCount);
                                        Pairs.Add(pair);
                                    }
                                }
                            }
                        }
                        else
                        {

                            int plus = fowardautoSinglePlane.PathCount * nowautoSinglePlane.PathCount;
                            for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                            {//垂直
                                int start = nowautoSinglePlane.points[0].Number + j * fowardautoSinglePlane.PathCount;
                                int t = 0;
                                int s = 0;
                                for (int k = 0; k < nowautoSinglePlane.PathCount + fowardautoSinglePlane.PathCount - 1; k++)
                                {

                                    if (k % 2 == 0)
                                    {
                                        Pair pair = new Pair(start + s,
                                            start + plus + t);
                                        Pairs.Add(pair);
                                    }
                                    else
                                    {
                                        Pair pair = new Pair(start + plus + t++,
                                            start + (++s));
                                        Pairs.Add(pair);
                                    }
                                }
                                plus--;
                            }
                        }





                    }
                }

                Debug.Log("--------------------------");

                {
                    AutoSinglePlane nowautoSinglePlane = Panels[Panels.Count - 1].GetComponent<AutoSinglePlane>();
                    AutoSinglePlane fowardautoSinglePlane = Panels[Panels.Count - 1].GetComponent<AutoSinglePlane>();
                    for (int k = 0; k < nowautoSinglePlane.PathCount * fowardautoSinglePlane.PathCount; k++)
                    {
                        //竖直
                        int second = nowautoSinglePlane.points[k].Number;
                        int first = nowautoSinglePlane.points[k].Number - nowautoSinglePlane.PathCount * fowardautoSinglePlane.PathCount;
                        Pair pair = new Pair(first, second);
                        Pairs.Add(pair);
                        VerticalPairs.Add(pair);

                    }
                    if (nowautoSinglePlane.Type == AutoSinglePlane.PathType.Horizontal)
                    {
                        for (int i = 0; i < nowautoSinglePlane.PathCount; i++)
                        {
                            for (int j = 0; j < fowardautoSinglePlane.PathCount - 1; j++)
                            {
                                Pair pair = new Pair(nowautoSinglePlane.points[0].Number + i + j * nowautoSinglePlane.PathCount,
                                    nowautoSinglePlane.points[0].Number + i + (j + 1) * nowautoSinglePlane.PathCount);
                                Pairs.Add(pair);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < nowautoSinglePlane.PathCount; i++)
                        {
                            for (int j = 0; j < fowardautoSinglePlane.PathCount - 1; j++)
                            {
                                Pair pair = new Pair(nowautoSinglePlane.points[0].Number + i * fowardautoSinglePlane.PathCount + j,
                                    nowautoSinglePlane.points[0].Number + i * fowardautoSinglePlane.PathCount + (j + 1));
                                Pairs.Add(pair);
                            }
                        }
                    }
                }

                Debug.Log("--------------------------");
                #region Pair对数
                Debug.Log("通孔对对数：" + Pairs.Count);
                #endregion

                #region 遍历Pair对
                string str = "Pair对：\n";
                foreach (var pair in Pairs)
                {
                    str += pair.ToString() + "\n";
                }
                Debug.Log(str);


                #endregion


                #region 输出通孔数量
                int sum = 0;
                foreach (var item in Panels)
                {
                    sum += item.GetComponent<AutoSinglePlane>().points.Count;
                }
                Debug.Log("通孔个数：" + sum);
                #endregion

                #region 输出线网个数
                Debug.Log("线网个数为：" + lineNets.Count);
                #endregion

                #region 输出每个线网引脚数和选中编号
                int l = 1;
                foreach (var net in lineNets)
                {
                    string str1 = "线网" + l.ToString() + "\n";
                    str1 += net.Points.Count.ToString() + "\n";
                    foreach (var point in net.Points)
                    {
                        str1 += point.Number + " ";
                    }
                    Debug.Log(str1);
                }
                #endregion


            }
            foreach (var panel in Panels)
            {
                panel.GetComponent<AutoSinglePlane>().SetEveryPathPoints();
                panel.GetComponent<AutoSinglePlane>().SetkeyValuePairs();
            }

            File_Writer.CreateClass createClass = new File_Writer.CreateClass();
            foreach (var panel in Panels)
            {
                AutoSinglePlane autoSinglePlane = panel.GetComponent<AutoSinglePlane>();
                createClass.AllPointCount += autoSinglePlane.points.Count;
            }
            createClass.Pairs.AddRange(Pairs);
            createClass.Nets.AddRange(lineNets);
            File_Writer.CreatWriter(createClass);

            CreateFunc();

            File_Reader.CreatResultRead(lineNets,RecoverPairs);
            ChangePairsToLine();
            CreatLineInstance();
            Limit limit = new Limit();
            Search(limit);
            File_Writer.LimitWireWriter(limit);
            File_Writer.LimitMentalWriter(limit);
            File_Writer.LimitMental1Writer(limit);
            buttons1[0].gameObject.SetActive(false);
            buttons1[1].gameObject.SetActive(true);
        }
        catch (System.Exception)
        {

            throw;
        }
    }



    public void ChangePairsToLine()
    {

        #region 调试
        print("------------------------");
        foreach (var panel in Panels)
        {
            foreach (var item in panel.GetComponent<AutoSinglePlane>().KeyValuePairs)
            {
                foreach (var t in item.Keys)
                {
                    print("Number为:"+t.Number+"Point为"+t);
                }
            }
        }
        print("------------------------");
        #endregion
        //print("--------------");
        //foreach (var net in lineNets)
        //{
        //    foreach (var pair in net.PathPair)
        //    {
        //        Debug.Log(pair);
        //    }
        //}
        //print("--------------");

        int netNumber=1;
        foreach (var net in lineNets)
        {
            foreach (var pair in net.PathPair)
            {
                bool IsFirst = false;
                bool IsSecond = false;

                foreach (var panel in Panels)
                {
                    List<Dictionary<Point, List<int>>> keyValuePairs = panel.GetComponent<AutoSinglePlane>().KeyValuePairs;
                    for (int i=0;i < keyValuePairs.Count;i++)
                    {
                        Point point1=null;
                        Point point2=null;
                        foreach (var item in panel.GetComponent<AutoSinglePlane>().KeyValuePairs[i])
                        {
                            if(pair.First==item.Key.Number)
                            {
                                point1 = item.Key;
                                IsFirst = true;
                            }
                            else if(pair.Second == item.Key.Number)
                            {
                                point2 = item.Key;
                                IsSecond = true;
                            }
                            if(IsFirst&&IsSecond)
                            {
                                break;
                            }
                        }
                        if(point1!=null)
                        {
                            List<int> vs = keyValuePairs[i][point1];
                            if(vs[0]==0)
                            {
                                vs[0] = netNumber;
                            }
                            else
                            {
                                vs.Add(netNumber);
                            }

                        }
                        if(point2!=null)
                        {
                            List<int> vs = keyValuePairs[i][point2];
                            if (vs[0] == 0)
                            {
                                vs[0] = netNumber;
                            }
                            else 
                            {
                                vs.Add(netNumber);
                            }
                        }
                        if(IsFirst&&IsSecond)
                        {
                            break;
                        }
                    }
                    if(IsFirst||IsSecond)
                    {
                        break;
                    }
                }

            }
            netNumber++;
        }
        #region 调试
        Debug.Log("----------------------");
        foreach (var panel in Panels)
        {
            foreach (var valuePairs in panel.GetComponent<AutoSinglePlane>().KeyValuePairs)
            {
                foreach (var pair in valuePairs)
                {
                    Debug.Log("Key为:" + pair.Key.ToString());
                    foreach (var value in pair.Value)
                    {
                        print("Value为:" + value.ToString());
                    }
                }
            }
        }
        #endregion

        #region 得到每个线网水平的导线
        for (int i = 0; i < lineNets.Count; i++)
        {

            for (int j=1;j<Panels.Count;j++)
            {
                AutoSinglePlane autoSinglePlane = Panels[j].GetComponent<AutoSinglePlane>();
                foreach (var keyValuePairs in autoSinglePlane.KeyValuePairs)
                {
                    List<Point> points = new List<Point>();
                    foreach (var item in keyValuePairs)
                    {
                        if (item.Value.Contains(i+1))
                        {
                            points.Add(item.Key);
                        }
                        else
                        {
                            if (points.Count >= 1)
                            {
                                lineNets[i].LinePoint.Add(points);
                            }
                            points = new List<Point>();
                        }
                    }
                    if(points.Count!=0)
                    {
                        lineNets[i].LinePoint.Add(points);
                    }
                }
            }
        }
        #endregion

        #region 得到每个线网垂直的导线
        foreach (var net in lineNets)
        {
            foreach (var pair  in net.PathPair)
            {
                if(VerticalPairs.Contains(pair))
                {
                    net.VerticalPairs.Add(pair);
                    int t = 0;
                    List<Point> points = new List<Point>();
                    foreach (var panel in Panels)
                    {
                        foreach (var point in panel.GetComponent<AutoSinglePlane>().points)
                        {
                               if(point.Number==pair.First||point.Number==pair.Second)
                            {
                                points.Add(point);
                            }
                               if(t==2)
                            {
                                break;
                            }
                        }
                        if(t==2)
                        {
                            break;
                        }
                    }
                    if(points[0].Floor<points[1].Floor)
                    {
                        Point temppoint = new Point(points[0]);
                        points[0] = points[1];
                        points[1] = temppoint;
                    }
                    net.VerticalPoints.Add(points);
                }
            }
        }




        #endregion


        #region 调试


        //foreach (var net in lineNets)
        //{
        //    foreach (var pair in net.VerticalPairs)
        //    {
        //        print(pair);
        //    }
        //}
        int count = 1;
        foreach (var net in lineNets)
        {
            Debug.Log($"线网{count}有:");
            for (int i = 0; i < net.LinePoint.Count; i++)
            {
                Debug.Log($"水平导线{(i + 1)}为：");
                foreach (var point in net.LinePoint[i])
                {
                    print(point);
                }
            }
            for (int i = 0; i < net.VerticalPoints.Count; i++)
            {
                Debug.Log($"垂直导线{(i + 1)}为：");
                foreach (var point in net.VerticalPoints[i])
                {
                    print(point);
                }
            }
            count++;
        }
        #endregion
    }



    public void CreatLineInstance()
    {
        print("--------生成线网------------");
        int count = 1;
        foreach (var net in lineNets)
        {
            GameObject t = new GameObject()
            { name = "LineNet" + count.ToString() };
            t.transform.SetParent(NetContain.transform);
            foreach (var horizon in net.LinePoint)
            {
                Point point1 = horizon[0];
                Point point2 = horizon[horizon.Count - 1];
                //X相等，即为垂直层
                if (point1.GetX() == point2.GetX()&&horizon.Count!=1)
                {
                    print("垂直");
                    print(point1);
                    print(point2);
                    print("-----");
                    double y = (point1.GetY() + point2.GetY()) / 2;
                    GameObject @object = Instantiate(Mental,
                        Panels[0].transform.Find("leftsorth").transform.position + point1.Floor * new Vector3(0, 1, 0)
                        + new Vector3((float)point1.GetX(), 0, (float)y)
                        , Quaternion.Euler(90,0,0), t.transform);
                    float ymul = Mathf.Abs((float)point1.GetY() - (float)point2.GetY()) / lineY;
                    @object.transform.localScale = new Vector3(1, ymul, 1);
                    net.HoriLine.Add(@object);
                    net.LinePointInstant.Add(@object);
                    
                }
                //Y相等即为水平层
                if (point1.GetY() == point2.GetY() && horizon.Count != 1)
                {
                    print("水平");
                    print(point1);
                    print(point2);
                    print("-----");
                    double x = (point1.GetX() + point2.GetX()) / 2;
                    GameObject @object = Instantiate(Mental,
                        Panels[0].transform.Find("leftsorth").transform.position + point1.Floor * new Vector3(0, 1, 0)
                        + new Vector3((float)x, 0,(float)point1.GetY())
                        , Quaternion.Euler(90,0,90), t.transform);
                    float ymul = Mathf.Abs((float)point1.GetX() - (float)point2.GetX()) / lineY;
                    @object.transform.localScale = new Vector3(1, ymul, 1);
                    net.HoriLine.Add(@object);
                    net.LinePointInstant.Add(@object);
                }
            }
            foreach (var vertical in net.VerticalPoints)
            {
                Point point1 = vertical[0];
                Point point2 = vertical[vertical.Count - 1];
                float floor = (point1.Floor + point2.Floor) / (float)2;
                GameObject @object = Instantiate(LinePrefab,
                    Panels[0].transform.Find("leftsorth").transform.position + floor * new Vector3(0, 1, 0)
                    + new Vector3((float)point1.GetX(), 0, (float)point1.GetY())
                    , Quaternion.Euler(90, 0, 0), t.transform);
                Vector3 upVector3 = Panels[0].transform.Find("leftsorth").transform.position + point1.Floor * new Vector3(0, 1, 0)
                    + new Vector3((float)point1.GetX(), 0, (float)point1.GetY());
                Vector3 underVector3 = Panels[0].transform.Find("leftsorth").transform.position + point2.Floor * new Vector3(0, 1, 0)
                    + new Vector3((float)point2.GetX(), 0, (float)point2.GetY());
                bool IsUnder = false;
                bool IsUp = false;
                if(point1.Floor==0)
                {
                    IsUp = true;
                }
                if(point2.Floor==0)
                {
                    IsUnder = true;
                }
                foreach (var mental in net.Mental)
                {
                    if(mental.transform.position==upVector3)
                    {
                        IsUp = true;
                    }
                    if(mental.transform.position==underVector3)
                    {
                        IsUnder = true;
                    }
                }
                if(!IsUnder)
                {
                    GameObject under = Instantiate(PointPrefab,
                    underVector3
                     , Quaternion.Euler(-90, 0, 0), @object.transform);
                    under.name = "under";
                    net.Mental.Add(under);
                }
                if(!IsUp)
                {
                    GameObject up = Instantiate(PointPrefab,
                    upVector3
                     , Quaternion.Euler(-90, 0, 0), @object.transform);
                    up.name = "up";
                    net.Mental.Add(up);
                }
                net.VerticalInstance.Add(@object);
            }
            count++;
        }


        print("--------生成线网结束------------");
    }
    /// <summary>
    /// 检查短路
    /// </summary>
    public void Search(Limit limit)
    {

        //导线和导线
        //对于每个导线
        for (int i= 0;i < lineNets.Count;i++)
        {
            foreach (var line in lineNets[i].LinePoint)
            {
                if(line.Count!=1)
                {
                    Line_RecoverLine line_RecoverLine = new Line_RecoverLine()
                    { Line = line };
                    //这条导线短路的段
                    List<Pair> ShortPair = new List<Pair>();
                    foreach (var pair in RecoverPairs)
                    {
                        if (ContainPair(line, pair))
                        {
                            ShortPair.Add(pair);
                        }
                    }
                    HashSet<List<Point>> lines = new HashSet<List<Point>>();
                    for (int k = 0; k < lineNets.Count; k++)
                    {
                        //不同线网时才进入
                        if (k != i)
                        {
                            foreach (var line1 in lineNets[k].LinePoint)
                            {
                                foreach (var pair in ShortPair)
                                {
                                    if (ContainPair(line1, pair))
                                    {
                                        lines.Add(line1);
                                    }
                                }
                            }
                        }
                    }
                    line_RecoverLine.Recoverlines = new List<List<Point>>(lines);
                    limit.Line_recoverLines.Add(line_RecoverLine);
                }
            }

            HashSet<List<Point>> Verticallines = new HashSet<List<Point>>();
            foreach (var net in lineNets)
            {
                foreach (var line in net.VerticalPoints)
                {
                    Verticallines.Add(line);
                }
            }
            foreach (var line in Verticallines)
            {
                Line_RecoverLine line_RecoverLine = new Line_RecoverLine()
                { Line = line};
                limit.Line_recoverLines.Add(line_RecoverLine);
            }
        }
        #region 调试
        foreach (var line_lines in limit.Line_recoverLines)
        {
            Debug.Log("导线" + line_lines.Line[0]+" "+line_lines.Line[line_lines.Line.Count-1]);
            foreach (var line in line_lines.Recoverlines)
            {
                print("短路导线:");
                print(line[0] + " " + line[line.Count - 1]);
            }
        }
        #endregion


        //金属片和导线
        for (int i = 0; i < lineNets.Count; i++)
        {
            List<Point> mentals = new List<Point>();
            //包括底座引脚的
            foreach (var line in lineNets[i].VerticalPoints)
            {
                if(!mentals.Contains(line[0]))
                {
                    Point point1 = line[0];
                    Line_RecoverPoint line_RecoverPoint1 = new Line_RecoverPoint()
                    { MentalPoint = point1 };
                    for (int k = 0; k < lineNets.Count; k++)
                    {
                        if (k != i)
                        {
                            foreach (var line1 in lineNets[k].LinePoint)
                            {
                                if (IsShortMental_Line(line1,point1))
                                {
                                    line_RecoverPoint1.Lines.Add(line1);
                                }
                            }
                        }
                    }
                    limit.Line_recoverPoints.Add(line_RecoverPoint1);
                    mentals.Add(point1);
                }
                if (!mentals.Contains(line[1]))
                {
                    Point point2 = line[1];
                    Line_RecoverPoint line_RecoverPoint2 = new Line_RecoverPoint()
                    {MentalPoint = point2};
                    for (int k = 0; k < lineNets.Count; k++)
                    {
                        if (k != i)
                        {
                            foreach (var line1 in lineNets[k].LinePoint)
                            {
                                if (IsShortMental_Line(line1, point2))
                                {
                                    line_RecoverPoint2.Lines.Add(line1);
                                }
                            }
                        }
                    }
                    limit.Line_recoverPoints.Add(line_RecoverPoint2);
                    mentals.Add(point2);
                }
            }
        }
        #region 调试
        print("-------------------");
        foreach (var point_line in limit.Line_recoverPoints)
        {
            Debug.Log("金属片" + point_line.MentalPoint);
            foreach (var line in point_line.Lines)
            {
                print("短路导线");
                print(line[0] + " " + line[line.Count - 1]);
            }
        }
        print("----------------");
        #endregion
        ////金属片和金属片
        for (int i = 0; i < lineNets.Count; i++)
        {
            List<Point> mentals = new List<Point>();
            //包括底座引脚，当重合的时候才时短路，可能的限制距离
            foreach (var line in lineNets[i].VerticalPoints)
            {
                if (!mentals.Contains(line[0]))
                {
                    Point point1 = line[0];
                    Point_RecoverPoint point_RecoverPoint1 = new Point_RecoverPoint()
                    {MentalPoint = point1};
                    HashSet<Point> shortMentals = new HashSet<Point>();
                    for (int j = 0; j < lineNets.Count; j++)
                    {
                        if (i != j)
                        {
                            foreach (var line1 in lineNets[j].VerticalPoints)
                            {
                                if (IsShortMental_Mental(line1[0],point1))
                                {
                                    shortMentals.Add(line1[0]);
                                }
                                if (IsShortMental_Mental(line1[1], point1))
                                {
                                    shortMentals.Add(line1[1]);
                                }
                            }
                        }
                    }
                    point_RecoverPoint1.MentalPoints = new List<Point>(shortMentals);
                    limit.Point_recoverPoints.Add(point_RecoverPoint1);
                    mentals.Add(point1);
                }
                if (!mentals.Contains(line[1]))
                {
                    Point point2 = line[1];
                    Point_RecoverPoint point_RecoverPoint2 = new Point_RecoverPoint()
                    {MentalPoint = point2};
                    HashSet<Point> shortMentals = new HashSet<Point>();
                    for (int j = 0; j < lineNets.Count; j++)
                    {
                        if (i != j)
                        {
                            foreach (var line1 in lineNets[j].VerticalPoints)
                            {
                                if (IsShortMental_Mental(line1[0], point2))
                                {
                                    shortMentals.Add(line1[0]);
                                }
                                if (IsShortMental_Mental(line1[1], point2))
                                {
                                    shortMentals.Add(line1[1]);
                                }
                            }
                        }
                    }
                    point_RecoverPoint2.MentalPoints = new List<Point>(shortMentals);
                    limit.Point_recoverPoints.Add(point_RecoverPoint2);
                    mentals.Add(point2);
                }

            }
        }
        #region 调试
        foreach (var point_RecoverPoint in limit.Point_recoverPoints)
        {
            Debug.Log("金属片" + point_RecoverPoint.MentalPoint);
            foreach (var point in point_RecoverPoint.MentalPoints)
            {
                print("短路金属片");
                print(point);
            }
        }
        #endregion
    }

    private bool ContainPair(List<Point> line,Pair pair)
    {
        bool IsFirst = false;
        bool IsSecond = false;
        foreach (var point in line)
        {
            if(point.Number==pair.First)
            {
                IsFirst = true;
            }
            if(point.Number==pair.Second)
            {
                IsSecond = true;
            }
            if(IsFirst&&IsSecond)
            {
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// 金属片和导线是不是短路
    /// </summary>
    /// <returns></returns>
    private bool IsShortMental_Line(List<Point> line,Point Mental)
    {
        if(line.Count<=1)
        {
            return false;
        }
        if(line[0].Floor==Mental.Floor)
        {
            int floor = line[0].Floor;
            float spacing = Panels[floor].GetComponent<AutoSinglePlane>().Spacing;
            //垂直导线
            if (line[0].GetX()==line[line.Count-1].GetX())
            {
               if (line[0].GetY()<Mental.GetY()+0.1*spacing&&
                    line[line.Count-1].GetY()<Mental.GetY()-spacing*0.1&&
                    Mathf.Abs((float)line[0].GetX()-(float)Mental.GetX())
                    <spacing*0.3)
                {
                    return true;
                }
            }
            //水平导线
            if(line[0].GetY()==line[line.Count-1].GetY())
            {
                if (line[0].GetX() < Mental.GetX() + 0.1 * spacing &&
                    line[line.Count - 1].GetX() < Mental.GetX() - spacing * 0.1 &&
                    Mathf.Abs((float)line[0].GetY() - (float)Mental.GetY())
                    < spacing * 0.3)
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 金属和金属是否短路
    /// </summary>
    /// <param name="Mental1"></param>
    /// <param name="Mental2"></param>
    /// <returns></returns>
    private bool IsShortMental_Mental(Point Mental1,Point Mental2)
    {
        if(Mental1.Floor==Mental2.Floor)
        {
            float spacing = Panels[Mental1.Floor].GetComponent<AutoSinglePlane>().Spacing;
            if (Mathf.Abs((float)Mental1.GetX()-(float)Mental2.GetX())<0.2*spacing)
            {
                return true;
            }
            if (Mathf.Abs((float)Mental1.GetY() - (float)Mental2.GetY()) < 0.2 * spacing)
            {
                return true;
            }
        }
        return false;
    }
    

    /// <summary>
    /// 确认按钮
    /// </summary>
    private void FirstSure()
    {

        dropdown1.enabled = false;
        dropdown2.enabled = false;
        dropdown3.enabled = false;
        dropdown4.enabled = false;
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

        Panels[0].GetComponent<AutoSinglePlane>().points.AddRange(allBasePoint);
        #region 调试程序
        //foreach (var item in allBasePoint)
        //{
        //    Debug.Log(item.Number);
        //}
        //foreach (var net in lineNets)
        //{
        //    foreach (var point in net.Points)
        //    {
        //        Debug.Log(point.Number);
        //    }
        //}



        #endregion


        foreach (var point in allBasePoint)
        {
            allBasePointInst.Add(Instantiate(PointPrefab, point.GetVector3() + Panels[0].transform.Find("leftsorth").transform.position, Quaternion.AngleAxis(-90, Vector3.right), PointContain.transform));
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
    public void SizeChange()
    {
        string[] temp = dropdown1.captionText.text.Split('*');
        AreaLength = int.Parse(temp[0]);
        AreaWidth = int.Parse(temp[1]);
        lenghtmul = AreaLength / 85;
        widthtmul = AreaWidth / 40;
        print(widthtmul.ToString() + lenghtmul.ToString());
        PanelPrefab.transform.localScale = new Vector3(lenghtmul, 1, widthtmul);
        Destroy(Panels[0]);
        Panels.Clear();
        Panels.Add(Instantiate(PanelPrefab, tartget.transform.position, Quaternion.identity, PanelContain.transform));
        Panels[0].GetComponent<AutoSinglePlane>().PathCount = PathwayCount;

        AutoSinglePlane.lenght = Mathf.Abs((Panels[0].transform.Find("leftnorth").transform.position - Panels[0].transform.Find("rightnorth").transform.position).magnitude);
        AutoSinglePlane.width = Mathf.Abs((Panels[0].transform.Find("leftnorth").transform.position - Panels[0].transform.Find("leftsorth").transform.position).magnitude);
        Mulsize = AreaLength / AutoSinglePlane.lenght;

    }
    public void PathwayChange()
    {
        PathwayCount = int.Parse(dropdown2.captionText.text);
        Panels[0].GetComponent<AutoSinglePlane>().PathCount = PathwayCount;
    }
    public void FloorChange()
    {
        FloorCount = int.Parse(dropdown3.captionText.text);
    }
    public void LineCountChange()
    {
        NowLineCount = int.Parse(dropdown4.captionText.text);
    }

    private void ReStart()
    {

    }
    
}