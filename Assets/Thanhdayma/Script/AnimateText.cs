using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AnimateText : MonoBehaviour
{
    [SerializeField] private float _hoverDistance;
    [Range(0, 1)]
    [SerializeField] private float _tweenDuration;

    Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.localPosition; 
        PlayTween();
    }
    
    public void PlayTween()
    {
        transform.DOLocalMoveY(_startPosition.y + _hoverDistance, _tweenDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
