using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private float _fadeTime;
    [SerializeField] private Image _fadePanel;
    [SerializeField] private GameObject _textAndButton;

    public void Appear()
    {

        _textAndButton.SetActive(true);
        //StartCoroutine(Fade());
    }
    /*
    IEnumerator Fade()
    {
        float fadeSpeed = 1 / _fadeTime;
        while(_fadePanel.color.a < 1)
        {
            Color tempColor = _fadePanel.color;
            tempColor.a += fadeSpeed * Time.deltaTime;
            _fadePanel.color = tempColor;
            yield return null;
        }
        yield break;
    }
    */
}
