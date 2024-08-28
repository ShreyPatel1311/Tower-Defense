using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStatus{
    next,play,GameOver,Win
}

public class GameManager : singleton<GameManager>
{
    [SerializeField] private int TotalWaves = 10;
    [SerializeField] private Text TotalMoneyLabel;
    [SerializeField] private Text CurrentWaveLbl;
    [SerializeField] private Text TotalEscapedLbl;
    [SerializeField] private Image StartLabel;
    [SerializeField] private GameObject SpawnPoint;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int TotalEnemies;
    [SerializeField] private int EnemiesPerSpawn;
    [SerializeField] private Text PlayBtnLabel;
    [SerializeField] private Button PlayBtn;

    private int WaveNumber = 0;
    private int totalMoney = 10;
    private int totalEscaped = 0;
    private int roundEscaped = 0;
    private int totalKilled = 0;
    private int whichEnemiesToSpawn = 0;
    private int count = 0;
    private GameStatus currentStatus = GameStatus.play;
    
    public List<Enemy> EnemyList = new List<Enemy>();

    public int TotalEscaped{
        get{
            return totalEscaped;
        }
        set{
            totalEscaped = value;
        }
    }
    public int RoundEscaped{
        get{
            return roundEscaped;
        }
        set{
            roundEscaped = value;
        }
    }
    public int TotalKilled{
        get{
            return totalKilled;
        }
        set{
            totalKilled = value;
        }
    }
    public int TotalMoney{
        get{
            return totalMoney;
        }
        set{
            totalMoney = value;
        }
    }

    const float SpawnDelay = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        PlayBtn.gameObject.SetActive(false);
        showMenu();
    }

    void Update(){
        HandleEscape();
    }

    IEnumerator SpawnEnemy()
    {
        if(EnemiesPerSpawn > 0 && TotalEnemies >= EnemyList.Count){
            for(int i=0;i<EnemiesPerSpawn;i++){
                if(count <= TotalEnemies){
                    GameObject newEnemy = Instantiate(enemies[Random.RandomRange(0, 2)]);                                                         //Instantiate method creates a game object in Untiy.
                    newEnemy.transform.position = SpawnPoint.transform.position;                                           //new Enemy's position is given here.
                    RegisterEnemy(newEnemy.GetComponent<Enemy>());
                    count++;
                }
            }
            yield return new WaitForSeconds(SpawnDelay);                                                                   //this will stacking of enemy object one over the other.
            StartCoroutine(SpawnEnemy());
        }
    }

    public void RegisterEnemy(Enemy enemy){
        EnemyList.Add(enemy);                                                                                              //Adds enemy to list of existing enemies.
    }

    public void UnRegisterEnemy(Enemy enemy){
        EnemyList.Remove(enemy);                                                                                           //Removes Enemy from list and existing game screen.
        Destroy(enemy.gameObject);
    }

    public void DestroyAllEnemies(){
        foreach(Enemy enemy in EnemyList){
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }

    public int AddMoney(int amount){
        totalMoney += amount;
        TotalMoneyLabel.text = totalMoney.ToString();
        return totalMoney;
    }

    public int SubtractMoney(int amount){
        totalMoney -= amount;
        TotalMoneyLabel.text = totalMoney.ToString();
        return totalMoney;
    }

    public void isWaveOver(){
        TotalEscapedLbl.text="Escaped " + TotalEscaped + "/10";
        Debug.Log(TotalKilled + RoundEscaped);
        if ((RoundEscaped + TotalKilled) == TotalEnemies){
            setCurrentGameState();
            showMenu();
        }
    }

    public void setCurrentGameState(){
        if(TotalEscaped >= 10){
            currentStatus = GameStatus.GameOver;
        }
        else if(WaveNumber == 0 && (TotalKilled + RoundEscaped) != TotalEnemies){
            currentStatus = GameStatus.play;
        }
        else if(WaveNumber >= TotalWaves){
            currentStatus = GameStatus.Win;
        }
        else{
            currentStatus = GameStatus.next;
        }
    }

    public void showMenu(){
        switch(currentStatus){
            case GameStatus.GameOver:
                PlayBtnLabel.text="Game Over";
                break;
            case GameStatus.play:
                PlayBtnLabel.text="Play";
                break;
            case GameStatus.Win:
                PlayBtnLabel.text="You Won!!!!";
                break;
            case GameStatus.next:
                PlayBtnLabel.text="Next Wave";
                break;
        }
        PlayBtn.gameObject.SetActive(true);
    }

    public void playBtnPressed(){
        switch(currentStatus){
            case GameStatus.next:
                WaveNumber += 1;
                TotalEnemies += WaveNumber;
                currentStatus = GameStatus.play;
                count = 0;
                break;
            case GameStatus.play:
                StartCoroutine(SpawnEnemy());
                break;
            default:
                TotalEnemies = 3;
                TotalEscaped = 0;
                totalMoney = 10;
                TotalMoneyLabel.text = totalMoney.ToString();
                TotalEscapedLbl.text = "Escaped " + TotalEscaped + "/10";
                break;
        }
        DestroyAllEnemies();
        TotalKilled = 0;
        RoundEscaped = 0;
        CurrentWaveLbl.text = "Wave "+ (WaveNumber + 1);
        PlayBtn.gameObject.SetActive(false);
    }

    public void HandleEscape(){
        Debug.Log(currentStatus);
        if(Input.GetMouseButton(1)){
            TowerManager.Instance.disableSpriteRenderer();
            TowerManager.Instance.towerBtnPressed = null;
        }
    }
}