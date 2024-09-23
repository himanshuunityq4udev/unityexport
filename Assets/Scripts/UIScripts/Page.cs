using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
[DisallowMultipleComponent]
public class Page : MonoBehaviour
{
    SoundManager soundManager;
    private RectTransform RectTransform;
    private CanvasGroup CanvasGroup;

    [SerializeField]
    private float AnimationSpeed = 1f;
    public bool ExitOnNewPagePush = false;

    [SerializeField]
    private EntryMode EntryMode = EntryMode.SLIDE;
    [SerializeField]
    private Direction EntryDirection = Direction.LEFT;
    [SerializeField]
    private EntryMode ExitMode = EntryMode.SLIDE;
    [SerializeField]
    private Direction ExitDirection = Direction.LEFT;
    [SerializeField]
    private UnityEvent PrePushAction;
    [SerializeField]
    private UnityEvent PostPushAction;
    [SerializeField]
    private UnityEvent PrePopAction;
    [SerializeField]
    private UnityEvent PostPopAction;

    private Coroutine AnimationCoroutine;
   

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        CanvasGroup = GetComponent<CanvasGroup>();
        soundManager = FindObjectOfType<SoundManager>();    
    }

    public void ResetFadeEffect()
    {
        CanvasGroup.alpha = 1f;
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;
    }


    public void Enter(bool PlayAudio)
    {
        PrePushAction?.Invoke();

        switch(EntryMode)
        {
            case EntryMode.SLIDE:
                SlideIn(PlayAudio);
                break;
            case EntryMode.ZOOM:
                ZoomIn(PlayAudio);
                break;
            case EntryMode.FADE:
                FadeIn(PlayAudio);
                break;
        }
    }

    public void Exit(bool PlayAudio)
    {
		PrePopAction?.Invoke();

        switch (ExitMode)
        {
            case EntryMode.SLIDE:
                SlideOut(PlayAudio);
                break;
            case EntryMode.ZOOM:
                ZoomOut(PlayAudio);
                break;
            case EntryMode.FADE:
                FadeOut(PlayAudio);
                break;
        }
    }

    private void SlideIn(bool PlayAudio)
    {
        if (AnimationCoroutine != null)
        {
            StopCoroutine(AnimationCoroutine);
        }
        AnimationCoroutine = StartCoroutine(AnimationHelper.SlideIn(RectTransform, EntryDirection, AnimationSpeed, PostPushAction));

        soundManager.EnterPlayClip(PlayAudio);
    }

    private void SlideOut(bool PlayAudio)
    {
        if (AnimationCoroutine != null)
        {
            StopCoroutine(AnimationCoroutine);
        }
        AnimationCoroutine = StartCoroutine(AnimationHelper.SlideOut(RectTransform, ExitDirection, AnimationSpeed, PostPopAction));

        soundManager.ExitPlayClip(PlayAudio);
    }
    
    private void ZoomIn(bool PlayAudio)
    {
        if (AnimationCoroutine != null)
        {
            StopCoroutine(AnimationCoroutine);
        }
        AnimationCoroutine = StartCoroutine(AnimationHelper.ZoomIn(RectTransform, AnimationSpeed, PostPushAction));

        soundManager.EnterPlayClip(PlayAudio);
    }

    private void ZoomOut(bool PlayAudio)
    {
        if (AnimationCoroutine != null)
        {
            StopCoroutine(AnimationCoroutine);
        }
        AnimationCoroutine = StartCoroutine(AnimationHelper.ZoomOut(RectTransform, AnimationSpeed, PostPopAction));

        soundManager.ExitPlayClip(PlayAudio);
    }

    private void FadeIn(bool PlayAudio)
    {
        if (AnimationCoroutine != null)
        {
            StopCoroutine(AnimationCoroutine);
        }
        AnimationCoroutine = StartCoroutine(AnimationHelper.FadeIn(CanvasGroup, AnimationSpeed, PostPushAction));

        soundManager.EnterPlayClip(PlayAudio);
    }

    private void FadeOut(bool PlayAudio)
    {
        if (AnimationCoroutine != null)
        {
            StopCoroutine(AnimationCoroutine);
        }
        AnimationCoroutine = StartCoroutine(AnimationHelper.FadeOut(CanvasGroup, AnimationSpeed, PostPopAction));

        soundManager.ExitPlayClip(PlayAudio);
    }

   
}
