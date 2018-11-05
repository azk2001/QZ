using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextRunTween : MonoBehaviour
{

    public UIText uiText = null;

    public AnimationCurve easeCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    public string mFormat = "{0}";

    public float showTime = 1;

    private bool isPlay = false;
    private float dataTime = 0;

    private float start = 0;
    private float end = 0;

    private void Awake()
    {
        if (uiText == null)
            uiText = this.GetComponent<UIText>();

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (isPlay == false)
            return;

        dataTime += Time.deltaTime;

        uiText.text = string.Format(mFormat, (int)(start + (end - start) * easeCurve.Evaluate(dataTime / showTime)));
    }

    public void Play(int start, int end)
    {
        this.start = start;
        this.end = end;

        dataTime = 0;

        isPlay = true;

        uiText.text = start.ToString();
    }

    private void OnEnable()
    {
        Play(0, 100);
    }
}
