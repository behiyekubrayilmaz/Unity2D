using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    delegate void TurnDelegate();
    TurnDelegate turnDelegate;


    public float moveSpeed = 2;
    bool lookingRight = true;
    GameManager gameManager;
    Animator anim;
    public Transform rayOrigin;
    public ParticleSystem effect;

    public Text scoreTxt, hScoreTxt;
    public int Score { get; private set; }
    public int HScore { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        #region PLATFORM FOR TURNING
            #if UNITY_EDITOR
                 turnDelegate = TurnPlayerUsingKeyboard;
            #endif
            #if UNITY_ANDROID
                 turnDelegate = TurnPlayerUsingTouch;
             #endif
        #endregion



        gameManager = GameObject.FindObjectOfType<GameManager>();
        anim = gameObject.GetComponent<Animator>();
        LoadHighScore();
    }

    private void LoadHighScore()
    {
        HScore = PlayerPrefs.GetInt("hiscore"); //daha önceki score alınıyor varsa
        hScoreTxt.text = HScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.gameStarted) return;

        anim.SetTrigger("gameStarted");

        moveSpeed *= 1.0001f;  //hız arttırma
        Debug.Log(moveSpeed);
        //transform.position += transform.forward*Time.deltaTime*moveSpeed;  // aynı işlemi yapıyor alt satırla
        transform.Translate(new Vector3(0, 0, 1) * moveSpeed * Time.deltaTime);  //bunun farkı yönü manuel olarak giriyoruz

        turnDelegate();

        CheckFalling();
    }

    private void TurnPlayerUsingKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Turn();
    }

    private void TurnPlayerUsingTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) 
            Turn();
    }

    float elapsedTime = 0; //CheckFalling fonk. herzaman çağırmamak için
    float freq = 1f / 5f;


    private void CheckFalling()
    {
        if ((elapsedTime += Time.deltaTime) > freq)
        {
            if (!Physics.Raycast(rayOrigin.position, new Vector3(0, -1, 0))) //ışının yönü belirleniyor player için
            {
                anim.SetTrigger("falling"); //düşme animasyonu ile değiştirildi
                gameManager.RestartGame();  //yeniden başlama
                elapsedTime = 0; //sn de 5 kere method çağrılır
            }
        }

    }

    private void Turn()
    {
        if (lookingRight)
        {
            transform.Rotate(new Vector3(0, 1, 0), -90);
        }
        else
        {
            transform.Rotate(new Vector3(0, 1, 0), 90);
        }
        lookingRight =! lookingRight;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("crystal"))   //Crystal in tag ini eşitliyoruz
        {
            MakeScore();   //Score yap
            CreateEffect(); //elması alınca çıkan effect
            Destroy(other.gameObject);  //crystal yok eder
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        Destroy(collision.gameObject, 2f); // 2sn sonra kamera dışındaki block ları yok etme
    }

    private void CreateEffect()
    {
        var vfx = Instantiate(effect, transform);
        Destroy(vfx, 1f);
    }

    private void MakeScore()
    {
        Score++;
        scoreTxt.text = Score.ToString();   //Score ekrana yazma
        if (Score > HScore)
        {
            HScore = Score;
            hScoreTxt.text = HScore.ToString();
            PlayerPrefs.SetInt("hiscore",HScore); //Load fonksiyondaki isim alındı kaydedildi score bu satırda
        }
    }
}
