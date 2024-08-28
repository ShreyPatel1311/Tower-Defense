using UnityEngine;

public enum proType{
    rock,arrow,fireball
};

public class Projectiles : singleton<Projectiles>
{
    [@SerializeField] private int attackStrength;
    [@SerializeField] private proType ProjectileType;
    
    public int AttackStrength{
        get{
            return attackStrength;
        }
    }
    public proType projectiles{
        get{
            return ProjectileType;
        }
    }
}
