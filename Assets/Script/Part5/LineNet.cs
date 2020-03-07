using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Part5
{
    public class LineNet
    {
        /// <summary>
        /// 线网引脚
        /// </summary>
        public List<GameObject> Point { get; set; } = new List<GameObject>();
        /// <summary>
        /// 线网金属线集
        /// </summary>
        public List<GameObject> Mental { get; set; } = new List<GameObject>();
        /// <summary>
        /// 线网导管
        /// </summary>
        public List<GameObject> Line { get; set; } = new List<GameObject>();
        public int  LineNumber { get; set; }
    }
}
