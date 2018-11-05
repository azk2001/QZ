using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PlayTweener : MonoBehaviour
{
    private List<DOTweenAnimation> tweenerList = new List<DOTweenAnimation>();

    void Awake()
    {
        DOTweenAnimation[] tweener = this.gameObject.GetComponents<DOTweenAnimation>();
        if(tweener!=null)
        {
            tweenerList.AddRange(tweener);
        }

        tweener = this.gameObject.GetComponentsInChildren<DOTweenAnimation>();
        if (tweener != null)
        {
            tweenerList.AddRange(tweener);
        }
    }

    public void Play()
    {
        for (int i = 0,max = tweenerList.Count; i < max; i++)
        {
            tweenerList[i].isRelative = true;
            tweenerList[i].isFrom = true;
            tweenerList[i].DORestart();
            tweenerList[i].DOPlayForward();
        }
    }
}
