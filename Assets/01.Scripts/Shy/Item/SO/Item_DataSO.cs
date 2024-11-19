using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shy
{
    public abstract class Item_DataSO : ScriptableObject
    {
        public ITEM_TYPE iType;
        public string itemName;
        [TextArea] public string itemExplain;
    }
}
