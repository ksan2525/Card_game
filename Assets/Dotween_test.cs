using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dotween_test : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject card;
    void Start()
    {
        transform.DOLocalMove(new Vector3(0, -420, 10f), 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
