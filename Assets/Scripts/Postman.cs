using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Postman : MonoBehaviour
{
    public static Postman Instance;

    private enum Server
    {
        DEV, PROD
    };

    //
    public SpriteRenderer spriteRenderer;
    //

    [SerializeField] private Server target = Server.DEV;
    [SerializeField] private string devAddress = "http://localhost";
    [SerializeField] private int devPort = 3000;
    [SerializeField] private string prodAddress = "http://localhost";
    [SerializeField] private int prodPort = 80;

    private void Awake()
    {
        if(Postman.Instance == null)
        {
            Postman.Instance = this;
            DontDestroyOnLoad(this);
        } else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        string url = getServer() + "/api/test";
        Get(url,
           (error) => {
               Debug.Log($"Error: {error}");
           },
           (result) =>
           {
               Message received = JsonUtility.FromJson<Message>(result);
               Debug.Log($"variable: {received.variable}");
               Debug.Log($"value: {received.value}");
           });
        url = getServer() + "/api/teste";
        Get(url,
           (error) => {
               Debug.Log($"Error: {error}");
           },
           (result) =>
           {
               Message received = JsonUtility.FromJson<Message>(result);
               Debug.Log($"variable: {received.variable}");
               Debug.Log($"value: {received.value}");
           });

        url = getServer() + "/images/badge05.png";
        GetImage(url,
           (error) => {
               Debug.Log($"Error: {error}");
           },
           (result) =>
           {
               Sprite sprite = Sprite.Create(result, new Rect(0, 0, result.width, result.height), new Vector2(0.5f, 0.5f));
               this.spriteRenderer.sprite = sprite;
           });
    }

    private string getServer()
    {
        switch (this.target) {
            case Server.DEV:
                return ($"{this.devAddress}:{this.devPort}");
            case Server.PROD:
                return ($"{this.prodAddress}:{this.prodPort}");
            default:
                return ($"{this.devAddress}:{this.devPort}");
        }
    }

    private void Get(string url, Action<string> onError, Action<string> onSuccess)
    {
        StartCoroutine(GetAssync(url, onError, onSuccess));
    }

    private IEnumerator GetAssync(string url, Action<string> onError, Action<string> onSuccess)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(url))
        {
            yield return unityWebRequest.SendWebRequest();
            if( unityWebRequest.result == UnityWebRequest.Result.ConnectionError ||
                unityWebRequest.result == UnityWebRequest.Result.ProtocolError
              )
            {
                onError(unityWebRequest.error);
            } else
            {
                onSuccess(unityWebRequest.downloadHandler.text);
            }
        }
    }

    private void GetImage(string url, Action<string> onError, Action<Texture2D> onSuccess)
    {
        StartCoroutine(GetImageAssync(url, onError, onSuccess));
    }

    private IEnumerator GetImageAssync (string url, Action<string> onError, Action<Texture2D> onSuccess)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url))
        {
            yield return unityWebRequest.SendWebRequest();
            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError ||
                unityWebRequest.result == UnityWebRequest.Result.ProtocolError
              )
            {
                onError(unityWebRequest.error);
            }
            else
            {
                DownloadHandlerTexture result = unityWebRequest.downloadHandler as DownloadHandlerTexture;
                onSuccess(result.texture);
            }
        }
    }

    private class Message
    {
        public string variable;
        public float value;
    }

    public void test()
    {
        Message message = new Message();
        message.variable = "Level";
        message.value = 1.0f;

        string json = JsonUtility.ToJson(message);
        Debug.Log(json);

        Message received = JsonUtility.FromJson<Message>(json);
        Debug.Log($"variable: {received.variable}");
        Debug.Log($"value: {received.value}");
    }
}
