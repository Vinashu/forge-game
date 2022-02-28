using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Postman : MonoBehaviour
{
    /// <summary>
    /// There are two possible servers, the DEVelopment and the
    /// PRODuction server.
    /// </summary>
    private enum Server
    {
        DEV, PROD
    };

    /// <summary>
    /// Strings to setup the url for the dev and production sever
    /// Those can be set in the Unity editor
    /// </summary>
    [SerializeField] private Server target = Server.DEV;
    [SerializeField] private string devAddress = "http://localhost";
    [SerializeField] private int devPort = 3000;
    [SerializeField] private string prodAddress = "http://localhost";
    [SerializeField] private int prodPort = 80;

    /// <summary>
    /// Setup the Singleton pattern
    /// When attached to one game object it will become available
    /// to all other game objects through the Instance property
    /// </summary>
    public static Postman Instance;

    private void Awake()
    {
        //Check if already exists an instance
        if(Postman.Instance == null)
        {
            //Creat the static instance
            Postman.Instance = this;
            DontDestroyOnLoad(this);
        } else
        {
            //Destroy the object to garantee that
            //there is just one instance of this class
            Destroy(this);
        }
    }

    /// <summary>
    /// Method to setup the sever url according to the deplyment target
    /// </summary>
    /// <returns>server url</returns>
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

    /// <summary>
    /// A http get method to make a request to the server
    /// </summary>
    /// <param name="url">The server url</param>
    /// <param name="onError">An event that will be executed if there is an error</param>
    /// <param name="onSuccess">An event that will be executed if the request is succed</param>
    private void Get(string url, Action<string> onError, Action<string> onSuccess)
    {
        StartCoroutine(GetAssync(url, onError, onSuccess));
    }

    /// <summary>
    /// The asyncronous version of the Get method
    /// </summary>
    /// <returns>It will execute onError or onSuccess depending on the return of the request</returns>
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

    /// <summary>
    /// A http get method to make a request to the server, expecting an image as return
    /// </summary>
    /// <param name="url">The server url</param>
    /// <param name="onError">An event that will be executed if there is an error</param>
    /// <param name="onSuccess">An event that will be executed if the request is succed</param>
    private void GetImage(string url, Action<string> onError, Action<Texture2D> onSuccess)
    {
        StartCoroutine(GetImageAssync(url, onError, onSuccess));
    }

    /// <summary>
    /// The asyncronous version of the GetImage method
    /// </summary>
    /// <returns>It will execute onError or onSuccess depending on the return of the request</returns>
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

    /// <summary>
    /// A http post method to make a request to the server
    /// </summary>
    /// <param name="url">The server url</param>
    /// <param name="onError">An event that will be executed if there is an error</param>
    /// <param name="onSuccess">An event that will be executed if the request is succed</param>
    private void Post(string json, string url, Action<string> onError, Action<string> onSuccess)
    {
        StartCoroutine(PostAssync(json, url, onError, onSuccess));
    }

    /// <summary>
    /// The asyncronous version of the Post method. It does not work properly if you create the
    /// object as UnityWebRequest.Post. To make it work it is needed to create it as a UnityWebRequest.Put
    /// and them change method property to be a Post request.
    /// </summary>
    /// <returns>It will execute onError or onSuccess depending on the return of the request</returns>
    private IEnumerator PostAssync(string json, string url, Action<string> onError, Action<string> onSuccess)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Put(url, json))
        {
            unityWebRequest.method = UnityWebRequest.kHttpVerbPOST;
            unityWebRequest.SetRequestHeader("Content-Type", "application/json");
            unityWebRequest.SetRequestHeader("Accept", "application/json");
            yield return unityWebRequest.SendWebRequest();
            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError ||
                unityWebRequest.result == UnityWebRequest.Result.ProtocolError
              )
            {
                onError(unityWebRequest.error);
            }
            else
            {
                onSuccess(unityWebRequest.downloadHandler.text);
            }
        }
    }

    /// <summary>
    /// Class to format the JSON object that will be sent to the server
    /// </summary>
    private class Message
    {
        public string variable;
        public float value;
    }


    /// <summary>
    /// Just for test purpose, should remove eventually
    /// </summary>
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


    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        string url = getServer() + "/api/test";
        Get(url,
           (error) =>
           {
               Debug.Log($"Error: {error}");
           },
           (result) =>
           {
               Debug.Log("Make a successeful get requset");
               Message received = JsonUtility.FromJson<Message>(result);
               Debug.Log($"variable: {received.variable}");
               Debug.Log($"value: {received.value}");
           });

        url = getServer() + "/api/teste";
        Get(url,
           (error) =>
           {
               Debug.Log("Make an unsuccesseful get requset");
               Debug.Log($"Error: {error}");
           },
           (result) =>
           {
               Message received = JsonUtility.FromJson<Message>(result);
               Debug.Log($"variable: {received.variable}");
               Debug.Log($"value: {received.value}");
           });

        url = getServer() + "/api/test";
        Message message = new Message();
        message.variable = "Level";
        message.value = 1.0f;
        string json = JsonUtility.ToJson(message);
        Post(json,url,
           (error) => {
               Debug.Log($"Error: {error}");
           },
           (result) =>
           {
               Debug.Log("Make a successeful post requset");
               Message received = JsonUtility.FromJson<Message>(result);
               Debug.Log($"variable: {received.variable}");
               Debug.Log($"value: {received.value}");
           });

        url = getServer() + "/images/badge02.png";
        GetImage(url,
           (error) => {
               Debug.Log($"Error: {error}");
           },
           (result) =>
           {
               Debug.Log("Make a successeful image get requset");
               Sprite sprite = Sprite.Create(result, new Rect(0, 0, result.width, result.height), new Vector2(0.5f, 0.5f));
               this.spriteRenderer.sprite = sprite;
           });
    }
}
