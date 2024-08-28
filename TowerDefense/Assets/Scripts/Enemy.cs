using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    [SerializeField] private int target=0;
    [SerializeField] private Transform ExitPoint;
    [SerializeField] private Transform[] WayPoints;
    [SerializeField] private float NavigationUpdate;
    [SerializeField] private float health;
    [SerializeField] private int rewardAmt;
 
    private float NavigationTime=0f;
    private Transform enemy;
    private Collider2D enemyCollider;
    private Animator Anim;
    private bool isDead = false;

    public bool IsDead{
        get{
            return isDead;
        }
    }
    public float Health{
        get{
            return health;
        }
    }
    public int RewardAmt{
        get{
            return rewardAmt;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        enemy=GetComponent<Transform>();
        enemyCollider = GetComponent<Collider2D>();
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(WayPoints != null && !isDead){
            NavigationTime += Time.deltaTime;                                                                                  //Time is being added to this at each frame
            if (NavigationTime > NavigationUpdate){
                if(WayPoints.Length > target){
                    enemy.position = Vector2.MoveTowards(enemy.position,WayPoints[target].position,NavigationTime);            //3 Parameters for this function(current position,next position,time taken)
                }
                else{
                    enemy.position = Vector2.MoveTowards(enemy.position,ExitPoint.position,NavigationTime);
                }
                NavigationTime=0;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Checkpoint"){
            target += 1;
        }
        else if(other.tag == "Finish"){
            GameManager.Instance.RoundEscaped += 1;
            GameManager.Instance.TotalEscaped += 1;
            GameManager.Instance.UnRegisterEnemy(this);
            GameManager.Instance.isWaveOver();
        }
        else if(other.tag == "Projectile"){
            Projectiles newP = other.gameObject.GetComponent<Projectiles>();
            enemyHit(newP.AttackStrength);
            Destroy(other.gameObject);
        }
    }

    public float enemyHit(float Damage){
        if((health-Damage)>0){
            Anim.Play("Hurt");
            health -= Damage;
        }
        else{
            Anim.SetTrigger("Died");
            die();
        }
        return health;
    }

    public void die(){
        isDead=true;
        GameManager.Instance.TotalKilled += 1;
        GameManager.Instance.AddMoney(rewardAmt);
        GameManager.Instance.UnRegisterEnemy(this);
        GameManager.Instance.isWaveOver();
    }
}
