using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI matchesValue;
    [SerializeField] TextMeshProUGUI pointsValue;
    [SerializeField] TextMeshProUGUI winsInARowValue;
    [SerializeField] TextMeshProUGUI badgesValue;
    [SerializeField] Transform errorWindow;
    [SerializeField] Transform badgesPannel;
    [SerializeField] Transform badgePrefab;
    [SerializeField] Button playHeads;
    [SerializeField] Button playTails;
    [SerializeField] Transform heads;
    [SerializeField] Transform tails;
    [SerializeField] Transform challenges;


    Dictionary<string, Transform> badges = new Dictionary<string, Transform>();

    float waitTime = 2.0f;

    private void Start()
    {
        EventBroker.OnPlayerUpdate += UpdatePlayerInfo;
        EventBroker.OnNewPlayer += NewPlayerInfo;
        EventBroker.OnPostmanError += OnPostmanError;
        EventBroker.OnPostmanSuccess += OnPostmanSuccess;
        EventBroker.OnCoinToss += OnCoinToss;
        EventBroker.OnShowChallenges += OnShowChallenges;
    }

    private void OnShowChallenges(bool modal)
    {
        this.challenges.gameObject.SetActive(modal);
    }

    private void OnCoinToss(string coin)
    {
        switch (coin)
        {
            case "heads":
                heads.GetComponent<SpriteRenderer>().color = Color.green;
                tails.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case "tails":
                heads.GetComponent<SpriteRenderer>().color = Color.red;
                tails.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case "reset":
                heads.GetComponent<SpriteRenderer>().color = Color.white;
                tails.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            default:
                heads.GetComponent<SpriteRenderer>().color = Color.white;
                tails.GetComponent<SpriteRenderer>().color = Color.white;
                break;
        }
    }

    private void UpdatePlayerInfo(Player player)
    {
        this.playerName.text = player.GetName();
        this.matchesValue.text = player.GetMatches().ToString();
        this.pointsValue.text = player.GetPoints().ToString();
        this.winsInARowValue.text = player.GetWinsInARow().ToString();
        this.badgesValue.text = player.GetNumberBadges().ToString();

        foreach (KeyValuePair<string, Reward> reward in player.rewards)
        {
            if(!this.badges.ContainsKey(reward.Key))
            {
                Transform badge = Instantiate(badgePrefab, badgesPannel);
                this.badges.Add(reward.Key, badge);
                //string url = Postman.Instance.getServer() + "/images/" +reward.Value.imagePath;
                string url = reward.Value.imagePath;
                Postman.Instance.GetImage(url,
                   (error) =>
                   {
                       Debug.Log($"Error: {error}");
                   },
                   (result) =>
                   {
                       Sprite sprite = Sprite.Create(result, new Rect(0, 0, result.width, result.height), new Vector2(0.5f, 0.5f));
                       badge.GetComponentInChildren<Image>().sprite = sprite;
                   });
            }
        }
    }



    private void NewPlayerInfo(Player player)
    {
        this.playerName.text = player.GetName();
        this.matchesValue.text = player.GetMatches().ToString();
        this.pointsValue.text = player.GetPoints().ToString();
        this.winsInARowValue.text = player.GetWinsInARow().ToString();
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
        playHeads.interactable = false;
        playTails.interactable = false;
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
        EventBroker.CallOnCoinToss("reset");
        playHeads.interactable = true;
        playTails.interactable = true;
    }

    private void OnPostmanError(string error)
    {
        StartCoroutine(OnPostmanErrorAsync(error));
    }

    IEnumerator OnPostmanErrorAsync(string error)
    {
        playHeads.interactable = false;
        playTails.interactable = false;
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
        EventBroker.CallOnCoinToss("reset");
        playHeads.interactable = true;
        playTails.interactable = true;
    }
}
