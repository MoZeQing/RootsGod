using System;
using UnityEngine;

namespace GameMain
{
    [Serializable]
    public class CardPackageData : EntityData
    {
        public CardPackageType CardPackageType
        {
            get;
            set;
        }

        public CardPackageData(int entityId, int typeId,CardPackageType cardPackageType)
            : base(entityId, typeId)
        {
            CardPackageType = cardPackageType;
        }
    }
}
