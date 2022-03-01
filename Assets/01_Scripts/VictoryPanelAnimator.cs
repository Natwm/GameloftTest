using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VictoryPanelAnimator : MonoBehaviour
{
    public bool isGameOver = false;

    public List<GameObject> _Stars;
    public List<GameObject> _ActionButton;

    private List<Sequence> _StarSequence = new List<Sequence>();

    [Space]
    public TMPro.TMP_Text timerText;


    private const float INITIAL_DELAY = 1.3f;
    private const float DELAY_BETWEEN_STAR = 0.3f;

    /// <summary>
    /// Initialise all star and there animation
    /// </summary>
    public void AnimatedStar(int score)
    {
        int index = -1;
        for (int i = 0; i < score; i++)
        {
            index++;
            _Stars[i].transform.localScale = Vector3.zero;
            AnimatedStar(index, INITIAL_DELAY + DELAY_BETWEEN_STAR * index);
        }
    }

    /// <summary>
    /// Set Up the DoTween Sequence for each Star and play it.
    /// </summary>
    /// <param name="index"> The index of the star to animate</param>
    /// <param name="delay"> The delay between the set-up and the animation </param>
    private void AnimatedStar(int index, float delay)
    {
        if (_StarSequence.Count <= index)
        {
            _StarSequence.Add(DOTween.Sequence());
        }
        else
        {
            if (_StarSequence[index].IsPlaying())
            {
                _StarSequence[index].Kill(true);
            }
        }

        Sequence currentSequence = _StarSequence[index];
        GameObject currentStar = _Stars[index];

        currentSequence.Append(currentStar.transform.DOScale(1, 0.3f)).
                        Append(currentStar.transform.DOPunchScale(Vector3.one * 0.5f, 0.7f, 5, 0.7f).SetEase(Ease.OutCirc))
                        .AppendCallback(() => SoundManager.instance.PlayStartSpawnUI());

        currentSequence.PrependInterval(delay);
    }

    /// <summary>
    /// Make pulse All button in the _ActionButton list.
    /// </summary>
    private void AnimatedButton()
    {
        foreach (var item in _ActionButton)
        {
            item.transform.DOScale(1.02f, 0.3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void OnEnable()
    {
        if(!isGameOver)
            AnimatedStar(ScoreManager.instance.playerScore);

        AnimatedButton();
        timerText.text = GameManager.instance.GetLevelTimer();
    }
}
