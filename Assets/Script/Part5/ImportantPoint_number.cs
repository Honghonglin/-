using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Script.Part5
{
    public class ImportantPoint_number:MonoBehaviour
    {
        public int Floor { get; set; }
        public int Number { get; set; }
        private void Update()
        {
              if(Floor==1)
            {
                gameObject.GetComponent<Pos_Set>().enabled = true;
                if(gameObject.GetComponent<Pos_Set>()._text!=null)
                {
                    gameObject.GetComponent<Pos_Set>()._text.text = Number.ToString();
                }
            }
        }

    }
}