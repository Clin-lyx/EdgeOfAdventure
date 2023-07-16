using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scrolling : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private RawImage _img;
    [SerializeField] private float _x;
    private bool moveLeft = true;


    // Update is called once per frame
    void Update()
    {
        if (!moveLeft) {
            _img.uvRect = new Rect(_img.uvRect.position - new Vector2(_x, 0f) * Time.deltaTime, _img.uvRect.size);
            if (_img.uvRect.x <= -0.03f) {
                moveLeft = true;
            }
        } else {
            _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, 0f) * Time.deltaTime, _img.uvRect.size);
            if (_img.uvRect.x >= 0.03f) {
                moveLeft = false;
            }
        }
    }
}
