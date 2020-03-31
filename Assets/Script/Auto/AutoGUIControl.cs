using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

namespace Assets.Script.Auto
{
    class AutoGUIControl:MonoBehaviour
    {
        public Button[] buttons;
        public GameObject TextContain;
        private GameObject textPrefab;
        private GameObject gameCanvas;

        private void Start()
        {
            textPrefab = Resources.Load<GameObject>("Text");
            gameCanvas = GameObject.FindGameObjectWithTag("Canvas");
            foreach (var item in buttons)
            {
                string t = item.transform.Find("Text").GetComponent<Text>().text;
                if( t== "显示随机生成的引脚")
                {
                    item.onClick.AddListener(() => CreatText());
                }
                if(t =="返回1")
                {
                    item.onClick.AddListener(() => DestroyText());
                }
                if(t=="返回2")
                {
                    item.onClick.AddListener(() => UnactiveText());
                }

            }
        }
        private void UnactiveText()
        {
            foreach (var net in Date.lineNets)
            {
                foreach (var item in net.Texts)
                {
                    item.gameObject.SetActive(false);
                }
            }
        }

        private void CreatText()
        {
            int count = 1;
            foreach (var net in Date.lineNets)
            {
                foreach (var item in net.PointsInstant)
                {
                    GameObject text = Instantiate(textPrefab, item.transform.Find("Target").transform.position , Quaternion.Euler(90,0,0), gameCanvas.transform);
                    text.transform.SetParent(TextContain.transform);
                    text.GetComponent<Text>().text = count.ToString();
                    net.Texts.Add(text.GetComponent<Text>());
                }
                count++;
            }
        }
        private void DestroyText()
        {
            foreach (var net in Date.lineNets)
            {
                foreach (var item in net.Texts)
                {
                    Destroy(item.gameObject);
                }
                net.Texts.Clear();
            }
        }
        private void Update()
        {
            #region Text位置更新
            try
            {
                foreach (var net in Date.lineNets)
                {
                    if (net.PointsInstant.Count == net.Texts.Count)
                    {
                        for (int i = 0; i < net.PointsInstant.Count; i++)
                        {
                            GameObject temp = net.PointsInstant[i].transform.Find("Target").gameObject;
                            if (temp != null)
                            {
                                net.Texts[i].transform.position = temp.transform.position;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Debug.Log("出错了");
                throw;
            }
            

            #endregion

        }
    }
}
