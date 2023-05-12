using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

namespace GameMain
{
    public class BoomAreaDara : EntityData
    {
        private float m_boomTime;
        private int m_ownerId;
        public BoomAreaDara(int entityId, int typeId,float boomTime,int ownerId)
    : base(entityId, typeId)
        {
            m_boomTime = boomTime;
            m_ownerId = ownerId;
        }

        public int OwnerId
        {
            get
            {
                return m_ownerId;
            }
        }
        public float BoomTime
        {
            get
            {
                return m_boomTime;
            }
        }
    }
}
