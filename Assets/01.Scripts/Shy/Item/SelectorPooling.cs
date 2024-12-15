using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace Shy
{
    public class SelectorPooling : MonoSingleton<SelectorPooling>
    {
        [SerializeField] private SerializedDictionary<ITEM_TYPE, SelectorItem> poolingItem;
        [SerializeField] private List<SelectorItem> poolResult;

        private void Awake()
        {
            foreach (ITEM_TYPE keys in poolingItem.Keys)
            {
                for (int i = 0; i < 5; i++)
                    poolResult.Add(CreatePool(keys));
            }
        }

        #region Get
        public SelectorItem GetPool(ITEM_TYPE _type)
        {
            for (int i = 0; i < poolResult.Count; i++)
            {
                if(poolResult[i].iType == _type)
                {
                    SelectorItem item = poolResult[i];
                    poolResult.Remove(item);
                    return item;
                }
            }

            return CreatePool(_type);
        }
        #endregion

        #region Return
        public void ReturnPool(SelectorItem _item)
        {
            _item.transform.parent = transform;
            _item.gameObject.SetActive(false);
            poolResult.Add(_item);
        }

        public void ReturnPool(SelectorItem[] _item)
        {
            if (_item == null) return;

            foreach (SelectorItem item in _item)
            {
                item.transform.parent = transform;
                item.gameObject.SetActive(false);
                poolResult.Add(item);
            }
        }
        #endregion

        #region Create
        public SelectorItem CreatePool(ITEM_TYPE _type)
        {
            if (!poolingItem.ContainsKey(_type)) return null;

            SelectorItem item = Instantiate(poolingItem[_type], transform);
            item.gameObject.SetActive(false);
            return item;
        }
        #endregion
    }
}
