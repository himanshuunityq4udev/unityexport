using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] Slider loadingSlider;
    [SerializeField] TMP_Text loadingText;
    [SerializeField] MenuController menuController;
    [SerializeField] UiRefrenceProvider uiRefrenceProvider;

    // [SerializeField] GameEvent cameraPermission;

    public void LoadHome(int _index)
    {

        //AndroidCallbacks.Instance.ActivateDeactivateBannerAds(_index);
        SceneManager.LoadScene(_index);
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
    public void LoadLevelNumber(int _index)
    {
        /* if (CheckCameraPermission())
         {*/
        //AndroidCallbacks.Instance.ActivateDeactivateBannerAds(_index);
        //AndroidCallbacks.Instance.ShowFullAds(true);
        StartCoroutine(loadLevelAsync(_index));
        //}

    }



    IEnumerator loadLevelAsync(int levelIndex)
    {
        uiRefrenceProvider._MenuController.PushPage(uiRefrenceProvider.GetPageByName(PanelName.Loading.ToString()));
        yield return new WaitForSeconds(1);
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelIndex);
        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            int progess = (int)progressValue * 100;
            loadingText.text = progess + "%";
            yield return null;
        }
    }
}
