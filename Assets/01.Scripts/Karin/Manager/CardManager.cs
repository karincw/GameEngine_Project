using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

namespace Karin
{

    public class CardManager : MonoSingleton<CardManager>
    {
        [SerializedDictionary("ShapeType", "TargetSprite")]
        public SerializedDictionary<SpecialShapeType, Sprite> ShapeToSpriteDictionary = new();
        [SerializedDictionary("ShapeType", "TargetColor")]
        public SerializedDictionary<SpecialShapeType, Color> ShapeToColorDictionary = new();

        [SerializeField] private AudioClip _swordClip;
        [SerializeField] private AudioClip _shieldClip;
        [SerializeField] private AudioClip _diceClip;

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
        public void ApplyCardEffect(CardDataSO card)
        {
            if (card.specialShape <= SpecialShapeType.Spade) return;

            switch (card.specialShape)
            {
                case SpecialShapeType.Shield:
                    TurnManager.Instance.Defence(-1);
                    SoundManager.Instance.PlayEffect(_shieldClip);
                    return;
                case SpecialShapeType.Sword2:
                    TurnManager.Instance.Attack(2);
                    SoundManager.Instance.PlayEffect(_swordClip);
                    return;
                case SpecialShapeType.Sword3:
                    TurnManager.Instance.Attack(3);
                    SoundManager.Instance.PlayEffect(_swordClip);
                    return;
                case SpecialShapeType.Sword5:
                    TurnManager.Instance.Attack(5);
                    SoundManager.Instance.PlayEffect(_swordClip);
                    return;
                case SpecialShapeType.Sword7:
                    TurnManager.Instance.Attack(7);
                    SoundManager.Instance.PlayEffect(_swordClip);
                    return;
                case SpecialShapeType.Give2:
                    TurnManager.Instance.GiveCard(2);
                    return;
                case SpecialShapeType.Give3:
                    TurnManager.Instance.GiveCard(3);
                    return;
                case SpecialShapeType.Give5:
                    TurnManager.Instance.GiveCard(5);
                    return;
                case SpecialShapeType.Give7:
                    TurnManager.Instance.GiveCard(7);
                    return;
                case SpecialShapeType.King:
                    return;
                case SpecialShapeType.Lens:
                    TurnManager.Instance.Lens(2);
                    return;
                case SpecialShapeType.Reflect:
                    TurnManager.Instance.Reflect();
                    return;
                case SpecialShapeType.Dice:
                    TurnManager.Instance.Dice();
                    SoundManager.Instance.PlayEffect(_diceClip);
                    return;
                case SpecialShapeType.ChangeShape:
                    return;
            }

        }

    }

}