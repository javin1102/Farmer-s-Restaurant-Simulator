using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIMarker : MonoBehaviour
{
    public void TweenTo( float x, float dur ) => transform.DOLocalMoveX( x, dur );
}
