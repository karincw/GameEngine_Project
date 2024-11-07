using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Shy
{
    public class NameCard : MonoSingleton<NameCard>
    {
        public CharacterBaseSO data;
        [SerializeField] private TextMeshProUGUI namePos;
        [SerializeField] private TextMeshProUGUI lifePos;

        [SerializeField] private int Health;
        public int health
        {
            get => Health;
            set { Health = value; lifePos.text = Health.ToString(); }
        }

       public void Init(CharacterBaseSO _base)
       {
            data = _base;

            health = data.life;
            namePos.text = data.cName;

            gameObject.SetActive(true);
       }
    }
}
