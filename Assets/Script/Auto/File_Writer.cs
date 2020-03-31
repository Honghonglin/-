using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

namespace Assets.Script.Auto
{
    public class File_Writer
    {
        private string Path1 { get;  set; } = "routingspace.txt";
        private string Path2 { get;  set; } = "pinmessage.txt";
        private string Path3 { get;  set; } = "createin.txt";
        /// <summary>
        /// Path1Writer
        /// </summary>
        public void RouWriter(int floor, int length, int width, int pathway)
        {
            if (File.Exists(Application.streamingAssetsPath + "/" + Path1))
            {
                File.Delete(Application.streamingAssetsPath + "/" + Path1);
            }
            using (FileStream fs = File.Create(Application.streamingAssetsPath + "/" + Path1))
            {
                using (StreamWriter streamWriter = new StreamWriter(fs))
                {
                    streamWriter.WriteLine(floor.ToString());
                    streamWriter.WriteLine(length.ToString() + " " + width.ToString());
                    streamWriter.WriteLine(pathway.ToString());
                }

            }
        }
        /// <summary>
        /// Path2Writer
        /// </summary>
        /// <param name="count"></param>
        public void PinWriter(int count)
        {
            if (File.Exists(Application.streamingAssetsPath + "/" + Path2))
            {
                File.Delete(Application.streamingAssetsPath + "/" + Path2);
            }
            using (FileStream fs = File.Create(Application.streamingAssetsPath + "/" + Path2))
            {
                using (StreamWriter streamWriter = new StreamWriter(fs))
                {
                    streamWriter.WriteLine(count.ToString());
                }
            }
        }
        public void CreatWriter(CreateClass createClass)
        {
            if (File.Exists(Application.streamingAssetsPath + "/" + Path3))
            {
                File.Delete(Application.streamingAssetsPath + "/" + Path3);
            }
            using (FileStream fs = File.Create(Application.streamingAssetsPath + "/" + Path3))
            {
                using (StreamWriter streamWriter = new StreamWriter(fs))
                {
                    streamWriter.WriteLine(createClass.AllPointCount);
                    streamWriter.WriteLine(createClass.Pairs.Count);
                    foreach (var item in createClass.Pairs)
                    {
                        streamWriter.WriteLine(item);
                    }
                    streamWriter.WriteLine(createClass.Nets.Count);
                    foreach (var net in createClass.Nets)
                    {
                        streamWriter.WriteLine(net.Points.Count);
                        foreach (var point in net.Points)
                        {
                            streamWriter.Write(point.Number + " ");
                        }
                        streamWriter.WriteLine();
                    }
                }
            }
        }

        public class CreateClass
        {
            public int AllPointCount { get; set; }
            public List<Pair> Pairs { get; set; } = new List<Pair>();
            public List<Net> Nets { get; set; } = new List<Net>();
        }

    }
    public class Pair
    {
        public Pair(Point first, Point second)
        {
            First = first.Number;
            Second = second.Number;
        }
        public Pair(int first,int second)
        {
            First = first;
            Second = second;
        }
        private int First { get; set; }
        private int Second { get; set; }
        public override string ToString()
        {
            return First.ToString() + " " + Second.ToString();
        }
    }





}

