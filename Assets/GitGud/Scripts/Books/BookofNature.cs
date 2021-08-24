using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookofNature : Book
{
    public override int BonusPoints() {
        Transform parent = transform.parent;
        if (!parent) {
            Debug.LogError("Book has no parent in BONUS POINTS");
        } else {

        }
        
        
        return bonusPoints;
    }
}
