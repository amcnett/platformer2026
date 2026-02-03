using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int lives = 3;

    public static GameManager instance = null;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void DecreaseLives()
    {
        lives--;
    }

    public int GetLives()
    {
        return lives;
    }
}
