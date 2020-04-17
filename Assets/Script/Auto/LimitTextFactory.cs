using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
namespace Assets.Script.Auto
{
    class LimitTextFactory : MonoBehaviour
    {
        [DllImport("link3")]
        private static extern void LimitFun();
        [HideInInspector]
        public GameObject prefabText = null;
        public GameObject limitTextContain = null;
        public GameObject LineRenderContain = null;
        public GameObject CreateLimitUI=null;
        public List<ButtonProp> buttonProps=null;
        private List<PanelLineLimit> panelLineLimits = new List<PanelLineLimit>();
        private List<PanelEndtailLimit> panelEndtailLimits = new List<PanelEndtailLimit>();
        private List<PanelThroughtLimit> panelThroughtLimits = new List<PanelThroughtLimit>();
        private List<PanelEdgeLimit> panelEdgeLimits = new List<PanelEdgeLimit>();
        private List<PanelMinAreaLimit> panelMinAreaLimits = new List<PanelMinAreaLimit>();
        private PanelLineLimitInstants panelLineLimitInstants = new PanelLineLimitInstants();
        private PanelEndtailLimitInstants panelEndtailLimitInstants = new PanelEndtailLimitInstants();
        private PanelThroughtLimitInstants panelThroughtLimitInstants = new PanelThroughtLimitInstants();
        private PanelEdgeLimitInstants panelEdgeLimitInstants = new PanelEdgeLimitInstants();
        private PanelMinAreaLimitInstants panelMinAreaLimitInstants = new PanelMinAreaLimitInstants();
        private List<GameObject> LineToLineTexts = new List<GameObject>();
        private List<GameObject> MentalToMentalTexts = new List<GameObject>();
        private List<GameObject> MentalToLineTexts = new List<GameObject>();
        //  private List<LineRenderer> lineRenderers = new List<LineRenderer>();
        private void Start()
        {
            prefabText = Resources.Load<GameObject>("LineLimitText");
            CreatButtons(CreateLimitUI, Resources.Load<GameObject>("Button"), buttonProps.ToArray());
        }

        private void CreatButtons(GameObject Container, GameObject Prefab, params ButtonProp[] buttonProps)
        {
            List<Button> buttons = new List<Button>();
            Date.ButtonEvent[] buttonEvents = new Date.ButtonEvent[] { FindAndCreat };
            foreach (var buttonProp in buttonProps)
            {
                GameObject button = Instantiate(Prefab, Container.transform);
                button.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(buttonProp.edge,buttonProp.distant , buttonProp.size);//位置
                button.transform.GetChild(0).GetComponent<Text>().text = buttonProp.ButtonText;
                //button.transform.GetComponent<Button>().onClick.AddListener(delegate () { FindAndCreat(); });
                //button.transform.GetComponent<Button>().onClick.AddListener(()=>FindAndCreat());
                button.transform.GetComponent<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(FindAndCreat));
                buttons.Add(button.GetComponent<Button>());    
            }
        }


        private void FindAndCreat()
        {
            LimitFun();
            GetLimit();
            CreateText(panelLineLimits, panelLineLimitInstants, limitTextContain.transform.Find("panelLineLimitsContainer"));
            CreateText(panelEndtailLimits, panelEndtailLimitInstants, limitTextContain.transform.Find("panelEndtailLimitsContainer"));
            CreateText(panelThroughtLimits, panelThroughtLimitInstants, limitTextContain.transform.Find("panelThroughtLimitsContainer"));
            CreateText(panelEdgeLimits, panelEdgeLimitInstants, limitTextContain.transform.Find("panelEdgeLimitsContainer"));
            CreateText(limitTextContain.transform.Find("AreaContainer"));
        }
        private void GetLimit()
        {
            File_Reader file_Reader = new File_Reader();
            file_Reader.PrlvRead(panelLineLimits);
            file_Reader.EofvRead(panelEndtailLimits);
            file_Reader.CutsvRead(panelThroughtLimits);
            file_Reader.CtcvRead(panelEdgeLimits);
            file_Reader.AreaRead(panelMinAreaLimits);
        }
        private void ClearLimit()
        {
            panelLineLimits.Clear();
            panelEndtailLimits.Clear();
            panelThroughtLimits.Clear();
            panelEdgeLimits.Clear();
            panelMinAreaLimits.Clear();
        }
        //前四个约束
        private void CreateText<T, V>(List<T> panelLimits, V panelLimitInstants,Transform Container)
            where T : BasePanelLimit
            where V : IPanelBaseLimitInstants
        {
            foreach (var panelLimit in panelLimits)
            {
                //LineToLine
                foreach (var lineToLine in panelLimit.LineToLines)
                {
                    LimitText limitText = new LimitText()
                    {
                        Type = lineToLine.LimitType,
                        LimitContent = panelLimit.Name
                    };
                    GameObject Text = Instantiate(prefabText, Container);
                    float firstX = ((float)lineToLine.FirstLine[0].GetX() + (float)lineToLine.FirstLine[1].GetX()) / 2f;
                    float firstY = ((float)lineToLine.FirstLine[0].GetY() + (float)lineToLine.FirstLine[1].GetY()) / 2f;
                    float firstF = (lineToLine.FirstLine[0].Floor + lineToLine.FirstLine[1].Floor) / 2f;
                    Vector3 first = Date.Panels[0].transform.Find("leftsorth").transform.position + firstF * new Vector3(0, 1, 0)
                        + new Vector3(firstX, 0, firstY);
                    float secondX = ((float)lineToLine.SecondLine[0].GetX() + (float)lineToLine.SecondLine[1].GetX()) / 2f;
                    float secondY = ((float)lineToLine.SecondLine[0].GetY() + (float)lineToLine.SecondLine[1].GetY()) / 2f;
                    float secondF = (lineToLine.SecondLine[0].Floor + lineToLine.SecondLine[1].Floor) / 2f;
                    Vector3 second = Date.Panels[0].transform.Find("leftsorth").transform.position + secondF * new Vector3(0, 1, 0)
                        + new Vector3(secondX, 0, secondY);
                    InstaintText(Text, first, second);
                    Text.GetComponent<Text>().text = limitText.TypeContent + "  " + limitText.LimitContent;
                    panelLimitInstants.PanelLimitInstants.Add(Text);
                    LineToLineTexts.Add(Text);
                }
                //MentalToLine
                foreach (var mentalToLine in panelLimit.MentalToLines)
                {
                    LimitText limitText = new LimitText()
                    {
                        Type = mentalToLine.LimitType,
                        LimitContent = panelLimit.Name
                    };
                    GameObject Text = Instantiate(prefabText, Container);
                    float firstX = ((float)mentalToLine.Line[0].GetX() + (float)mentalToLine.Line[1].GetX()) / 2f;
                    float firstY = ((float)mentalToLine.Line[0].GetY() + (float)mentalToLine.Line[1].GetY()) / 2f;
                    float firstF = (mentalToLine.Line[0].Floor + mentalToLine.Line[1].Floor) / 2f;
                    Vector3 first = Date.Panels[0].transform.Find("leftsorth").transform.position + firstF * new Vector3(0, 1, 0)
                        + new Vector3(firstX, 0, firstY);
                    Vector3 second = Date.Panels[0].transform.Find("leftsorth").transform.position + mentalToLine.Mental.Floor * new Vector3(0, 1, 0)
                        + new Vector3((float)mentalToLine.Mental.GetX(), 0, (float)mentalToLine.Mental.GetY());
                    InstaintText(Text, first, second);
                    Text.GetComponent<Text>().text = limitText.TypeContent + "  " + limitText.LimitContent;
                    panelLimitInstants.PanelLimitInstants.Add(Text);
                    MentalToLineTexts.Add(Text);
                }
                //MentalToMental
                foreach (var mentalToMental in panelLimit.MentalToMentals)
                {
                    LimitText limitText = new LimitText()
                    {
                        Type = mentalToMental.LimitType,
                        LimitContent = panelLimit.Name
                    };
                    GameObject Text = Instantiate(prefabText, Container);

                    Vector3 first = Date.Panels[0].transform.Find("leftsorth").transform.position + mentalToMental.FirstMental.Floor * new Vector3(0, 1, 0)
                        + new Vector3((float)mentalToMental.FirstMental.GetX(), 0, (float)mentalToMental.FirstMental.GetY());
                    Vector3 second = Date.Panels[0].transform.Find("leftsorth").transform.position + mentalToMental.SecondMenatl.Floor * new Vector3(0, 1, 0)
                        + new Vector3((float)mentalToMental.SecondMenatl.GetX(), 0, (float)mentalToMental.SecondMenatl.GetY());
                    InstaintText(Text, first, second);
                    Text.GetComponent<Text>().text = limitText.TypeContent + "  " + limitText.LimitContent;
                    panelLimitInstants.PanelLimitInstants.Add(Text);
                    MentalToMentalTexts.Add(Text);
                }
            }
        }
        private void CreateText(Transform Container)
        {
            //最小面积约束
            foreach (var panelMinAreaLimit in panelMinAreaLimits)
            {
                foreach (var lineArea in panelMinAreaLimit.LineAreas)
                {
                    LimitText limitText = new LimitText()
                    {
                        Type = lineArea.LimitType,
                        LimitContent = panelMinAreaLimit.Name
                    };
                    GameObject Text = Instantiate(prefabText, Container);
                    float firstX = ((float)lineArea.Line[0].GetX() + (float)lineArea.Line[1].GetX()) / 2f;
                    float firstY = ((float)lineArea.Line[0].GetY() + (float)lineArea.Line[1].GetY()) / 2f;
                    float firstF = (lineArea.Line[0].Floor + lineArea.Line[1].Floor) / 2f;
                    Vector3 first = Date.Panels[0].transform.Find("leftsorth").transform.position + firstF * new Vector3(0, 1, 0)
                        + new Vector3(firstX, 0, firstY);
                    Text.transform.position = first;
                    Text.transform.SetParent(limitTextContain.transform);
                    Text.GetComponent<Text>().text = limitText.TypeContent + "  " + limitText.LimitContent;
                    panelMinAreaLimitInstants.PanelLimitInstants.Add(Text);
                }
                foreach (var mentalArea in panelMinAreaLimit.MentalAreas)
                {
                    LimitText limitText = new LimitText()
                    {
                        Type = mentalArea.LimitType,
                        LimitContent = panelMinAreaLimit.Name
                    };
                    GameObject Text = Instantiate(prefabText, Container);
                    Vector3 first = Date.Panels[0].transform.Find("leftsorth").transform.position + mentalArea.Point.Floor * new Vector3(0, 1, 0)
                        + new Vector3((float)mentalArea.Point.GetX(), 0, (float)mentalArea.Point.GetY());
                    Text.transform.position = first;
                    Text.transform.SetParent(limitTextContain.transform);
                    Text.GetComponent<Text>().text = limitText.TypeContent + "  " + limitText.LimitContent;
                    panelMinAreaLimitInstants.PanelLimitInstants.Add(Text);
                }
            }
        }

        private void ClearText()
        {
            foreach (var PanelLimitInstant in panelLineLimitInstants.PanelLimitInstants)
            {
                if (PanelLimitInstant == null) { }
                else { Destroy(PanelLimitInstant); }
            }
            panelLineLimitInstants.PanelLimitInstants.Clear();
            foreach (var PanelLimitInstant in panelEndtailLimitInstants.PanelLimitInstants)
            {
                if (PanelLimitInstant == null) { }
                else { Destroy(PanelLimitInstant); }
            }
            panelEndtailLimitInstants.PanelLimitInstants.Clear();
            foreach (var PanelLimitInstant in panelThroughtLimitInstants.PanelLimitInstants)
            {
                if (PanelLimitInstant == null) { }
                else { Destroy(PanelLimitInstant); }
            }
            panelThroughtLimitInstants.PanelLimitInstants.Clear();
            foreach (var PanelLimitInstant in panelEdgeLimitInstants.PanelLimitInstants)
            {
                if (PanelLimitInstant == null) { }
                else { Destroy(PanelLimitInstant); }
            }
            panelEdgeLimitInstants.PanelLimitInstants.Clear();
            foreach (var PanelLimitInstant in panelMinAreaLimitInstants.PanelLimitInstants)
            {
                if (PanelLimitInstant == null) { }
                else { Destroy(PanelLimitInstant); }
            }
            panelMinAreaLimitInstants.PanelLimitInstants.Clear();
            LineToLineTexts.Clear();
            MentalToMentalTexts.Clear();
            MentalToLineTexts.Clear();
        }

        private void InstaintText(GameObject @object, Vector3 org, Vector3 end)
        {
            @object.transform.position = (org + end) / 2;
        }

        //创建指向线
        private void CreatLineRender(Vector3 org, Vector3 end)
        {


        }

    }
    [Serializable]
    class ButtonProp
    {
        public string ButtonText=null;
        //位置
        public RectTransform.Edge edge;
        public float distant;
        public float size;
    }

    interface IPanelBaseLimitInstants
    {
        List<GameObject> PanelLimitInstants { get; set; }
    }
    class PanelLineLimitInstants : IPanelBaseLimitInstants
    {
        public List<GameObject> PanelLimitInstants { get; set; } = new List<GameObject>();
    }
    class PanelEndtailLimitInstants : IPanelBaseLimitInstants
    {
        public List<GameObject> PanelLimitInstants { get; set; } = new List<GameObject>();

    }
    class PanelThroughtLimitInstants : IPanelBaseLimitInstants
    {

        public List<GameObject> PanelLimitInstants { get; set; } = new List<GameObject>();


    }
    class PanelEdgeLimitInstants : IPanelBaseLimitInstants
    {
        public List<GameObject> PanelLimitInstants { get; set; } = new List<GameObject>();

    }
    class PanelMinAreaLimitInstants : IPanelBaseLimitInstants
    {
        public List<GameObject> PanelLimitInstants { get; set; } = new List<GameObject>();

    }
    class LimitText
    {
        public LimitType Type { get; set; }
        public string TypeContent
        {
            get
            {
                switch (Type)
                {
                    case LimitType.LineToLine:
                        return "导线和导线";
                    case LimitType.MentalToMental:
                        return "金属片和金属片";
                    case LimitType.MentalToLine:
                        return "金属片和导线";
                    case LimitType.Mental:
                        return "金属片";
                    case LimitType.Line:
                        return "导线";
                    default:
                        return "无";
                }
            }
            set { }
        }
        public string LimitContent { get; set; }
    }
}
