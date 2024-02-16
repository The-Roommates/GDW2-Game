using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuShader : MonoBehaviour
{
    [SerializeField] private RawImage _img;
    [SerializeField] private float _x, _y;

    //Curson Definition
    public Image _cursor;

    public int _selection;

    //1 = Demo
    //2 = Start
    //3 = Options
    //4 = Exit

    void Start()
    {
        _selection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _img.uvRect.size);


       

    }
}
