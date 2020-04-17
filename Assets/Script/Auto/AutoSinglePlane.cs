using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Script.Auto
{
  public  class AutoSinglePlane:MonoBehaviour
    {
        public static float lenght;
        public static float width;
        public float Spacing
        {
            get
            {
                if (Type == PathType.Horizontal)
                {
                    return width / PathCount;
                }
                else
                {
                    return lenght / PathCount;
                }
            } set { } }
        public int Floor { get; set; } = 0;
        public enum PathType
        {
            Horizontal,
            Vertical
        }
        public PathType Type { get; set; } = PathType.Horizontal;

        public List<Point> points = new List<Point>();
        public int PathCount { get; set; }

        /// <summary>
        /// 每个轨道拥有的Point组
        /// </summary>
        public List<List<Point> > EveryPathPoints { get; set; } = new List<List<Point> >();
        
        public void SetEveryPathPoints()
        {
            EveryPathPoints.Clear();
            if(Type==PathType.Horizontal)
            {
                for (int i = 0; i < PathCount; i++)
                {
                    List<Point> _points = new List<Point>();
                    foreach (var point in points)
                    {
                        if(point.YEqual(points[i]))
                        {
                            _points.Add(point);
                        }
                    }
                    _points.Sort((a, b) =>
                    {
                        if ((float)a.GetX() - (float)b.GetX() > 0.1f)
                            return 1;
                        else if ((float)b.GetX() - (float)a.GetX() > 0.1f)
                            return -1;
                        else
                            return 0;
                    });
                    EveryPathPoints.Add(_points);
                }
            }
            if(Type==PathType.Vertical)
            {
                List<double> vs = new List<double>();
                foreach (var point in points)
                {
                    if(vs.Contains(point.GetX()))
                    {
                    }
                    else
                    {
                        vs.Add(point.GetX());
                    }
                }
                for (int i = 0; i < PathCount; i++)
                {
                    List<Point> _points = new List<Point>();
                    foreach (var point in points)
                    {
                        if(Mathf.Abs((float)point.GetX()-(float)vs[i])<0.1f)
                        {
                            _points.Add(point);
                        }
                    }
                    _points.Sort((a, b) =>
                    {
                        if ((float)a.GetY() - (float)b.GetY() > 0.1f)
                            return 1;
                        else if ((float)b.GetY() - (float)a.GetY() > 0.1f)
                            return -1;
                        else
                            return 0;
                    });
                    EveryPathPoints.Add(_points);
                }
            }
            #region 调试
            print($"这是第{Floor}层:\n");
            foreach (var pointlist in EveryPathPoints)
            {
                foreach (var point in pointlist)
                {
                    print(point);
                }
            }
            #endregion

        }

        public List<Dictionary<Point, List<int>>> KeyValuePairs { get; set; } = new List<Dictionary<Point, List<int>>>();
        
        public void SetkeyValuePairs()
        {
            foreach (var listpoint in EveryPathPoints)
            {
                Dictionary<Point, List<int>> valuePairs = new Dictionary<Point, List<int>>();
                foreach (var point in listpoint)
                {
                    valuePairs.Add(point,new List<int> { 0 });
                }
                KeyValuePairs.Add(valuePairs);
            }
            #region 调试
            print(KeyValuePairs.Count);
            foreach (var valusPairs in KeyValuePairs)
            {
                foreach (var pair in valusPairs)
                {
                    Debug.Log("Key为:"+pair.Key.ToString());
                    foreach (var temp in pair.Value)
                    {
                        print("Value为:" + temp.ToString());
                    }
                }
            }
            #endregion
        }
        public override string ToString()
        {

            string str = "Point组\n";
            foreach (var item in points)
            {
                str += item.ToString() +"Number" +item.Number.ToString()+"\n";
            }

            return "长度为:" + lenght.ToString() +"\n"+
                "宽度为:" + width.ToString() +"\n"+
                "Spacing为:" + Spacing.ToString() +"\n"+
                "Floor为:" + Floor.ToString() + "\n" +
                "Type为" + Type.ToString()+ "\n" +
                "PathCount为:" +PathCount.ToString()+"\n"+
                str;

        }

    }
}
