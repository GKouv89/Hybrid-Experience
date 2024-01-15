using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueApp{
    [CreateAssetMenu]
    public class Character : ScriptableObject
    {
        public string charName;
        public Sprite charSprite;
        public bool isNPC = true;
    }
}