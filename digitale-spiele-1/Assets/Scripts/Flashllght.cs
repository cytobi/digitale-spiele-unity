using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashllght : MonoBehaviour
{
    [SerializeField] GameObject FlashlightLight;
    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        FlashlightLight.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("f")) {
            active = !active;
            FlashlightLight.gameObject.SetActive(active);
        }
    }
}
