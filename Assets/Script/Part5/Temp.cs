﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Part5;
using System.Runtime.InteropServices;
using Assets.Script.Auto;

/// <summary>
/// 没用
/// </summary>
public class Temp : MonoBehaviour
{
    //public Dropdown dropdown3;
    //public Dropdown dropdown2;
    //public GameObject t;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    //dropdown2.ClearOptions();
    //    //List<string> vs = new List<string>() { "线网5" };
    //    //dropdown2.AddOptions(vs);
    //    GameObject k=Instantiate(t,gameObject.transform);
    //    k.GetComponent<ImportantPoint_number>().Number = 5;
    //}
    //private void Update()
    //{
    //    //print(int.Parse(dropdown2.captionText.text.Replace("线网", "")));
    //    //dropdown3.onValueChanged.AddListener((int v) => OnValue(5));

    //}
    //public void OnValue(int v)
    //{
    //    Debug.Log(v);
    //}
    //public void Pro()
    //{
    //    List<string> vs = new List<string>();
    //    for (int i = 1; i <= 5; i++)
    //    {
    //        vs.Add(i.ToString());
    //    }
    //    dropdown3.AddOptions(vs);
    //}

    //[DllImport("link 9")]
    //private static extern double Multip(double x, double y);
    //[DllImport("link 9")]
    //private static extern void Func();

    private void Start()
    {
        File_Writer file_Writer = new File_Writer();
        //file_Writer.RouWriter(5, 90, 40, 5);
        //file_Writer.PinWriter(4);
        //Func();
        File_Reader file_Reader = new File_Reader();
        //List<Point> points = new List<Point>();
        //file_Reader.CandRead(points);
        //List<Net> nets = new List<Net>();
        //file_Reader.RanRead(nets);
        //foreach (var net in nets)
        //{
        //    foreach (var item in net.Points)
        //    {
        //        Debug.Log(item);
        //    }
        //}
        File_Writer.CreateClass createClass = new File_Writer.CreateClass
        {
            AllPointCount = 6
        };
        Point[] points = new Point[6];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new Point();
        }
        for (int i = 0; i < 6; i++)
        {
            points[i].Number = i+1;
        }
        for (int i = 0; i < 6; i+=2)
        {
            createClass.Pairs.Add(new Pair(points[i], points[i + 1]));
        }
        Net[] nets = new Net[2];
        for (int i = 0; i < nets.Length; i++)
        {
            nets[i] = new Net();
        }
        for (int i = 0; i < 2; i++)
        {
            nets[i].Points.Add(points[0+i]);
            nets[i].Points.Add(points[2+i]);
        }
        foreach (var item in nets)
        {
            createClass.Nets.Add(item);
        }
        file_Writer.CreatWriter(createClass);

        List<Pair> pairs =new List<Pair>();
        file_Reader.CreatResultRead(pairs);
        foreach (var pair in pairs)
        {
            Debug.Log(pair);
        }

    }

}
