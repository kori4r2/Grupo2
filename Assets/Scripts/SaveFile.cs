using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class SaveFile {
    [SerializeField]
    private bool exists = false;
    public bool Exists {
        get {
            return exists;
        }
        set {
            exists = ((warriorHP > -1) || (archerHP > -1) || (mageHP > -1));
        }
    }
    public float warriorHP = -1, archerHP = -1, mageHP = -1;
    public float warriorSpecial = -1, archerSpecial = -1, mageSpecial = -1;
    public int nPotions = 0;
    public void Reset() {
        exists = false;
        warriorHP = archerHP = mageHP = warriorSpecial = archerSpecial = mageSpecial = -1;
        nPotions = 0;
    }
}
