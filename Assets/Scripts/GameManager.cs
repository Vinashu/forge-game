using UnityEngine;

public class GameManager : MonoBehaviour
{
    int players = 0;
    Player player;
    Postman.Targets targets;
    bool challenge = false;

    void Start()
    {
        PlayerInit();
        TargetsInit();
    }

    public void ToggleChallenges()
    {
        this.challenge = !this.challenge;
        EventBroker.CallOnShowChallenges(this.challenge);
    }

    public void PlayHeads()
    {
        this.player.UpdateMatches();
        int points = UnityEngine.Random.Range(0, 100);
        if (points < 50)
        {
            // Heads Win
            this.player.UpdateWinsInARow();
            EventBroker.CallOnCoinToss("heads");
        } else
        {
            // Heds Lost
            this.player.ResetWinsInARow();
            EventBroker.CallOnCoinToss("tails");
        }
        Postman.Dispatcher dispatcher = this.player.CreateMessages();        
        string url = Postman.Instance.getServer() + "/api/engine/object";
        string json = JsonUtility.ToJson(dispatcher);
        Postman.Instance.Post(
            json,
            url,
            (error) =>
            {
                Debug.LogError($"error: {error}");
                EventBroker.CallOnPostmanError(error);
            },
            (result) =>
            {
                Rewards rewards = JsonUtility.FromJson<Rewards>(result);
                this.player.UpdateRewards(rewards);
                EventBroker.CallOnPostmanSuccess(
                    $"Message successfully received from the server! {rewards.rewards.Length} rewards for you."
                );
                EventBroker.CallOnPlayerUpdate(this.player);
            }
        );
    }

    public void PlayTails()
    {
        this.player.UpdateMatches();
        int points = UnityEngine.Random.Range(0, 100);
        if (points >= 50)
        {
            // Tails Win
            this.player.UpdateWinsInARow();
            EventBroker.CallOnCoinToss("tails");
        }
        else
        {
            // Tails Lost
            this.player.ResetWinsInARow();
            EventBroker.CallOnCoinToss("heads");
        }
        Postman.Dispatcher dispatcher = this.player.CreateMessages();
        string url = Postman.Instance.getServer() + "/api/engine/object";
        string json = JsonUtility.ToJson(dispatcher);
        Postman.Instance.Post(
            json,
            url,
            (error) =>
            {
                Debug.LogError($"error: {error}");
                EventBroker.CallOnPostmanError(error);
            },
            (result) =>
            {
                Rewards rewards = JsonUtility.FromJson<Rewards>(result);
                this.player.UpdateRewards(rewards);
                EventBroker.CallOnPostmanSuccess(
                    $"Message successfully received from the server! {rewards.rewards.Length} rewards for you."
                );
                EventBroker.CallOnPlayerUpdate(this.player);
            }
        );

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            PlayHeads();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayTails();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            PlayerInit();
        }
    }

    public void PlayerInit()
    {
        this.players++;
        player = new Player($"Player #{this.players}", 0, 0, 0);
        EventBroker.CallOnNewPlayer(this.player);
        EventBroker.CallOnCoinToss("reset");
    }

    private void TargetsInit()
    {
        string url = Postman.Instance.getServer() + "/api/engine/targets/object";
        Postman.Instance.Get(
            url,
            (error) =>
            {
                Debug.LogError($"error: {error}");
                EventBroker.CallOnPostmanError(error);
            },
            (result) =>
            {
                Postman.Targets targets = JsonUtility.FromJson<Postman.Targets>(result);
                this.targets = targets;
            }
        );
    }
}
