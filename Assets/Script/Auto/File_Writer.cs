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
        private string Path3 { get;  set; } = "temp.txt";
        private string Path4 { get; set; } = "Wire.txt";
        private string Path5 { get; set; } = "mental.txt";
        private string Path6 { get; set; } = "mental1.txt";

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
        
        public void LimitWireWriter(Limit limit)
        {
            if (File.Exists(Application.streamingAssetsPath + "/" + Path4))
            {
                File.Delete(Application.streamingAssetsPath + "/" + Path4);
            }
            using (FileStream fs = File.Create(Application.streamingAssetsPath + "/" + Path4))
            {
                using (StreamWriter streamWriter = new StreamWriter(fs))
                {
                    streamWriter.WriteLine(limit.Line_recoverLines.Count);
                    foreach (var line_RecoverLine in limit.Line_recoverLines)
                    {
                        streamWriter.WriteLine(line_RecoverLine.Line[line_RecoverLine.Line.Count - 1].GetLimitPoint()+" "
                            +line_RecoverLine.Line[0].GetLimitPoint());
                        streamWriter.WriteLine(line_RecoverLine.Recoverlines.Count);
                        foreach (var line in line_RecoverLine.Recoverlines)
                        {
                            streamWriter.WriteLine(line[line.Count - 1].GetLimitPoint() + " " + line[0].GetLimitPoint());
                        }
                    }
                }
            }
        }
        
        public void LimitMentalWriter(Limit limit)
        {
            if (File.Exists(Application.streamingAssetsPath + "/" + Path5))
            {
                File.Delete(Application.streamingAssetsPath + "/" + Path5);
            }
            using (FileStream fs = File.Create(Application.streamingAssetsPath + "/" + Path5))
            {
                using (StreamWriter streamWriter = new StreamWriter(fs))
                {
                    streamWriter.WriteLine(limit.Line_recoverPoints.Count);
                    foreach (var line_RecoverPoint in limit.Line_recoverPoints)
                    {
                        streamWriter.WriteLine(line_RecoverPoint.MentalPoint.GetLimitPoint());
                        streamWriter.WriteLine(line_RecoverPoint.Lines.Count);
                        foreach (var line in line_RecoverPoint.Lines)
                        {
                            streamWriter.WriteLine(line[line.Count - 1].GetLimitPoint() + " " + line[0].GetLimitPoint());
                        }
                    }
                }
            }
        }
        public void LimitMental1Writer(Limit limit)
        {
            if (File.Exists(Application.streamingAssetsPath + "/" + Path6))
            {
                File.Delete(Application.streamingAssetsPath + "/" + Path6);
            }
            using (FileStream fs = File.Create(Application.streamingAssetsPath + "/" + Path6))
            {
                using (StreamWriter streamWriter = new StreamWriter(fs))
                {
                    streamWriter.WriteLine(limit.Point_recoverPoints.Count);
                    foreach (var point_RecoverPoint in limit.Point_recoverPoints)
                    {
                        streamWriter.WriteLine(point_RecoverPoint.MentalPoint.GetLimitPoint());
                        streamWriter.WriteLine(point_RecoverPoint.MentalPoints.Count);
                        foreach (var point in point_RecoverPoint.MentalPoints)
                        {
                            streamWriter.WriteLine(point.GetLimitPoint());
                        }
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
        public int First { get; set; }
        public int Second { get; set; }
        public override string ToString()
        {
            return First.ToString() + " " + Second.ToString();
        }
        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            if(obj!=this)
            {
                if(obj is Pair)
                {
                    Pair pair = obj as Pair;
                    if(pair.First==this.First&&pair.Second==this.Second)
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
            result = 31 * result + First.GetHashCode();
            result = 31 * result + Second.GetHashCode();
            return result;
        }

    }

    public class Limit
    {
     public   List<Line_RecoverLine> Line_recoverLines { get; set; } = new List<Line_RecoverLine>();
      public  List<Line_RecoverPoint>     Line_recoverPoints { get; set; } = new List<Line_RecoverPoint>();
     public   List<Point_RecoverPoint> Point_recoverPoints { get; set; } = new List<Point_RecoverPoint>();
    }



}

