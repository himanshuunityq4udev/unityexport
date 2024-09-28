using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif


public class SceneController : MonoBehaviour
{
    public void GoHome()
    {
        //AndroidCallbacks.Instance.ActivateDeactivateBannerAds(_index);
        SceneManager.LoadScene(0);
    }

    public int GetCurrentScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        return index;
    }

    public void LoadCurrentScene()
    {
        int index = GetCurrentScene();
        SceneManager.LoadScene(index);
    }
    public void LoadScene(int _index)
    {
        //AndroidCallbacks.Instance.ActivateDeactivateBannerAds(_index);
        //AndroidCallbacks.Instance.ShowFullAds(true);
        StartCoroutine(loadLevelAsync(_index));
    }

    IEnumerator loadLevelAsync(int levelIndex)
    {
        //AssetRefrence.Instance.LoadingPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelIndex);
        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
           /* AssetRefrence.Instance.LoadingSlider.value = progressValue;
            int progess = (int)progressValue * 100;
            AssetRefrence.Instance.LoadingText.text = progess + "%";*/
            yield return null;
        }
    }

}