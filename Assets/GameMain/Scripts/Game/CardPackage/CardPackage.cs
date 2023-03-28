using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class CardPackage : MonoBehaviour
{

    public List<GameObject> CardChildList;

    public GameObject cardChild;

    private void Start()
    {
        for (int i = 0; i < CardChildList.Count; i++)
        {
            //CardChildList[i].Init();
        }
    }

    public void CreateCard()
    {
        Transform CardTran = Instantiate(cardChild).transform;
    }

    private IEnumerator CardChildInitAnin()
    {

        yield return new WaitForSeconds(1);
        
    }
}
