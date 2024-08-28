using UnityEngine.EventSystems;
using UnityEngine;

public class TowerManager : singleton<TowerManager>
{
    public TowerBtn towerBtnPressed{get; set;}
    private SpriteRenderer spriteRender;

    // Start is called before the first frame update
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)){
            Vector2 WorldPoint=Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(WorldPoint, Vector2.zero);
            if(hit.collider.tag == "BuildSites"){
                hit.collider.tag="BuildSiteFull";
                PlaceTower(hit);
            }
        }
        if(spriteRender.enabled){
            followMouse();
        } 
    }

    public void PlaceTower(RaycastHit2D hit){
        if(!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null){
            GameObject newTower = Instantiate(towerBtnPressed.towerObject);
            newTower.transform.position=hit.transform.position;
            disableSpriteRenderer();
        }
    }

    public void SelectedTower(TowerBtn TowerSelected){
        towerBtnPressed=TowerSelected;
        enableSpriteRenderer(towerBtnPressed.Dragsprite);
    }

    public void followMouse(){
        transform.position=Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public void enableSpriteRenderer(Sprite Dsprite){
        spriteRender.enabled=true;
        spriteRender.sprite=Dsprite;
    }

    public void disableSpriteRenderer(){
        spriteRender.enabled=false;
    }
}