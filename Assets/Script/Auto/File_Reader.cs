using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;
namespace Assets.Script.Auto
{
   public class File_Reader
    {
        
        private string Path1 { get; set; } = "candidatepin.txt";
        private string Path2 { get;  set; } = "randpin.txt";

        private string Path3 { get; set; } = "result.txt";

        public void CandRead(List<Point> points)
        {
            if (File.Exists(Application.streamingAssetsPath+"/"+Path1))
            {
                using (FileStream fs = File.OpenRead(Application.streamingAssetsPath+"/"+Path1))
                {

                    using (StreamReader streamReader = new StreamReader(fs))
                    {
                        streamReader.ReadLine();
                        int count = 1;
                        while (!streamReader.EndOfStream)
                        {
                            var temp = Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => m.Groups[0].Value).ToArray();
                            Point point = new Point();
                            point.Change(temp);
                            point.Number = count;
                            count++;
                            points.Add(point);
                        }
                    } 
                }
            }
            else
            {
                Debug.Log("不存在");
            }
        }
        public void RanRead(List<Net> nets)
        {
            if (File.Exists(Application.streamingAssetsPath + "/" + Path2))
            {
                using (FileStream fs = File.OpenRead(Application.streamingAssetsPath + "/" + Path2))
                {

                    using (StreamReader streamReader = new StreamReader(fs))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            int length=int.Parse(streamReader.ReadLine());
                            Net lineNet = new Net();
                            for (int i = 0; i < length; i++)
                            {
                                var temp = Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => m.Groups[0].Value).ToArray();
                                Point point = new Point();                                
                                point.Change(temp);
                                foreach (var point1 in Date.allBasePoint)
                                {
                                    if (point1.Equals(point))
                                    {
                                        point.Number = point1.Number;
                                    }
                                }
                                lineNet.Points.Add(point);
                            }
                            nets.Add(lineNet);
                            streamReader.ReadLine();
                        }
                    }
                }
            }
            else
            {
                Debug.Log("不存在");
            }
        }
        public void CreatResultRead(List<Pair> pairs)
        {
            if (File.Exists(Application.streamingAssetsPath + "/" + Path3))
            {
                using (FileStream fs = File.OpenRead(Application.streamingAssetsPath + "/" + Path3))
                {

                    using (StreamReader streamReader = new StreamReader(fs))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            var temp = Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => m.Groups[0].Value).ToArray();
                            if(temp.Length==2)
                            {
                                int first=0;
                                int second=0;
                                if(int.TryParse(temp[0],out first))
                                {
                                    if (int.TryParse(temp[1], out second))
                                    {
                                        Pair pair = new Pair(first, second);
                                        pairs.Add(pair);
                                    }
                                }
                                
                            }
                            else
                            {
                                Debug.Log("读入错误");
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.Log("不存在");
            }
        }

    }
}
