
using UnityEngine;

public class WallMaker : MonoBehaviour
{
    public Transform lastWall;
    public GameObject wallPrefab;
    Vector3 lastPos;
    PlayerController player;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        lastPos = lastWall.position;  //son bloğun konumu
        InvokeRepeating("CreateWalls", 1, 0.01f);  // 1sn de 2 tane oluşması için clone lama fonksiyonu
        cam = Camera.main;
        player = FindObjectOfType<PlayerController>();
    }

    private void CreateWalls()
    {
        //gereksiz block oluşumu engellendi 
        float distance = Vector3.Distance(lastPos, player.transform.position); //son block ile oyuncu arasındaki fark
        if (distance > cam.orthographicSize * 2) return; // kamera boyutunun yarısını verir bu yüzden 2 ile çarpılır
        

        Vector3 newPos = Vector3.zero;
        int rand = Random.Range(0, 11);  // rastgele sayi üretimi

        if (rand <= 5)
            newPos = new Vector3(lastPos.x - 0.707f, lastPos.y, lastPos.z + 0.707f);   //sağa doğu konumlandırma
        
        else
            newPos = new Vector3(lastPos.x + 0.70711f, lastPos.y, lastPos.z + 0.70711f);   //önüne konumlandırma

        GameObject newBlock = Instantiate(wallPrefab, newPos, Quaternion.Euler(0, 45, 0), transform);  //yeni block oluşumu
        newBlock.transform.GetChild(0).gameObject.SetActive(rand % 3 == 2);  //elmas oluşturma
        lastPos = newBlock.transform.position; //güncelleme yapılıyor
    }
}
