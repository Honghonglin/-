using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Part5;
using UnityEngine.UI;
public class UI_Message : MonoBehaviour
{
    public  static int MaxPoint { get; set; }
    public Text JudgeText;
    public int MaxLineNet { get { return MaxPoint / 2; } }
    public int SeletLineNet { get; set; }
    public int MaxSingleNetLinePoint { get { return MaxPoint - (SeletLineNet - 1) * 2; } }
    /// <summary>
    /// 线网集
    /// </summary>
    public List<LineNet> LineNets { get; set; } = new List<LineNet>();
    public bool IsSeletingMode { get; set; } = false;
    public InputField inputField;
    /// <summary>
    /// 长*宽
    /// </summary>
    public Dropdown dropdown1;
    /// <summary>
    /// LineDrop
    /// </summary>
    public Dropdown dropdown2;
    /// <summary>
    /// 线网个数Dropdown
    /// </summary>
    public Dropdown dropdown3;
    /// <summary>
    /// LinePointText
    /// </summary>
    public Text LinePointText;
    /// <summary>
    /// LineCountText
    /// </summary>
    public Text LineCountText;
    private LineNet NowLineNet { get; set; }
    public bool IsLineNetCountChange { get; set; } = false;
    public bool IsLineNetChange { get; set; } = false;
    public int NowLineNetCount { get; set; }
    public int NowLineNetNumber { get; set; }
    public List<GameObject> Have_SeletPointList { get; set; } = new List<GameObject>();
    public Button ClearButton;
    public Button CreatButton;
    public Button SureButton;
    public Button Button3;
    public Button ExitButton1;
    public Button ExitButton2;
    public bool IsSure { get; set; } = false;
    public void Awake()
    {
        dropdown2.gameObject.SetActive(false);
        dropdown3.ClearOptions();
        dropdown3.gameObject.SetActive(false);
        LinePointText.gameObject.SetActive(false);
        LineCountText.gameObject.SetActive(false);
        ClearButton.gameObject.SetActive(false);
        CreatButton.gameObject.SetActive(false);
        SureButton.gameObject.SetActive(false);
        JudgeText.gameObject.SetActive(false);
        ExitButton1.gameObject.SetActive(false);
        ExitButton2.gameObject.SetActive(false);
    }

    public void Judge()
    {
        JudgeText.text = "脚点个数为:" + MaxPoint.ToString()+"\n"+"最大线网个数为:"+MaxLineNet+"\n"+"单个线网最大引脚数为:"+MaxSingleNetLinePoint;    
    }
    private void Update()
    {
        #region 检测是否显示SureButton
        if(JudgeSureButton()&&IsSeletingMode)
        {
            SureButton.gameObject.SetActive(true);
        }
        else
        {
            SureButton.gameObject.SetActive(false);
        }
        #endregion
        LineNetJudge();
        ClearButton.gameObject.SetActive(IsHavePoint());
        if(IsSure)
        {
            CreatButton.gameObject.SetActive(true);
        }
        else
        {
            CreatButton.gameObject.SetActive(false);
        }
        JudgeNowLine();
    }
    /// <summary>
    /// 确认按钮事件1，确认线网范围，管理UI
    /// </summary>
    public void AffirmButton()
    {
        inputField.enabled = false;
        dropdown1.enabled = false;
        dropdown3.gameObject.SetActive(true);
        LineCountText.gameObject.SetActive(true);
        List<string> vs = new List<string>();
        for (int i = 1 ; i <=MaxLineNet; i++)
        {
            vs.Add(i.ToString());
        }
        dropdown3.AddOptions(vs);
        NowLineNetCount = 1;
        NowLineNetNumber = 1;
        IsSeletingMode = true;
        IsLineNetCountChange = true;
        Button3.gameObject.SetActive(false);
        JudgeText.gameObject.SetActive(true);
        ExitButton1.gameObject.SetActive(true);
    }
    /// <summary>
    /// 选择不同线网过程
    /// </summary>
    public void LineNetJudge()
    {
        if(IsSeletingMode&& IsLineNetCountChange)
        {
            LineNets.Clear();
            dropdown2.ClearOptions();
            dropdown2.gameObject.SetActive(true);
            LinePointText.gameObject.SetActive(true);
            SeletLineNet = int.Parse(dropdown3.captionText.text);
            Judge();
            List<string> vs = new List<string>();
            for (int i = 1; i <=SeletLineNet; i++)
            {
                vs.Add("线网" + i.ToString());
                LineNet lineNet = new LineNet() { LineNumber = i };
                LineNets.Add(lineNet);
            }
            dropdown2.AddOptions(vs);
            Have_SeletPointList.Clear();
            IsLineNetCountChange = false;
            IsLineNetChange = true;
        }
        if(IsSeletingMode)
        {
            string str = dropdown2.captionText.text.Replace("线网", "");
            if(NowLineNetNumber!=int.Parse(str))
            {
                IsLineNetChange = true;
            }
        }
        if (IsSeletingMode && IsLineNetCountChange || IsSeletingMode && IsLineNetChange)
        {
            string str = dropdown2.captionText.text.Replace("线网", "");
            NowLineNetNumber = int.Parse(str);
            NowLineNet = LineNets[NowLineNetNumber - 1];
        }
        if (IsSeletingMode&&IsLineNetChange)
        {
            LinePointText.text = "";
            if (NowLineNet.Point.Count != 0)
            {
                foreach (var item in NowLineNet.Point)
                {
                    LinePointText.text += item.GetComponent<ImportantPoint_number>().Number.ToString() + " ";
                }
            }
            IsLineNetChange = false;
        }
        if(IsSeletingMode&&NowLineNetCount!=int.Parse(dropdown3.captionText.text))
        {
            IsLineNetCountChange = true;
            NowLineNetCount = int.Parse(dropdown3.captionText.text);
        }
        if(IsSeletingMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit=new RaycastHit();
            if(Physics.Raycast(ray,out hit,50,LayerMask.GetMask("ImportantPoint"))&&Input.GetMouseButtonDown(0)&&!Have_SeletPointList.Contains(hit.collider.gameObject))
            {
                NowLineNet.Point.Add(hit.collider.gameObject);
                Have_SeletPointList.Add(hit.collider.gameObject);
                IsLineNetChange = true;
            }
            if(Physics.Raycast(ray, out hit, 50, LayerMask.GetMask("ImportantPoint")) && Input.GetMouseButtonDown(1)&&NowLineNet.Point.Contains(hit.collider.gameObject))
            {
                NowLineNet.Point.Remove(hit.collider.gameObject);
                Have_SeletPointList.Remove(hit.collider.gameObject);
                IsLineNetChange = true;
            }
        }
    }
    /// <summary>
    /// 检查该线网是否有引脚
    /// </summary>
    /// <returns></returns>
    public bool IsHavePoint()
    {
        if(NowLineNet!=null&&IsSeletingMode)
        {
            if(NowLineNet.Point.Count!=0)
            {
                return true;
                
            }
            else if(NowLineNet.Point.Count==0)
            {
                return false;
            }
        }
        return false;
    }
    /// <summary>
    /// 清除线网
    /// </summary>
    public void CancelLineNet()
    {
        foreach (var item in NowLineNet.Point)
        {
            Have_SeletPointList.Remove(item);
        }
        NowLineNet.Point.Clear();
        IsLineNetChange = true;
    }
    /// <summary>
    /// 线网确定
    /// </summary>
    public void SureButtonFunc()
    {
        IsSeletingMode = false;
        IsLineNetCountChange = false;
        IsLineNetCountChange = false;
        dropdown3.gameObject.SetActive(false);
        LineCountText.gameObject.SetActive(false);
        ClearButton.gameObject.SetActive(false);
        ExitButton1.gameObject.SetActive(false);
        JudgeText.gameObject.SetActive(false);
        ExitButton2.gameObject.SetActive(true);
        IsSure = true;
    }
    /// <summary>
    /// 判断Sure阶段下的NowLine
    /// </summary>
    public void JudgeNowLine()
    {
        if(IsSure)
        {
            string str = dropdown2.captionText.text.Replace("线网", "");
            if(NowLineNetNumber!=int.Parse(str))
            {
                NowLineNetNumber = int.Parse(str);
                NowLineNet = LineNets[NowLineNetNumber - 1];
                LinePointText.text = "";
                if (NowLineNet.Point.Count != 0)
                {
                    foreach (var item in NowLineNet.Point)
                    {
                        LinePointText.text += item.GetComponent<ImportantPoint_number>().Number.ToString() + " ";
                    }
                }
            }
        }

    }
    /// <summary>
    /// ExitButton2
    /// </summary>
    public void ExitButton2_Func()
    {
        IsSeletingMode = true;
        dropdown3.gameObject.SetActive(true);
        LineCountText.gameObject.SetActive(true);
        ClearButton.gameObject.SetActive(true);
        ExitButton1.gameObject.SetActive(true);
        JudgeText.gameObject.SetActive(true);
        ExitButton2.gameObject.SetActive(false);
        IsSure = false;
    }
    
    public bool JudgeSureButton()
    {
        if (LineNets.Count != 0)
        {
            foreach (var item in LineNets)
            {
                if (item.Point.Count < 2)
                { return false; }
            }
            return true;
        }
        else
            return false;

    }
   /// <summary>
   /// ExitButton1
   /// </summary>
    public void ExitButton1Func2()
    {
        inputField.enabled = true;
        dropdown1.enabled = true;
        dropdown2.ClearOptions();
        LinePointText.text = "";
        dropdown3.gameObject.SetActive(false);
        dropdown2.gameObject.SetActive(false);
        LineCountText.gameObject.SetActive(false);
        LinePointText.gameObject.SetActive(false);
        dropdown3.ClearOptions();
        NowLineNetCount = 0;
        NowLineNetNumber = 0;
        IsSeletingMode = false;
        IsLineNetCountChange = false;
        Button3.gameObject.SetActive(true);
        JudgeText.text = "";
        JudgeText.gameObject.SetActive(false);
        ExitButton1.gameObject.SetActive(false);
        IsLineNetChange = false;
        Have_SeletPointList.Clear();
        LineNets.Clear();
        NowLineNet = null;
        IsSure = false;
    }

    //生成线网
    public void CreatLineNetButton()
    {
        
    }
}
