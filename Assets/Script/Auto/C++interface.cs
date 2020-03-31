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
        private double  X{ get;  set; }
        private double Y { get;  set; }
        private int Floor { get;  set; }
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
