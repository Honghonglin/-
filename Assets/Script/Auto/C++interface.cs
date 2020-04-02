using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Auto
{
    enum ErrorType
    {
        
    }
    public class Point
    {
       public Point() { }
       public Point(double X,double Y,int Floor,int Number)
        {
            this.X = X;
            this.Y = Y;
            this.Floor = Floor;
            this.Number = Number;
        }
        private double  X{ get;  set; }
        private double Y { get;  set; }
        private int Floor { get;  set; }//Floor从0开始
        public int Number { get; set; }
        public void Change(string[] temp)
        {
            X = double.Parse(temp[0]);
            Y = double.Parse(temp[1]);
            Floor = int.Parse(temp[2]);
        }


        public override string ToString()
        {
            return X.ToString() + " " + Y.ToString() + " " + Floor.ToString();
        }
        public Vector3 GetVector3()
        {
            return new Vector3((float)X, Floor*1f, (float)Y);
        }
        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if(this!=obj)
            {
                if(obj is Point)
                {
                    Point point= obj as Point;
                    float x1 = (float)point.X;
                    float x2 = (float)this.X;
                    float y1 = (float)point.Y;
                    float y2 = (float)this.Y;
                    if (Mathf.Abs(x1-x2)<0.1f&&Mathf.Abs(y1-y2)<0.1&&Mathf.Abs(point.Floor-this.Floor)==0)
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
    }
    class Line
    {
        
    }
    class Mental
    {
        Point point;
    }
    class Wrong
    {
        // 违反限制的两条线
        ErrorType _errorType;// 这两条线违反约束的类型
    }
    public class Net
    {
        public List<Point> Points { get; set; } = new List<Point>();
        public List<GameObject> PointsInstant { get; set; } = new List<GameObject>();
        public List<Text> Texts { get; set; } = new List<Text>();

    }
}
