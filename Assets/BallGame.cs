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
    Vector3 spawnPos; //���� ���� ��ġ
    GameObject ball; //�� ������Ʈ
    
    GameObject GameOverBlock; //���ӿ��� �ؽ�Ʈ,��ư
    GameObject GameClearBlock; //����Ŭ���� �ؽ�Ʈ,��ư

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        //����, ���� �ʱ�ȭ
        score = 0;
        life = 3;

        //�� ������Ʈ ��ġ ���
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
            //���ӿ��� 
            Time.timeScale = 0;
            GameOverBlock.SetActive(true);
        }

        if(score >= 15)
        {
            //���� Ŭ����
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

        //ȭ�� �Ʒ����� ������
        if( Camera.main.WorldToViewportPoint(ball.transform.position).y < -1)
        {
            //������
            ball.transform.position = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z); //������
            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            //���� ���
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
