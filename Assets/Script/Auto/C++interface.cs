using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Auto
{
    public class Point
    {
        public Point() { }
        public Point(double X, double Y, int Floor, int Number)
        {
            this.X = X;
            this.Y = Y;
            this.Floor = Floor;
            this.Number = Number;
        }
        public Point(Point point)
        {
            this.X = point.X;
            this.Y = point.Y;
            this.Floor = point.Floor;
            this.Number = point.Number;
        }
        private double X { get; set; }
        private double Y { get; set; }
        public int Floor { get; set; }//Floor从0开始
        public int Number { get; set; }
        public void Change(string[] temp)
        {
            X = double.Parse(temp[0]) / Date.Mulsize;
            Y = double.Parse(temp[1]) / Date.Mulsize;
            Floor = int.Parse(temp[2]);
        }
        public double GetX()
        {
            return X;
        }
        public double GetY()
        {
            return Y;
        }
        public override string ToString()
        {
            return X.ToString() + " " + Y.ToString() + " " + Floor.ToString();
        }
        public Vector3 GetVector3()
        {
            return new Vector3((float)X, Floor * 1f, (float)Y);
        }
        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (this != obj)
            {
                if (obj is Point)
                {
                    Point point = obj as Point;
                    float x1 = (float)point.X;
                    float x2 = (float)this.X;
                    float y1 = (float)point.Y;
                    float y2 = (float)this.Y;
                    if (Mathf.Abs(x1 - x2) < 0.1f && Mathf.Abs(y1 - y2) < 0.1 && Mathf.Abs(point.Floor - this.Floor) == 0)
                    {
                        return true;
                    }

                }
            }
            return false;
        }
        public override int GetHashCode()
        {
            int result = 17;
            result = 31 * result + X.GetHashCode();
            result = 31 * result + Y.GetHashCode();
            result = 31 * result + Floor.GetHashCode();
            return result;
        }
        public bool YEqual(Point point)
        {
            if (Mathf.Abs((float)point.Y - (float)this.Y) <= 0.1f)
            {
                return true;
            }
            return false;
        }
        public bool XEqual(Point point)
        {
            if (Mathf.Abs((float)point.X - (float)this.X) <= 0.1f)
            {
                return true;
            }
            return false;
        }
        public Point GetLimitPoint()
        {
            Point point = new Point(X * 10, Y * 10, Floor, Number);
            return point;
        }
    }
    public class Net
    {
        //引脚
        public List<Point> Points { get; set; } = new List<Point>();
        //引脚
        public List<GameObject> PointsInstant { get; set; } = new List<GameObject>();
        public List<Text> Texts { get; set; } = new List<Text>();

        public List<Pair> PathPair { get; set; } = new List<Pair>();

        /// <summary>
        /// 水平导线
        /// </summary>
        public List<List<Point>> LinePoint { get; set; } = new List<List<Point>>();
        /// <summary>
        ///水平导线实例
        /// </summary>
        public List<GameObject> LinePointInstant { get; set; } = new List<GameObject>();
        /// <summary>
        /// 竖直导线
        /// </summary>
        public List<List<Point>> VerticalPoints { get; set; } = new List<List<Point>>();
        /// <summary>
        /// 竖直的导线对
        /// </summary>
        public List<Pair> VerticalPairs { get; set; } = new List<Pair>();
        /// <summary>
        /// 竖直实例
        /// </summary>
        public List<GameObject> VerticalInstance { get; set; } = new List<GameObject>();
        /// <summary>
        /// 通孔实例
        /// </summary>
        public List<GameObject> Mental { get; set; } = new List<GameObject>();


        /// <summary>
        /// 水平实例
        /// </summary>
        public List<GameObject> HoriLine { get; set; } = new List<GameObject>();


    }
    public class Line_RecoverLine
    {
        public List<Point> Line { get; set; } = new List<Point>();
        public List<List<Point>> Recoverlines { get; set; } = new List<List<Point>>();

        public override string ToString()
        {
            Debug.Log("-------导线分割线-------");
            Debug.Log("Line为：");
            foreach (var point in Line)
            {
                Debug.Log(point);
            }
            Debug.Log("----Recoverlines:");
            foreach (var lines in Recoverlines)
            {
                Debug.Log("Recoverline");
                foreach (var point in lines)
                {
                    Debug.Log(point);
                }
            }
            return "";
        }
    }
    public class Line_RecoverPoint
    {
        public Point MentalPoint { get; set; }
        public List<List<Point>> Lines { get; set; } = new List<List<Point>>();
    }
    public class Point_RecoverPoint
    {
        public Point MentalPoint { get; set; }
        public List<Point> MentalPoints { get; set; } = new List<Point>();
    }
}
