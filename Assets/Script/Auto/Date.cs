using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Auto;
using System.Runtime.InteropServices;
using System.IO;
public class Date : MonoBehaviour
{
    [DllImport("link 12")]
    private static extern void Func();
    public static List<Net> lineNets = new List<Net>();
    public static List<Point> allBasePoint = new List<Point>();
    public static List<GameObject> Panels = new List<GameObject>();//层
    public static List<Point> allNoBasePoint = new List<Point>();//所有通孔
    private List<GameObject> allBasePointInst = new List<GameObject>();
    private List<Pair> Pairs = new List<Pair>();


    public Vector3 tartget;
    public Dropdown dropdown1;
    public Dropdown dropdown2;
    public Dropdown dropdown3;
    public Dropdown dropdown4;
    public Button[] buttons;
    private string[] names = new string[] { "确认", "显示随机生成的引脚", "返回1", "返回2" };
    private ButtonEvent[] events;
    private GameObject PointPrefab;
    private GameObject PointContain;
    private GameObject PanelPrefab;
    private GameObject PanelContain;

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

    private float Mulsize { get; set; }

    private delegate void ButtonEvent();

    private void Awake()
    {
        PanelContain = GameObject.Find("PanelContain");
        PanelPrefab = Resources.Load<GameObject>("basepanel");
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

        Panels.Add(Instantiate(PanelPrefab, tartget, Quaternion.identity, PanelContain.transform));
        Panels[0].GetComponent<AutoSinglePlane>().PathCount = PathwayCount;

        AutoSinglePlane.lenght = Mathf.Abs((Panels[0].transform.Find("leftnorth").transform.position - Panels[0].transform.Find("rightnorth").transform.position).magnitude);
        AutoSinglePlane.width= Mathf.Abs((Panels[0].transform.Find("leftnorth").transform.position - Panels[0].transform.Find("leftsorth").transform.position).magnitude);
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
                net.PointsInstant.Add(Instantiate(PointPrefab, point.GetVector3() / Mulsize + Panels[0].transform.Find("leftsorth").transform.position, Quaternion.AngleAxis(-90, Vector3.right), PointContain.transform));
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
        allNoBasePoint.Clear();
        allBasePoint.Clear();
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
    /// 生成线网
    /// </summary>
    private void CreatNets()
    {
        if(Panels.Count>=1)
        {
            //生成层
            for (int i = 0; i < FloorCount - 1; i++)
            {
                GameObject single = Instantiate(PanelPrefab, Panels[0].transform.position, Quaternion.identity, PanelContain.transform);
                single.GetComponent<AutoSinglePlane>().Floor = i + 1;
                if(i%2==0)
                {
                    single.GetComponent<AutoSinglePlane>().Type = AutoSinglePlane.PathType.Vertical;
                }
                else
                {
                    single.GetComponent<AutoSinglePlane>().Type = AutoSinglePlane.PathType.Horizontal;
                }
                Panels.Add(single);
            }
            
            //设置每层的pathway
            int tempPathway = PathwayCount;
            for (int i = 0; i < Panels.Count; i++)
            {
                if(i%3==0&&i!=0)
                {
                    tempPathway--;
                }
                Panels[i].GetComponent<AutoSinglePlane>().PathCount = tempPathway;
            }

            //生成所有通孔
            for (int i = 1; i < FloorCount; i++)
            {
                AutoSinglePlane nowautoSinglePlane = Panels[i].GetComponent<AutoSinglePlane>();
                AutoSinglePlane formerauroPlane = Panels[i - 1].GetComponent<AutoSinglePlane>();
                #region 水平
                if (nowautoSinglePlane.Type==AutoSinglePlane.PathType.Horizontal)
                {
                    if(nowautoSinglePlane.PathCount==formerauroPlane.PathCount)
                    {
                        int t = formerauroPlane.points[0].Number + formerauroPlane.points.Count;
                        for (int k = 0; k < formerauroPlane.PathCount; k++)
                        {
                            for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                            {
                                Point point = new Point(
                                    formerauroPlane.Spacing * 0.5 + k * formerauroPlane.Spacing,
                                    nowautoSinglePlane.Spacing * 0.5 + j * nowautoSinglePlane.Spacing,
                                    i,t++ );
                                nowautoSinglePlane.points.Add(point);
                            }
                        }
                    }
                    if(nowautoSinglePlane.PathCount!=formerauroPlane.PathCount)
                    {
                        int t = formerauroPlane.points[0].Number + formerauroPlane.points.Count;
                        for (int k = 0; k < formerauroPlane.PathCount; k++)
                        {
                            for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                            {
                                Point point = new Point(
                                    formerauroPlane.Spacing * 0.5 + k * formerauroPlane.Spacing,
                                    nowautoSinglePlane.Spacing * 0.5 + j * nowautoSinglePlane.Spacing,
                                    i-1, t++);
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
                                    i , t++);
                                nowautoSinglePlane.points.Add(point);
                            }
                        }

                        if(i!=FloorCount-1)
                        {
                            t = nowautoSinglePlane.points.Count + nowautoSinglePlane.points[0].Number;
                            for (int k = 0; k < nowautoSinglePlane.PathCount; k++)
                            {
                                for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                                {
                                    Point point = new Point(
                                        nowautoSinglePlane.Spacing * 0.5 + k * nowautoSinglePlane.Spacing,
                                        nowautoSinglePlane.Spacing * 0.5 + j * nowautoSinglePlane.Spacing,
                                        i, t++);
                                    nowautoSinglePlane.points.Add(point);
                                }
                            }
                        }
                        
                    }
                }
                #endregion


                #region 垂直
                if (formerauroPlane.Type==AutoSinglePlane.PathType.Vertical)
                {
                    if (nowautoSinglePlane.PathCount == formerauroPlane.PathCount)
                    {
                        int t=formerauroPlane.points[0].Number + formerauroPlane.points.Count;
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
                                    i-1, t++);
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
                            t = nowautoSinglePlane.points.Count + nowautoSinglePlane.points[0].Number;
                            for (int k = 0; k < nowautoSinglePlane.PathCount; k++)
                            {
                                for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                                {
                                    Point point = new Point(
                                        nowautoSinglePlane.Spacing * 0.5 + k * nowautoSinglePlane.Spacing,
                                        nowautoSinglePlane.Spacing * 0.5 + j * nowautoSinglePlane.Spacing,
                                        i, t++);
                                    nowautoSinglePlane.points.Add(point);
                                }
                            }
                        }



                    }
                }
                #endregion
            }
            //设置Pairs对
            for (int i = 1; i < Panels.Count-1; i++)
            {
                AutoSinglePlane nowautoSinglePlane = Panels[i].GetComponent<AutoSinglePlane>();
                AutoSinglePlane fowardautoSinglePlane = Panels[i - 1].GetComponent<AutoSinglePlane>();
                AutoSinglePlane afterautoSinglePlane = Panels[i + 1].GetComponent<AutoSinglePlane>();
                 if(nowautoSinglePlane.PathCount==fowardautoSinglePlane.PathCount&&
                    afterautoSinglePlane.PathCount==nowautoSinglePlane.PathCount)
                {
                    foreach (var point in nowautoSinglePlane.points)
                    {
                        Pair pair = new Pair(point.Number - nowautoSinglePlane.points.Count,
                            point.Number);
                        Pairs.Add(pair);
                    }
                    int start = nowautoSinglePlane.points[0].Number;
                    if(nowautoSinglePlane.Type==AutoSinglePlane.PathType.Horizontal)
                    {
                        for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                        {
                            for (int k = 0; k < nowautoSinglePlane.PathCount-1; k++)
                            {
                                Pair pair = new Pair(start + k * nowautoSinglePlane.PathCount+j,
                                    start + (k + 1) * nowautoSinglePlane.PathCount+j);
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
                                Pair pair = new Pair(start + k+j*nowautoSinglePlane.PathCount,
                                    start + k+1+j*nowautoSinglePlane.PathCount);
                                Pairs.Add(pair);
                            }
                        }


                    }
                    
                }
                else if(nowautoSinglePlane.PathCount==fowardautoSinglePlane.PathCount&&
                    afterautoSinglePlane.PathCount!=nowautoSinglePlane.PathCount)
                {
                    for (int k = 0; k < nowautoSinglePlane.PathCount*fowardautoSinglePlane.PathCount; k++)
                    {
                        int second = nowautoSinglePlane.points[k].Number;
                       int first=nowautoSinglePlane.points[k].Number - nowautoSinglePlane.PathCount * fowardautoSinglePlane.PathCount;
                        Pair pair = new Pair(first, second);
                        Pairs.Add(pair);
                    }
                    if (nowautoSinglePlane.Type == AutoSinglePlane.PathType.Horizontal)
                    {
                        for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                        {
                            int start = nowautoSinglePlane.points[0].Number + j * nowautoSinglePlane.PathCount;
                            int t = 0;
                            int s = 0;
                            for (int k = 0; k < nowautoSinglePlane.PathCount; k++)
                            {

                                if (k % 2 == 0)
                                {
                                    Pair pair = new Pair(start + s*nowautoSinglePlane.PathCount,
                                        start + fowardautoSinglePlane.points.Count + t);
                                    t++;
                                    Pairs.Add(pair);
                                }
                                else
                                {
                                    Pair pair = new Pair(start + fowardautoSinglePlane.points.Count + t,
                                        start + (++s)*nowautoSinglePlane.PathCount);
                                    Pairs.Add(pair);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                        {
                            int start = nowautoSinglePlane.points[0].Number + j * nowautoSinglePlane.PathCount;
                            int t = 0;
                            int s = 0;
                            for (int k = 0; k < nowautoSinglePlane.PathCount; k++)
                            {

                                if (k % 2 == 0)
                                {
                                    Pair pair = new Pair(start + s, start + fowardautoSinglePlane.points.Count + t);
                                    t++;
                                    Pairs.Add(pair);
                                }
                                else
                                {
                                    Pair pair = new Pair(start + fowardautoSinglePlane.points.Count + t, start + (++s));
                                    Pairs.Add(pair);
                                }
                            }
                        }
                    }

                }
                else if(nowautoSinglePlane.PathCount!=fowardautoSinglePlane.PathCount)
                {
                    for (int k = 0; k < nowautoSinglePlane.PathCount * fowardautoSinglePlane.PathCount; k++)
                    {
                        int second = nowautoSinglePlane.points[k].Number;
                        int first = nowautoSinglePlane.points[k].Number - nowautoSinglePlane.PathCount * fowardautoSinglePlane.PathCount;
                        Pair pair = new Pair(first, second);
                        Pairs.Add(pair);
                    }


                    if (nowautoSinglePlane.Type == AutoSinglePlane.PathType.Horizontal)
                    {
                        for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                        {
                            int start = nowautoSinglePlane.points[0].Number + j;
                            int t = 0;
                            int s = 0;
                            for (int k = 0; k < nowautoSinglePlane.PathCount; k++)
                            {

                                if (k % 2 == 0)
                                {
                                    Pair pair = new Pair(start + s * nowautoSinglePlane.PathCount,
                                        start + nowautoSinglePlane.PathCount*fowardautoSinglePlane.PathCount + 
                                        t*nowautoSinglePlane.PathCount);
                                    t++;
                                    Pairs.Add(pair);
                                }
                                else
                                {
                                    Pair pair = new Pair(start + nowautoSinglePlane.PathCount * fowardautoSinglePlane.PathCount +
                                        t * nowautoSinglePlane.PathCount,
                                        start + (++s) * nowautoSinglePlane.PathCount);
                                    Pairs.Add(pair);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < nowautoSinglePlane.PathCount; j++)
                        {
                            int start = nowautoSinglePlane.points[0].Number + j * fowardautoSinglePlane.PathCount;
                            int t = 0;
                            int s = 0;
                            for (int k = 0; k < nowautoSinglePlane.PathCount; k++)
                            {

                                if (k % 2 == 0)
                                {
                                    Pair pair = new Pair(start + s,
                                        start + fowardautoSinglePlane.points.Count*nowautoSinglePlane.PathCount + t);
                                    t++;
                                    Pairs.Add(pair);
                                }
                                else
                                {
                                    Pair pair = new Pair(start + fowardautoSinglePlane.points.Count * nowautoSinglePlane.PathCount + t,
                                        start + (++s));
                                    Pairs.Add(pair);
                                }
                            }
                        }
                    }





                }
            }


            {
                AutoSinglePlane nowautoSinglePlane = Panels[Panels.Count - 1].GetComponent<AutoSinglePlane>();
                AutoSinglePlane fowardautoSinglePlane = Panels[Panels.Count - 1].GetComponent<AutoSinglePlane>();
                for (int k = 0; k < nowautoSinglePlane.PathCount * fowardautoSinglePlane.PathCount; k++)
                {
                    int second = nowautoSinglePlane.points[k].Number;
                    int first = nowautoSinglePlane.points[k].Number - nowautoSinglePlane.PathCount * fowardautoSinglePlane.PathCount;
                    Pair pair = new Pair(first, second);
                    Pairs.Add(pair);
                }
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
            allBasePointInst.Add(Instantiate(PointPrefab, point.GetVector3() / Mulsize + Panels[0].transform.Find("leftsorth").transform.position, Quaternion.AngleAxis(-90, Vector3.right), PointContain.transform));
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
