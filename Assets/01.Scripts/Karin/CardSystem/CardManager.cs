using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Karin
{

    public class CardManager : MonoSingleton<CardManager>
    {
        [SerializedDictionary("ShapeType","TargetSprite")]
        public SerializedDictionary<SpecialShapeType, Sprite> ShapeToSpriteDictionary = new();
        [SerializedDictionary("ShapeType","TargetColor")]
        public SerializedDictionary<SpecialShapeType, Color> ShapeToColorDictionary = new();

        public TMP_FontAsset BlackFont, PinkFont;

        /// <summary>
        /// 0. base
        /// 1. blue
        /// 2. red
        /// 3. golden
        /// 4. background
        /// </summary>
        public Sprite[] cards;

        public string GetCountText(CountType ct)
        {
            switch (ct)
            {
                case CountType.ACE:
                    return "A";
                case CountType.One:
                case CountType.Two:
                case CountType.Three:
                case CountType.Four:
                case CountType.Five:
                case CountType.Six:
                case CountType.Seven:
                case CountType.Eight:
                case CountType.Nine:
                case CountType.Ten:
                    return ((int)ct).ToString();
                case CountType.Jack:
                    return "J";
                case CountType.Queen:
                    return "Q";
                case CountType.King:
                    return "K";
            }
            Debug.LogError("GetCountText Error");
            return null;
        }

    }

}