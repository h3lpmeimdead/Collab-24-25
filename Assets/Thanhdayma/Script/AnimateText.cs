using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AnimateText : MonoBehaviour
{
    public enum TweenType
    {
        Hover, 
        FadeIn,
        FadeOut
    }

    [SerializeField] private TweenType _tweenType;    
    [SerializeField] private float _hoverDistance;
    [Range(0, 1)]
    [SerializeField] private float _tweenDuration;

    Vector3 _startPosition;
    Vector3 _startScale;

    private void Start()
    {
        _startPosition = transform.localPosition; 
        _startScale = transform.localScale;
        PlayTween();
    }
    
    public void PlayTween()
    {
        switch (_tweenType)
        {
            case TweenType.Hover:
                transform.DOLocalMoveY(_startPosition.y + _hoverDistance, _tweenDuration)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
                break;
            case TweenType.FadeIn:
                CanvasGroup canvasFadeInGroup = GetComponent<CanvasGroup>();
                if (canvasFadeInGroup != null)
                {
                    canvasFadeInGroup.alpha = 0f;
                    canvasFadeInGroup.DOFade(1f, _tweenDuration);
                }
                break;
            case TweenType.FadeOut:
                CanvasGroup canvasFadeOut = GetComponent<CanvasGroup>();
                if (canvasFadeOut != null)
                {
                    canvasFadeOut.alpha = 1f;
                    canvasFadeOut.DOFade(0f, _tweenDuration);
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _tweenType = TweenType.FadeIn;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _tweenType = TweenType.FadeOut;
        }
    }
}
