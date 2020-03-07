using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

namespace Assets.Script.Part5
{
    class TextControl:MonoBehaviour
    {
        public static List<GameObject> texts = new List<GameObject>();
        public  void DestroyTexts()
        {
            if(texts.Count!=0)
            {
                foreach (var item in texts)
                {
                    Destroy(item);
                }
                texts.Clear();
            }
        }
    }
}
