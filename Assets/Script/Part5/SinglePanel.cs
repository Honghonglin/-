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
    }

    /// <summary>
    /// 显示当前层级轨道1
    /// </summary>
    private void CreatPathway1()
    {
        Vector3 offset = gameObject.transform.Find("rightnorth").position - gameObject.transform.Find("leftnorth").position;
        float sum = (Pathway_count - 1) * (ConstClass.interval + ConstClass.min_lineWidth);
        if (offset.x < sum)
        {
            Debug.LogError("wrong");
        }
        else
        {
            float lineinteval = offset.x / (Pathway_count - 1);
            for (int i = 0; i < Pathway_count; i++)
            {
                Vector3 left = gameObject.transform.Find("leftnorth").position + new Vector3(i * lineinteval, 0, 0);
                Vector3 right = gameObject.transform.Find("leftsorth").position + new Vector3(i * lineinteval, 0, 0);
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
    public void LineRenderPosSet(Vector3 left,Vector3 right)
    {
        if(Lis.Count!=0)
        {
            Vector3 NitVec = new Vector3(0, gameObject.transform.Find("leftnorth").position.y, 0).normalized;
            foreach (var item in Lis)
            {
                LineRenderer lineRenderer = item.GetComponent<LineRenderer>();
                lineRenderer.GetComponent<LineRenderer>().SetPosition(0, left - NitVec * 0.02f);
                lineRenderer.GetComponent<LineRenderer>().SetPosition(1, right - NitVec * 0.02f);
            }
        }
    }
}

