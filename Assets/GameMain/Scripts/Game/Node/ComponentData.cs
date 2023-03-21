using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class ComponentData : AccessoryObjectData
    {
        private NodeData m_nodeData;
        public ComponentData(int entityId, int typeId, int ownerId, NodeData nodeData)
    : base(entityId, typeId, ownerId)
        {
            m_nodeData = nodeData;
        }

        public NodeData NodeData
        {
            get
            {
                return m_nodeData;
            }
        }
    }
}

