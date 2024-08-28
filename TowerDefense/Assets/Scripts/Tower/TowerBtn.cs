using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBtn : MonoBehaviour
{
    [SerializeField] private GameObject TowerObject;
    [SerializeField] private Sprite TSprite;
    [SerializeField] private int towerPrice;

    public GameObject towerObject{
        get{
            return TowerObject;
        }
    }

    public Sprite Dragsprite{
        get{
            return TSprite;
        }
    }

    public int TowerPrice{
        get{
            return towerPrice;
        }
    }
}