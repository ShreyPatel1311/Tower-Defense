using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStatus{
    next,play,GameOver,Win
}

public class GameManager : singleton<GameManager>
{
    [@SerializeField] private int TotalWaves = 10;
    [@SerializeField] private Text totalMoneyLbl;
    [@SerializeField] private Text CurrentWaveLbl;
    [@SerializeField] private Text TotalEscapedLbl;
    [@SerializeField] private Image StartLabel;
    [@SerializeField] private GameObject SpawnPoint;
    [@SerializeField] private Enemy[] enemies;
    [@SerializeField] private int TotalEnemies = 3;
    [@SerializeField] private int MaxEnemiesOnScreen = 3;
    [@SerializeField] private int EnemiesPerSpawn;
    [@SerializeField] private Text PlayBtnLabel;
    [@SerializeField] private Button PlayBtn;

    private int WaveNumber = 0;
    private int totalMoney = 10;
    private int totalEscaped = 0;
    private int roundEscaped = 0;
    private int totalKilled = 0;
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
            totalMoneyLbl.text = totalMoney.ToString();
        }
    }

    const float SpawnDelay = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        PlayBtn.gameObject.SetActive(false);
        showMenu();
    }

    void Update()
    {
        HandleEscape();
    }

    public int RandomInt(int min, int max) 
    {
        return Random.Range(min, max);
    }

    IEnumerator spawn() {
		if (EnemiesPerSpawn > 0 && EnemyList.Count < TotalEnemies) {
			for(int i = 0; i < MaxEnemiesOnScreen; i++) {
				if(EnemyList.Count < MaxEnemiesOnScreen) {

                    Enemy newEnemy = Instantiate(enemies[RandomInt(0, 3)]);
					newEnemy.transform.position = SpawnPoint.transform.position;
				}
			}
			yield return new WaitForSeconds(SpawnDelay);
            StartCoroutine(spawn());
		}
	}

    public void RegisterEnemy(Enemy enemy){
        EnemyList.Add(enemy);                                                                                              //Adds enemy to list of existing enemies.
    }

    public void UnRegisterEnemy(Enemy enemy){
        EnemyList.Remove(enemy);                                                                                           //Removes Enemy from list and existing game screen.
        Destroy(enemy.gameObject);
        Debug.Log("Enemy Destroyed");
    }

    public void DestroyAllEnemies(){
        foreach(Enemy enemy in EnemyList){
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }

    public void AddMoney(int amount){
        TotalMoney += amount;
        totalMoneyLbl.text = TotalMoney.ToString();
    }

    public void SubtractMoney(int amount){
        TotalMoney -= amount;
    }

    public void isWaveOver(){
        TotalEscapedLbl.text = "Escaped   " + TotalEscaped + "/10";
        if((RoundEscaped +TotalKilled) == TotalEnemies){
            setCurrentGameState();
            showMenu();
        }
    }

    public void setCurrentGameState(){
        if(totalEscaped >= 10){
            currentStatus = GameStatus.GameOver;
        }
        else if(WaveNumber == 0 && (TotalKilled + RoundEscaped) == 0){
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
                break;
            default:
                TotalEnemies=8;
                TotalEscaped = 0;
                TotalMoney = 10;
                totalMoneyLbl.text = TotalMoney.ToString();
                TotalEscapedLbl.text = "Escaped   " + TotalEscaped + "/10";
                break;
        }
        DestroyAllEnemies();
        TotalKilled = 0;
        RoundEscaped = 0;
        CurrentWaveLbl.text = "Wave " + (WaveNumber + 1);
        StartCoroutine(spawn());
        PlayBtn.gameObject.SetActive(false);
    }

    public void HandleEscape(){
        if(Input.GetMouseButton(1)){
            TowerManager.Instance.disableSpriteRenderer();
            TowerManager.Instance.towerBtnPressed = null;
        }
    }
}