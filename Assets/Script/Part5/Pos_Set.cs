using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Script.Part5;
public class Pos_Set : MonoBehaviour
{
    public Text text;
    private Camera _WorldCamera;
    private Transform _Target;
    [HideInInspector]
    public Text _text;
    private GameObject _Panel;
    private void Start()
    {
        _WorldCamera = GameObject.Find("Part5Camera").GetComponent<Camera>();
        _Target = gameObject.transform.Find("Target");
        _Panel = GameObject.FindGameObjectWithTag("Canvas");
        _text = Instantiate(text, _Panel.transform.Find("PointText"));
        TextControl.texts.Add(_text.gameObject);
    }
    private void Update()
    {
        _text.transform.position = _Target.transform.position;
       // _text.text = gameObject.GetComponent<ImportantPoint_number>().Number.ToString();
    }

}
