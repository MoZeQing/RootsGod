using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardChild : MonoBehaviour
{

    public bool isCreated = false;
    public void Init()
    {
        isCreated = true;
    }

    private void Start()
    {
        transform.DOMove(new Vector3(300, 300, 0), 1);
        transform.DORotate(new Vector3(0, 360, 0), 1);
    }
}
