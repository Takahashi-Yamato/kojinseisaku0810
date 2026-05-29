using UnityEngine;
using UnityEngine.SceneManagement;

public class Clear : MonoBehaviour
{
    public void EndGame()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
