using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class CreepCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.gameObject.layer == LayerMask.GetMask(Line))
        // {
        //     return;
        // }
        if(other.gameObject == transform.parent.gameObject)
            return;
        var targetPosition = other.transform.position;
        GameObject.Destroy(other.gameObject);
        ShowNode(targetPosition);
        Debug.Log(1);
    }

    private void ShowNode(Vector3 position)
    {
        var randomNum = GameEntry.Utils.rand(GameEntry.Utils.creepPoolDic,100);
        var entity = Instantiate(GameEntry.Utils.nodes[randomNum], position,
                Quaternion.Euler(0, 0, 0));
    }
}

}

