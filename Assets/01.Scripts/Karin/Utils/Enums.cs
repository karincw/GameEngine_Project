using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karin
{

    #region Card
    public enum BaseShapeType : int
    {
        Diamond = 0,
        Heart,
        Clover,
        Spade,
    }
    public enum SpecialShapeType : int
    {
        Diamond = 0,
        Heart,
        Clover,
        Spade,
        Sword2,
        Sword3,
        Sword5,
        Sword7,
        Shield,
        Reflect,
        Give2,
        Give3,
        Give5,
        Give7,
        King,
        Dice,
        Lens,
        ChangeShape,
    }
    public enum CountType : int
    {
        ACE = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
    }
    public enum CardType : int
    {
        Basic = 0,
        Blue,
        Red,
        Gold
    }
    #endregion

    public enum Turn
    {
        Player,
        Enemy
    }

}