using System;
using UnityEngine;

namespace GameMain
{
    [Serializable]
    public class LineData : EntityData
    {
        public Transform Self
        {
            get;
            set;
        }

        public Transform Target
        {
            get;
            set;
        }

        public LineData(int entityId, int typeId, Transform self)
            : base(entityId, typeId)
        {
            Self = self;
        }
    }
}
