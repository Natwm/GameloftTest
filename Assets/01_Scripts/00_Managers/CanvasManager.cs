using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    [Header("Panel")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject victoryPanel;

    [Header("Text")]
    public TMPro.TMP_Text amoutOfObjectToKill;
    public TMPro.TMP_Text gameTimer;

    [Header("Image")]
    [SerializeField] private GameObject handImage;

    [Header("Slider")]
    [SerializeField] private Slider sliderIndicator;

    [Header("Animation")]
    [SerializeField] private float handMouvementSpeed;
    [SerializeField] private GameObject pos;

    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : CanvasManager");
        else
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);

        handImage.transform.DOMove(pos.transform.position, handMouvementSpeed).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// Disable the Menu panel and Enable the game panel
    /// </summary>
    public void DisableMenu()
    {
        menuPanel.SetActive(false);
        handImage.transform.DOKill();

        gamePanel.SetActive(true);
    }

    /// <summary>
    /// Set Up the game panel
    /// </summary>
    /// <param name="amountOfobject"> Amount of object that the player have to destroy to win the level</param>
    public void SetUpGamePanel(int amountOfobject)
    {
        SetUpSlider(amountOfobject);
        UpdateLevelIndicator(amountOfobject);
    }

    public void SetUpSlider(int amountOfobject)
    {
        sliderIndicator.maxValue = amountOfobject;
        sliderIndicator.value = 0;
    }

    public void UpdateLevelIndicator(int amountOfobject)
    {
        amoutOfObjectToKill.text = amountOfobject.ToString();
        sliderIndicator.value = sliderIndicator.maxValue - amountOfobject;
    }

    public void VictoryPanel()
    {
        victoryPanel.GetComponent<CanvasGroup>().DOFade(1, 0.6f);
        victoryPanel.SetActive(true);
    }
}
