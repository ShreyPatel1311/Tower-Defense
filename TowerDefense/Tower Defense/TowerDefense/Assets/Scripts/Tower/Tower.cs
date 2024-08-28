using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [@SerializeField] private float AttackRadius;
    [@SerializeField] private float timeBetweenAttacks;
    [@SerializeField] private Projectiles Weapon;
    [@SerializeField] private float LeastAttackDistance = 0.2f;

    private Enemy targetEnemy = null;
    private float attackCounter = 0f;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackCounter -= Time.deltaTime;
        if(targetEnemy == null || targetEnemy.IsDead){
            Enemy nearestEnemy = NearestEnemiesInRange();
            if(nearestEnemy != null &&Vector2.Distance(transform.position, nearestEnemy.transform.position) <= AttackRadius){
                targetEnemy = nearestEnemy;
            }
        }
        else{
            if(attackCounter<=0){
                isAttacking=true;
                attackCounter = timeBetweenAttacks;
            }
            else{
                isAttacking=false;
            }
            if(Vector2.Distance(transform.position, targetEnemy.transform.position)>AttackRadius){
                targetEnemy = null;
            }    
        }
    }

    void FixedUpdate(){
        if(isAttacking == true){
            Attack();
        }
    }

    public void Attack(){
        Projectiles LoadedWeapon = Instantiate(Weapon);
        LoadedWeapon.transform.localPosition = transform.localPosition;

        if(targetEnemy == null){
            Destroy(LoadedWeapon);
        }
        else{
            StartCoroutine(LaunchWeapon(LoadedWeapon));
        }
    }

    IEnumerator LaunchWeapon(Projectiles WeaponLoaded){
        while (TargetDistance(targetEnemy) > LeastAttackDistance && targetEnemy != null && WeaponLoaded != null)
        {
            var dir = targetEnemy.transform.localPosition - transform.localPosition;
            var angleDirection = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
            WeaponLoaded.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
            WeaponLoaded.transform.localPosition = Vector2.MoveTowards(WeaponLoaded.transform.localPosition, targetEnemy.transform.localPosition, 5f*Time.deltaTime);
            yield return null;
        }
        if(WeaponLoaded != null || targetEnemy == null){
            Destroy(WeaponLoaded.gameObject);
        }
    }

    public float TargetDistance(Enemy targetEnemy){
        if(targetEnemy == null){
            targetEnemy=NearestEnemiesInRange();
            if(targetEnemy == null){
                return 0f;
            }
        }
        return Vector2.Distance(targetEnemy.transform.position, transform.position);  
    }

    public List<Enemy> GetAllEnemiesInNearestRange(){
        List<Enemy> EnemiesInRange = new List<Enemy>();
        foreach(Enemy enemy in GameManager.Instance.EnemyList){
            if(Vector2.Distance(transform.position, enemy.transform.position)<=AttackRadius){
                EnemiesInRange.Add(enemy);
            }
        }
        return EnemiesInRange;
    }

    private Enemy NearestEnemiesInRange(){
        Enemy nearestEnemy=null;
        float smallestDistance=float.PositiveInfinity;
        foreach(Enemy enemy in GetAllEnemiesInNearestRange()){
            if(Vector2.Distance(transform.position, enemy.transform.position)<smallestDistance){
                smallestDistance = Vector2.Distance(transform.position, enemy.transform.position);
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }

}
