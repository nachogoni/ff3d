using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    static int score = 0;
    static int enemys = 0;
    static BombType bomb = BombType.DefaultBomb;

    public static void addScore(int iscore)
    {
        score += iscore;
    }

    public static void addEnemy()
    {
        enemys++;
    }

    public static void enemyDie(int score)
    {
        //Bajo la cantidad de enemigos restantes
        enemys--;
        //Agrego el score
        addScore(score);
        //checkeo el status
        checkStatus();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0f, 0f, 300f, 300f), " Score: " + score);
        GUI.Label(new Rect(0f, 15f, 300f, 300f), " Enemys left: " + enemys);
    }

    public static void setEnemys(int count)
    {
        enemys = count;
    }

    public static void checkStatus()
    {
        if (enemys == 0)
        {
            changeLevel();
        }
    }

    public static void restart()
    {
        Application.LoadLevel(0);
    }

    public static void changeLevel()
    {
        Application.LoadLevel(0);
    }

    public static void gameOver()
    {
        Application.LoadLevel(1);
    }
	
}
