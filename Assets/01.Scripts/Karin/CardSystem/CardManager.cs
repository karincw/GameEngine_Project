using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karin
{

    public class CardManager : MonoSingleton<CardManager>
    {
        [SerializedDictionary("ShapeType","Sprite")]
        public SerializedDictionary<SpecialShapeType, Sprite> ShapeToSpriteDictionary = new();

        private void Awake()
        {

        }
    }

}