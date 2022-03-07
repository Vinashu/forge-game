using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI levelValue;
    [SerializeField] TextMeshProUGUI levelsValue;
    [SerializeField] TextMeshProUGUI pointsValue;
    [SerializeField] TextMeshProUGUI totalPointsValue;
    [SerializeField] TextMeshProUGUI badgesValue;
    [SerializeField] Transform errorWindow;
    [SerializeField] Transform badgesPannel;
    [SerializeField] Transform badgePrefab;
    [SerializeField] Button playButton;

    Dictionary<int, Transform> badges = new Dictionary<int, Transform>();

    float waitTime = 2.0f;

    private void Start()
    {
        EventBroker.OnPlayerUpdate += UpdatePlayerInfo;
        EventBroker.OnNewPlayer += NewPlayerInfo;
        EventBroker.OnPostmanError += OnPostmanError;
        EventBroker.OnPostmanSuccess += OnPostmanSuccess;
    }

    private void UpdatePlayerInfo(Player player)
    {
        this.playerName.text = player.GetName();
        this.levelValue.text = player.GetLevel().ToString();
        this.levelsValue.text = player.GetLevels().ToString();
        this.pointsValue.text = player.GetPoints().ToString();
        this.totalPointsValue.text = player.GetTotalPoints().ToString();
        this.badgesValue.text = player.GetNumberBadges().ToString();

        foreach (KeyValuePair<int, Reward> reward in player.rewards)
        {
            if(!this.badges.ContainsKey(reward.Key))
            {
                Transform badge = Instantiate(badgePrefab, badgesPannel);
                this.badges.Add(reward.Key, badge);
                string url = Postman.Instance.getServer() + "/images/" +reward.Value.imagePath;
                Postman.Instance.GetImage(url,
                   (error) =>
                   {
                       Debug.Log($"Error: {error}");
                   },
                   (result) =>
                   {
                       Debug.Log("Make a successeful image get request");
                       Sprite sprite = Sprite.Create(result, new Rect(0, 0, result.width, result.height), new Vector2(0.5f, 0.5f));
                       badge.GetComponentInChildren<Image>().sprite = sprite;
                   });
            }
        }
    }



    private void NewPlayerInfo(Player player)
    {
        this.playerName.text = player.GetName();
        this.levelValue.text = player.GetLevel().ToString();
        this.levelsValue.text = player.GetLevels().ToString();
        this.pointsValue.text = player.GetPoints().ToString();
        this.totalPointsValue.text = player.GetTotalPoints().ToString();
        this.badgesValue.text = player.GetNumberBadges().ToString();
        ClearBadges();
    }

    private void ClearBadges()
    {
        foreach (Transform child in this.badgesPannel)
        {
            //child.SetParent(null);
            GameObject.Destroy(child.gameObject);
        }
        this.badges.Clear();
    }

    private void OnPostmanSuccess(string result)
    {
        StartCoroutine(OnPostmanSuccessAsync(result));
    }

    IEnumerator OnPostmanSuccessAsync(string result)
    {
        playButton.interactable = false;
        errorWindow.gameObject.SetActive(true);
        Color color = new Color();
        ColorUtility.TryParseHtmlString("#05AD0164", out color);
        Image image = errorWindow.GetComponentInChildren<Image>();
        image.color = color;
        TextMeshProUGUI errorText = errorWindow.GetComponentInChildren<TextMeshProUGUI>();
        errorText.text = result;
        yield return new WaitForSeconds(waitTime);
        errorText.text = "";
        errorWindow.gameObject.SetActive(false);
        playButton.interactable = true;
    }

    private void OnPostmanError(string error)
    {
        StartCoroutine(OnPostmanErrorAsync(error));
    }

    IEnumerator OnPostmanErrorAsync(string error)
    {
        playButton.interactable = false;
        errorWindow.gameObject.SetActive(true);
        Color color = new Color();
        ColorUtility.TryParseHtmlString("#FF210664", out color);
        Image image = errorWindow.GetComponentInChildren<Image>();
        image.color = color;
        TextMeshProUGUI errorText = errorWindow.GetComponentInChildren<TextMeshProUGUI>();
        errorText.text = $"Error: {error}";
        yield return new WaitForSeconds(waitTime);
        errorText.text = "";
        errorWindow.gameObject.SetActive(false);
        playButton.interactable = true;
    }
}
