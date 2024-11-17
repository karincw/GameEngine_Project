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
        private Transform poolPos;

        private void Awake()
        {
            poolPos = transform.Find("PoolPos");
            foreach (ITEM_TYPE keys in poolingItem.Keys)
            {
                for (int i = 0; i < 5; i++)
                    poolResult.Add(CreatePool(keys));
            }
        }

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

        public void ReturnPool(SelectorItem _item)
        {
            _item.transform.parent = poolPos;
            _item.gameObject.SetActive(false);
            poolResult.Add(_item);
        }
        public void ReturnPool(SelectorItem[] _item)
        {
            foreach (SelectorItem item in _item)
            {
                item.transform.parent = poolPos;
                item.gameObject.SetActive(false);
                poolResult.Add(item);
            }
        }

        public SelectorItem CreatePool(ITEM_TYPE _type)
        {
            if (!poolingItem.ContainsKey(_type)) return null;

            SelectorItem item = Instantiate(poolingItem[_type], poolPos);
            item.gameObject.SetActive(false);
            return item;
        }
    }
}
