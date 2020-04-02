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
        public int Floor { get; set; }
        public enum PathType
        {
            Horizontal,
            Vertical
        }
        public PathType Type { get; set; } = PathType.Horizontal;

        public List<Point> points = new List<Point>();
        public int PathCount { get; set; }

        

    }
}
