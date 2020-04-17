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

        private string Path4 { get; set; } = "prlviolation.txt";

        private string Path5 { get; set; } = "eofviolation.txt";
        private string Path6 { get; set; } = "cutsviolation.txt";
        private string Path7 { get; set; } = "ctcviolation.txt";

        private string Path8 { get; set; } = "areaviolation.txt";

        public void CandRead(List<Point> points)
        {
            if (File.Exists(Application.streamingAssetsPath+"/"+Path1))
            {
                using (FileStream fs = File.OpenRead(Application.streamingAssetsPath+"/"+Path1))
                {

                    using (StreamReader streamReader = new StreamReader(fs))
                    {
                        streamReader.ReadLine();
                        int count = 0;
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
        public void CreatResultRead(List<Net> nets,List<Pair> pairs)
        {
            if (File.Exists(Application.streamingAssetsPath + "/" + Path3))
            {
                using (FileStream fs = File.OpenRead(Application.streamingAssetsPath + "/" + Path3))
                {

                    using (StreamReader streamReader = new StreamReader(fs))
                    {
                        int netcount=int.Parse(streamReader.ReadLine().Trim());
                        for (int i = 0; i < netcount; i++)
                        {
                            int paircount = int.Parse(streamReader.ReadLine().Trim());
                            for (int j = 0; j <paircount ; j++)
                            {
                                var temp = Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => m.Groups[0].Value).ToArray();
                                if (temp.Length == 2)
                                {
                                    if (int.TryParse(temp[0], out int first))
                                    {
                                        if (int.TryParse(temp[1], out int second))
                                        {
                                            Pair pair = new Pair(first, second);
                                            nets[i].PathPair.Add(pair);
                                        }
                                    }
                                }
                                else
                                {
                                    Debug.Log("读入错误");
                                }
                            }
                        }
                        int t = int.Parse(streamReader.ReadLine());
                        for (int i = 0; i < t; i++)
                        {
                            string[] vs=streamReader.ReadLine().Split(' ');
                            Pair pair = new Pair(int.Parse(vs[0]), int.Parse(vs[1]));
                            pairs.Add(pair);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("不存在");
            }
        }

        public void PrlvRead(List<PanelLineLimit> lineToLines)
        {
            if (File.Exists(Application.streamingAssetsPath + "/" + Path4))
            {
                using (FileStream fs = File.OpenRead(Application.streamingAssetsPath + "/" + Path4))
                {
                    using (StreamReader streamReader = new StreamReader(fs))
                    {
                        //读取第一行的according
                        int floor = 0;
                        streamReader.ReadLine();
                        while (!streamReader.EndOfStream)
                        {
                            PanelLineLimit panelLineLimit = new PanelLineLimit()
                            { Floor = floor };

                                var temp = Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => m.Groups[0].Value).ToArray();
                                int rows = int.Parse(temp[0]);
                                int columns = int.Parse(temp[1]);
                                for (int i = 0; i < rows; i++)
                                {
                                    var row = Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => float.Parse(m.Groups[0].Value)).ToList();
                                    panelLineLimit.Form.Add(row);
                                }
                                int count=int.Parse(streamReader.ReadLine());
                                var columprop = Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => float.Parse(m.Groups[0].Value)).ToList();
                                for (int i = 0; i < count; i++)
                                {
                                    panelLineLimit.Prls.Add(columprop[i], i);
                                }
                                count = int.Parse(streamReader.ReadLine());
                                var withprop= Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => float.Parse(m.Groups[0].Value)).ToList();
                                for (int i = 0; i < count; i++)
                                {
                                    panelLineLimit.Widths.Add(withprop[i], i);
                                }

                                streamReader.ReadLine();//读取message
                                SetMessage<PanelLineLimit>(streamReader, panelLineLimit);

                            floor++;
                            lineToLines.Add(panelLineLimit);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("不存在");
            }
        }
        public void EofvRead(List<PanelEndtailLimit> panelEndtailLimits)
        {
            if (File.Exists(Application.streamingAssetsPath + "/" + Path5))
            {
                using (FileStream fs = File.OpenRead(Application.streamingAssetsPath + "/" + Path5))
                {
                    using (StreamReader streamReader = new StreamReader(fs))
                    {
                        //读取第一行的according
                        int floor = 0;
                        streamReader.ReadLine();
                        while (!streamReader.EndOfStream)
                        {
                            PanelEndtailLimit panelEndtailLimit = new PanelEndtailLimit()
                            { Floor = floor };

                                var temp = Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => float.Parse(m.Groups[0].Value)).ToArray();
                                panelEndtailLimit.Eolspace = temp[0];
                                panelEndtailLimit.Space = temp[1];
                                streamReader.ReadLine();//读取message
                                SetMessage<PanelEndtailLimit>(streamReader, panelEndtailLimit);
                            
                            floor++;
                            panelEndtailLimits.Add(panelEndtailLimit);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("不存在");
            }
        }
        public void CutsvRead(List<PanelThroughtLimit> panelThroughtLimits)
        {
            if (File.Exists(Application.streamingAssetsPath + "/" + Path6))
            {
                using (FileStream fs = File.OpenRead(Application.streamingAssetsPath + "/" + Path6))
                {
                    using (StreamReader streamReader = new StreamReader(fs))
                    {
                        //读取第一行的according
                        int floor = 0;
                        streamReader.ReadLine();
                        while (!streamReader.EndOfStream)
                        {
                            PanelThroughtLimit panelThroughtLimit = new PanelThroughtLimit()
                            { Floor = floor };

                            var temp = Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => float.Parse(m.Groups[0].Value)).ToArray();
                                panelThroughtLimit.Within = temp[0];
                                panelThroughtLimit.Adjspace = temp[1];
                                panelThroughtLimit.Num = (int)temp[2];
                                streamReader.ReadLine();//读取message
                                SetMessage<PanelThroughtLimit>(streamReader, panelThroughtLimit);
                            
                            floor++;
                            panelThroughtLimits.Add(panelThroughtLimit);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("不存在");
            }
        }
        public void CtcvRead(List<PanelEdgeLimit> panelEdgeLimits)
        {
            if (File.Exists(Application.streamingAssetsPath + "/" + Path7))
            {
                using (FileStream fs = File.OpenRead(Application.streamingAssetsPath + "/" + Path7))
                {
                    using (StreamReader streamReader = new StreamReader(fs))
                    {
                        //读取第一行的according
                        int floor = 0;
                        streamReader.ReadLine();
                        while (!streamReader.EndOfStream)
                        {
                            PanelEdgeLimit panelEdgeLimit  = new PanelEdgeLimit()
                            { Floor = floor };
                            int i = 0;
                            streamReader.ReadLine();
                            while (true)
                                {
                                    var t = streamReader.ReadLine();
                                    var temp = Regex.Matches(t, @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => float.Parse(m.Groups[0].Value)).ToArray();
                                    if (t.Equals("message"))
                                    {
                                        break;//读message
                                    }
                                    else
                                    {
                                        panelEdgeLimit.WithList.Add(temp[0],i);
                                        panelEdgeLimit.Spacing.Add(temp[1],i);
                                        i++;
                                    }
                                }
                                SetMessage(streamReader, panelEdgeLimit);
                            
                            floor++;
                            panelEdgeLimits.Add(panelEdgeLimit);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("不存在");
            }
        }
        public void AreaRead(List<PanelMinAreaLimit> panelMinAreaLimits)
        {
            if (File.Exists(Application.streamingAssetsPath + "/" + Path8))
            {
                using (FileStream fs = File.OpenRead(Application.streamingAssetsPath + "/" + Path8))
                {
                    using (StreamReader streamReader = new StreamReader(fs))
                    {
                        //读取第一行的according
                        int floor = 0;
                        streamReader.ReadLine();
                        while (!streamReader.EndOfStream)
                        {
                            PanelMinAreaLimit panelMinAreaLimit = new PanelMinAreaLimit()
                            { Floor = floor };

                                float t=float.Parse(streamReader.ReadLine());
                                panelMinAreaLimit.Minarea = t;
                                streamReader.ReadLine();//读取message
                                SetMessage(streamReader, panelMinAreaLimit);

                            floor++;
                            panelMinAreaLimits.Add(panelMinAreaLimit);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("不存在");
            }
        }

        private void SetMessage<T>(StreamReader streamReader,T PanelLimit)where  T:BasePanelLimit
        {
            while (true)
            {
                if (streamReader.EndOfStream)
                {
                    break;
                }
                string  t=streamReader.ReadLine();
                if (t.Equals("according"))
                {
                    break;
                } 
                switch (int.Parse(t))
                {
                    case 1:
                        {
                            var temp = Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => m.Groups[0].Value).ToArray();
                            string[] mental1 = new string[] { temp[0], temp[1], temp[2] };
                            var mental2 = new string[] { temp[3], temp[4], temp[5] };
                            Point mental1point = new Point();
                            Point mental2point = new Point();
                            mental1point.Change(mental1);
                            mental2point.Change(mental2);
                            MentalToMental mentalToMental = new MentalToMental()
                            {
                                FirstMental = mental1point,
                                SecondMenatl = mental2point
                            };
                            var temp1 = Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => float.Parse(m.Groups[0].Value)).ToArray();
                            mentalToMental.With1 = temp1[0];
                            mentalToMental.With2 = temp1[1];
                            if(temp1.Count()==4)
                            {
                                mentalToMental.Prl = temp1[2];
                                mentalToMental.Spacing = temp1[3];
                            }
                            mentalToMental.SetMaxWith();
                            PanelLimit.MentalToMentals.Add(mentalToMental);
                        }
                        break;
                    case 2:
                        {
                            var temp = Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => m.Groups[0].Value).ToArray();
                            string[] mental = new string[] { temp[0], temp[1], temp[2] };
                            string[] lineFirst = new string[] { temp[3], temp[4], temp[5] };
                            string[] lineSecond = new string[] { temp[6], temp[7], temp[8] };
                            Point mentalpoint = new Point();
                            Point lineFirstpoint = new Point();
                            Point lineSecondpoint = new Point();
                            mentalpoint.Change(mental);
                            lineFirstpoint.Change(lineFirst);
                            lineSecondpoint.Change(lineSecond);
                            MentalToLine mentalToLine = new MentalToLine()
                            { Mental = mentalpoint };
                            mentalToLine.Line.Add(lineFirstpoint);
                            mentalToLine.Line.Add(lineSecondpoint);
                            var temp1 = Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => float.Parse(m.Groups[0].Value)).ToArray();
                            mentalToLine.With1 = temp1[0];
                            mentalToLine.With2 = temp1[1];
                            PanelLimit.MentalToLines.Add(mentalToLine);
                        }
                        break;
                    case 3:
                        {
                            var temp = Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => m.Groups[0].Value).ToArray();
                            string[] mental = new string[] { temp[6], temp[7], temp[8] };
                            string[] lineFirst = new string[] { temp[0], temp[1], temp[2] };
                            string[] lineSecond = new string[] { temp[3], temp[4], temp[5] };
                            Point mentalpoint = new Point();
                            Point lineFirstpoint = new Point();
                            Point lineSecondpoint = new Point();
                            mentalpoint.Change(mental);
                            lineFirstpoint.Change(lineFirst);
                            lineSecondpoint.Change(lineSecond);
                            MentalToLine mentalToLine = new MentalToLine()
                            { Mental = mentalpoint };
                            mentalToLine.Line.Add(lineFirstpoint);
                            mentalToLine.Line.Add(lineSecondpoint);
                            var temp1 = Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => float.Parse(m.Groups[0].Value)).ToArray();
                            mentalToLine.With1 = temp1[0];
                            mentalToLine.With2 = temp1[1];
                            PanelLimit.MentalToLines.Add(mentalToLine);
                        }
                        break;
                    case 4:
                        {
                            var temp = Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => m.Groups[0].Value).ToArray();
                            string[] line1First = new string[] { temp[0], temp[1], temp[2] };
                            string[] line1Second = new string[] { temp[3], temp[4], temp[5] };
                            string[] line2First = new string[] { temp[6], temp[7], temp[8] };
                            string[] line2Second = new string[] { temp[9], temp[10], temp[11] };
                            Point line1Firstpoint = new Point();
                            Point line1Secondpoint = new Point();
                            Point line2Firstpoint = new Point();
                            Point line2Secondpoint = new Point();
                            line1Firstpoint.Change(line1First);
                            line1Secondpoint.Change(line1Second);
                            line2Firstpoint.Change(line2First);
                            line2Secondpoint.Change(line2Second);
                            LineToLine lineToLine = new LineToLine();
                            lineToLine.FirstLine.Add(line1Firstpoint);
                            lineToLine.FirstLine.Add(line1Secondpoint);
                            lineToLine.SecondLine.Add(line2Firstpoint);
                            lineToLine.SecondLine.Add(line2Secondpoint);
                            var temp1 = Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => float.Parse(m.Groups[0].Value)).ToArray();
                            if(temp1.Count()==2)
                            {
                                lineToLine.With1 = temp1[0];
                                lineToLine.With2 = temp1[1];
                            }
                            if(temp1.Count()==1)
                            {
                                lineToLine.MaxWith = temp1[0];
                            }
                            if (temp1.Count() == 4)
                            {
                                lineToLine.Prl = temp1[2];
                                lineToLine.Spacing = temp1[3];
                            }
                            PanelLimit.LineToLines.Add(lineToLine);
                        }
                        break;
                }
            }
        }
        private void SetMessage(StreamReader streamReader,PanelMinAreaLimit panelMinAreaLimit)
        {
            while (true)
            {
                string t = streamReader.ReadLine();
                if (streamReader.EndOfStream)
                {
                    break;
                }
                if (t.Equals("according"))
                {
                    break;
                }
                switch (int.Parse(t))
                {
                    case 1:
                        {
                            var temp = Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => m.Groups[0].Value).ToArray();
                            LineArea lineArea = new LineArea();
                            Point point1 = new Point();
                            Point point2 = new Point();
                            string[] temp1 = new string[] { temp[0], temp[1], temp[2] };
                            string[] temp2 = new string[] { temp[3], temp[4], temp[5] };
                            point1.Change(temp1);
                            point2.Change(temp2);
                            lineArea.Line.Add(point1);
                            lineArea.Line.Add(point2);
                            float area = float.Parse(streamReader.ReadLine());
                            lineArea.Area = area;
                            panelMinAreaLimit.LineAreas.Add(lineArea);
                        }
                        break;
                    case 2:
                        {
                            var temp = Regex.Matches(streamReader.ReadLine(), @"(((\d+)\.){1}\d+)|\d+").OfType<Match>().Select(m => m.Groups[0].Value).ToArray();
                            MentalArea mentalArea = new MentalArea();
                            Point point = new Point();
                            point.Change(temp);
                            mentalArea.Point = point;
                            float area = float.Parse(streamReader.ReadLine());
                            mentalArea.Area = area;
                            panelMinAreaLimit.MentalAreas.Add(mentalArea);
                        }
                        break;
                }
            }
        }
    }
    

}
