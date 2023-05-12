using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public abstract class BaseNodeComponent : Entity
    {
        public bool IsConnect { get; set; } = false;
        public BoomArea BoomArea { get; set; }

        public virtual void RemoveNode()
        { 
            
        }

        public virtual void SetConnect(bool flag)
        { 
            
        }
    }
}