using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public abstract class BaseNodeComponent : Entity
    {
        /// <summary>
        /// �Ƿ���
        /// </summary>
        public bool IsBoom
        {
            get;
            set;
        }
        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public bool IsConnect
        {
            get;
            set;
        }
        /// <summary>
        /// ����ˢ�½ڵ������
        /// </summary>
        public virtual void SetConnect(bool flag)
        { 
            
        }

        public virtual void RemoveNode()
        { 
            
        }
    }
}