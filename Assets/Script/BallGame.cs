using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BallGame : MonoBehaviour
{
    
    float smooth = 2.0f;
    float tiltAngle = 30.0f;
    
    public Text ScoreText,LifeText;

    int score, life;
    Vector3 spawnPos; //공의 스폰 위치
    GameObject ball; //공 오브젝트
    
    GameObject GameOverBlock; //게임오버 텍스트,버튼
    GameObject GameClearBlock; //게임클리어 텍스트,버튼

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        //점수, 생명 초기화
        score = 0;
        life = 3;

        //공 오브젝트 위치 기록
        ball = GameObject.Find("Ball");
        GameOverBlock = GameObject.Find("GameOverText");
        GameOverBlock.SetActive(false);

        GameClearBlock = GameObject.Find("GameClearText");
        GameClearBlock.SetActive(false);

        spawnPos =  ball.transform.position;

       
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            //게임오버 
            Time.timeScale = 0;
            GameOverBlock.SetActive(true);
        }

        if(score >= 15)
        {
            //게임 클리어
            Time.timeScale = 0;
            GameClearBlock.SetActive(true);

        }

        float halfW = Screen.width / 2,halfH = Screen.height / 2;

        this.transform.position = new Vector3((Input.mousePosition.x-halfW)/halfW, (Input.mousePosition.y-halfH)/halfH, this.transform.position.z);

        //Smoothly tilts a transform
        float tiltAroundZ = Input.GetAxis("Mouse X") * tiltAngle*2;
        float tiltAroundX = Input.GetAxis("Mouse Y") * tiltAngle*-2;
        Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);

        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);

        ScoreText.text = "Score : " + score;    
        LifeText.text = "Life : " + life;

        //화면 아래으로 나가면
        if( Camera.main.WorldToViewportPoint(ball.transform.position).y < -1)
        {
            //리스폰
            ball.transform.position = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z); //리스폰
            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            //점수 깎기
            life -= 1;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        score += 1;
    }

    public void GameReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
     }
}
