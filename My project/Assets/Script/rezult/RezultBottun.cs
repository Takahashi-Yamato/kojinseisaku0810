using UnityEngine;
using UnityEngine.SceneManagement;

public class RezultBottun : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Retry()
    {
        // 直前のステージ番号を保存しておく必要がある
        int lastStage = PlayerPrefs.GetInt("LastStage", 0);
        SceneManager.LoadScene(lastStage);
    }
    // タイトルへボタンのOnClickに割り当て
    public void OnTitleButton()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
    }
}
