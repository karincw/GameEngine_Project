using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Karin
{

    [Flags]
    public enum BaseShapeType : int
    {
        Black = 0,
        Pink,
        Diamond,
        Heart,
        Clover,
        Spade,
    }

    [Flags]
    public enum SpecialShapeType
    {
        Diamond = 0,
        Heart,
        Clover,
        Spade,
        Sword,
        Shield,
    }

    public enum CountType
    {
        ACE = 0,
        One,
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

}