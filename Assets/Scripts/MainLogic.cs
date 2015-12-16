using UnityEngine;
using System.Collections;

public class MainLogic : MonoBehaviour {


    // Use this for initialization
    void Start() {
        UnityEngine.Object pPrefab = Resources.Load("Prefabs/Platform04x01");
        float currentX = 0f;
        for (int i = 0; i < 100; i++)
        {
            currentX += Random.Range(5f, 25f);
            Vector3 blockPos = new Vector3(currentX, Random.Range(-3f, 3f), 0f);
            GameObject pNewObject = (GameObject)GameObject.Instantiate(pPrefab, blockPos, Quaternion.identity);
        }
        Destroy(pPrefab);
        
        GameObject platform = new GameObject("test");
        platform.transform.position = new Vector3(0, -4, 0);
        platform.transform.localScale = new Vector3(1000, 1, 1);
        BoxCollider2D col = platform.AddComponent<BoxCollider2D>();
        col.size = new Vector2(1, 1);
        col.offset = new Vector2(0, 0);
        col.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Slippery");
        SpriteRenderer sr = platform.AddComponent<SpriteRenderer>();
        sr.sprite = Sprite.Create(Resources.Load<Texture2D>("PlatformWhiteSprite"), new Rect(0, 0, 100, 100), new Vector2(0.5f, 0.5f));
        //sr.sprite.texture.SetPixels(0, 0, 100, 1, new Color[] { Color.white });
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
