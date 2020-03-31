using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePanel : MonoBehaviour
{
    public int  Length { get; set; }
    public int  Width { get; set; } 
    public int Pathway_count { get; set; }
    public float Line_width { get; set; }
    public bool Is_across { get; set; }
    public int Floor { get; set; }
    public Material Mat { get; set; }
    /// <summary>
    /// 轨道集合
    /// </summary>
    public List<GameObject> Lis { get; set; } = new List<GameObject>();

    public bool IsSelet { get; set; } = false;

    public bool IsHave { get; set; } = false;
    public bool IsAppearance { get; set; } = false;

    private void Update()
    {
        if(!IsHave)
        {
            if (Is_across)
            {
                CreatPathway();
            }
            else
            {
                CreatPathway1();
            }
            IsHave = true;
        }
        if(!IsAppearance&&Lis.Count!=0&&IsHave)
        {
            foreach (var item in Lis)
            {
                item.SetActive(false);
            }
        }
        if(IsHave && Lis.Count!=0&&IsAppearance)
        {
            foreach (var item in Lis)
            {
                item.SetActive(true);
            }
        }
        if(Input.GetMouseButton(2)&&Lis.Count!=0)
        {
            Reset_Pos();
        }
    }
    public void Reset_Pos()
    {
        if (Is_across)
        {
            Vector3 offset = gameObject.transform.Find("leftnorth").position - gameObject.transform.Find("leftsorth").position;
            float lineinteval = offset.magnitude / (Pathway_count - 1);
            Vector3 firstleft = gameObject.transform.Find("leftnorth").position;
            Vector3 firstright = gameObject.transform.Find("rightnorth").position;
            for (int i = 0; i < Lis.Count; i++)
            {
                Vector3 left = firstleft - i * lineinteval * offset.normalized;
                Vector3 right = firstright - i * lineinteval * offset.normalized;
                LineRenderPosSet(left, right, Lis[i].GetComponent<LineRenderer>());
            }
        }
        else
        {
            Vector3 offset = gameObject.transform.Find("rightnorth").position - gameObject.transform.Find("leftnorth").position;
            float lineinteval = offset.magnitude / (Pathway_count - 1);
            Vector3 firstleft = gameObject.transform.Find("leftnorth").position;
            Vector3 firstright = gameObject.transform.Find("leftsorth").position;
            for (int i = 0; i < Lis.Count; i++)
            {
                Vector3 left = firstleft + i * lineinteval * offset.normalized;
                Vector3 right = firstright + i * lineinteval * offset.normalized;
                LineRenderPosSet(left, right, Lis[i].GetComponent<LineRenderer>());
            }
        }
    }

    /// <summary>
    /// 显示当前层级轨道1
    /// </summary>
    private void CreatPathway1()
    {
        Vector3 offset = gameObject.transform.Find("rightnorth").position - gameObject.transform.Find("leftnorth").position;
        float sum = (Pathway_count - 1) * (ConstClass.interval + ConstClass.min_lineWidth);
        if (offset.magnitude < sum)
        {
            Debug.LogError("wrong");
        }
        else
        {
            float lineinteval = offset.magnitude / (Pathway_count - 1);
            for (int i = 0; i < Pathway_count; i++)
            {
                Vector3 left = gameObject.transform.Find("leftnorth").position + i * lineinteval * offset.normalized;
                Vector3 right = gameObject.transform.Find("leftsorth").position + i * lineinteval * offset.normalized;
                Pathwayactive(left, right);
            }
        }
    }
    /// <summary>
    /// 显示当前层级轨道
    /// </summary>
    private void CreatPathway()
    {
        Vector3 offset = gameObject.transform.Find("leftnorth").position - gameObject.transform.Find("leftsorth").position;
        float sum = (Pathway_count - 1) * (ConstClass.interval + ConstClass.min_lineWidth);
        if (offset.magnitude < sum)
        {
            Debug.LogError("wrong");
        }
        else
        {
            float lineinteval = offset.magnitude / (Pathway_count - 1);
            for (int i = 0; i < Pathway_count; i++)
            {
                Vector3 left = gameObject.transform.Find("leftnorth").position - i * lineinteval * offset.normalized;
                Vector3 right = gameObject.transform.Find("rightnorth").position - i * lineinteval * offset.normalized;
                Pathwayactive(left, right);
            }
        }
    }
    /// <summary>
    /// 生成该层的轨道
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    private void Pathwayactive(Vector3 left, Vector3 right)
    {
        GameObject _gameObject = new GameObject();
        Vector3 NitVec = new Vector3(0, gameObject.transform.Find("leftnorth").position.y, 0).normalized;
        _gameObject.transform.SetParent(gameObject.transform);
        LineRenderer lineRenderer = _gameObject.AddComponent<LineRenderer>();
        lineRenderer.GetComponent<LineRenderer>().SetPosition(0, left-NitVec*0.02f);
        lineRenderer.GetComponent<LineRenderer>().SetPosition(1, right - NitVec * 0.02f);
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = Mat;
        Lis.Add(_gameObject);
    }

    /// <summary>
    /// 设置LineRender的位置
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    public void LineRenderPosSet(Vector3 left,Vector3 right,LineRenderer lineRenderer)
    {
        Vector3 NitVec = new Vector3(0, gameObject.transform.Find("leftnorth").position.y, 0).normalized;
        lineRenderer.GetComponent<LineRenderer>().SetPosition(0, left - NitVec * 0.02f);
        lineRenderer.GetComponent<LineRenderer>().SetPosition(1, right - NitVec * 0.02f);
    }
}

