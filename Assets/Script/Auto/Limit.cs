using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Auto
{
   public enum LimitType
    {
        LineToLine,
        MentalToMental,
        MentalToLine,
        Mental,
        Line
    }
    public class LineToLine
    {
        public LimitType LimitType { get; private set; } = LimitType.LineToLine;
        public List<Point> FirstLine { get; set; } = new List<Point>();
        public List<Point> SecondLine { get; set; } = new List<Point>();
        public float With1 { get; set; }
        public float With2 { get; set; }
        public float MaxWith { get { return With1 > With2 ? With1 : With2; } set { } }
        //长度
        public float Prl { get; set; }
        //间距
        public float Spacing { get; set; }
    }
    public class MentalToMental
    {
        public LimitType LimitType { get; private set; } = LimitType.MentalToMental;
        public Point FirstMental { get; set; }
        public Point SecondMenatl { get; set; }
        public float MaxWith { get; set; }
        public float Prl { get; set; }
        public float Spacing { get; set; }
        public float With1 { get; set; }
        public float With2 { get; set; }
        public void SetMaxWith()
        {
            MaxWith = With1 > With2 ? With1 : With2;
        }
    }

    public class MentalToLine
    {
        public LimitType LimitType { get; private set; } = LimitType.MentalToLine;
        public Point Mental { get; set; }
        public List<Point> Line { get; set; } = new List<Point>();
        public float With1 { get; set; }
        public float With2 { get; set; }
        public float MaxWith { get { return With1 > With2 ? With1 : With2; } set { } }
    }
    //基础约束
    public class BasePanelLimit
    {
        public virtual string Name { get; set; }
        public List<LineToLine> LineToLines { get; set; } = new List<LineToLine>();
        public List<MentalToLine> MentalToLines { get; set; } = new List<MentalToLine>();
        public List<MentalToMental> MentalToMentals { get; set; } = new List<MentalToMental>();
    }
    //平行走线约束(每层）
    public class PanelLineLimit :BasePanelLimit
    {
        public PanelLineLimit()
        {
            Name = "平行走线约束";
        }
        public override string Name { get => base.Name; set => base.Name = value; }
        //according
        public int Floor { get; set; }
        public List<List<float>> Form { get; set; } = new List<List<float>>();

        public Dictionary<float, int> Prls { get; set; } = new Dictionary<float, int>();
        public Dictionary<float, int> Widths { get; set; } = new Dictionary<float, int>();
    }
    //末端约束
    public class PanelEndtailLimit:BasePanelLimit
    {
        public PanelEndtailLimit()
        {
            Name = "末端约束";
        }
        public override string Name { get => base.Name; set => base.Name = value; }
        public int Floor { get; set; }
        //according
        public float Eolspace { get; set; }
        public float Space { get; set; }
    }
    //通孔柱约束
    public class PanelThroughtLimit:BasePanelLimit
    {
        public PanelThroughtLimit()
        {
            Name = "通孔柱约束";
        }
        public override string Name { get => base.Name; set => base.Name = value; }
        public int Floor { get; set; }

        //according
        public float Within { get; set; }
        public float Adjspace { get; set; }
        public int  Num { get; set; }
    }
    //角对角约束
    public class PanelEdgeLimit:BasePanelLimit
    {
        public PanelEdgeLimit()
        {
            Name = "角对角约束";
        }
        public override string Name { get => base.Name; set => base.Name = value; }
        public int Floor { get; set; }

        //according
        public Dictionary<double, int> WithList { get; set; } = new Dictionary<double, int>();
        public Dictionary<double, int> Spacing { get; set; } = new Dictionary<double, int>();
    }
    //最小面积约束
    public class PanelMinAreaLimit
    {
        public string Name { get; set; } = "最小面积约束";
        public int Floor { get; set; }

        //according
        public double Minarea { get; set; }
        public List<LineArea> LineAreas { get; set; } = new List<LineArea>();
        public List<MentalArea> MentalAreas { get; set; } = new List<MentalArea>();
    }
    public class LineArea
    {
        public LimitType LimitType { get; private set; } = LimitType.Line;
        public List<Point> Line { get; set; } = new List<Point>();
        public float Area { get; set; }
    }
    public class MentalArea
    {
        public LimitType LimitType { get; private set; } = LimitType.Mental;
        public Point Point { get; set; }
        public float Area { get; set; }
    }
}
