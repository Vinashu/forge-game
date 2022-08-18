using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Postman : MonoBehaviour
{
    #region variables
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
    #endregion

    #region Singleton
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
    #endregion

    #region Getters
    /// <summary>
    /// Method to setup the sever url according to the deplyment target
    /// </summary>
    /// <returns>server url</returns>
    public string getServer()
    {
        switch (this.target) {
            case Server.DEV:
                return ($"{this.devAddress}:{this.devPort}");
            case Server.PROD:
                return this.prodAddress;
                //return ($"{this.prodAddress}:{this.prodPort}");
            default:
                return ($"{this.devAddress}:{this.devPort}");
        }
    }
    #endregion

    #region Get() method
    /// <summary>
    /// A http get method to make a request to the server
    /// </summary>
    /// <param name="url">The server url</param>
    /// <param name="onError">An event that will be executed if there is an error</param>
    /// <param name="onSuccess">An event that will be executed if the request is succed</param>
    public void Get(string url, Action<string> onError, Action<string> onSuccess)
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
    #endregion

    #region GEtImage() method
    /// <summary>
    /// A http get method to make a request to the server, expecting an image as return
    /// </summary>
    /// <param name="url">The server url</param>
    /// <param name="onError">An event that will be executed if there is an error</param>
    /// <param name="onSuccess">An event that will be executed if the request is succed</param>
    public void GetImage(string url, Action<string> onError, Action<Texture2D> onSuccess)
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
    #endregion

    #region Post() method
    /// <summary>
    /// A http post method to make a request to the server
    /// </summary>
    /// <param name="url">The server url</param>
    /// <param name="onError">An event that will be executed if there is an error</param>
    /// <param name="onSuccess">An event that will be executed if the request is succed</param>
    public void Post(string json, string url, Action<string> onError, Action<string> onSuccess)
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
    #endregion

    #region Put() method
    /// <summary>
    /// A http put method to make a request to the server
    /// </summary>
    /// <param name="url">The server url</param>
    /// <param name="onError">An event that will be executed if there is an error</param>
    /// <param name="onSuccess">An event that will be executed if the request is succed</param>
    public void Put(string json, string url, Action<string> onError, Action<string> onSuccess)
    {
        StartCoroutine(PutAssync(json, url, onError, onSuccess));
    }

    /// <summary>
    /// The asyncronous version of the Put method. 
    /// </summary>
    /// <returns>It will execute onError or onSuccess depending on the return of the request</returns>
    private IEnumerator PutAssync(string json, string url, Action<string> onError, Action<string> onSuccess)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Put(url, json))
        {
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
    #endregion

    #region Delete() method
    /// <summary>
    /// A http delete method to make a request to the server
    /// </summary>
    /// <param name="url">The server url</param>
    /// <param name="onError">An event that will be executed if there is an error</param>
    /// <param name="onSuccess">An event that will be executed if the request is succed</param>
    public void Delete(string url, Action<string> onError, Action<string> onSuccess)
    {
        StartCoroutine(DeleteAssync(url, onError, onSuccess));
    }

    /// <summary>
    /// The asyncronous version of the Delete method. 
    /// </summary>
    /// <returns>It will execute onError or onSuccess depending on the return of the request</returns>
    private IEnumerator DeleteAssync(string url, Action<string> onError, Action<string> onSuccess)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Delete(url))
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
                onSuccess("Message successefully received from the server.");
            }
        }
    }
    #endregion

    #region Message Control
    /// <summary>
    /// Class to format the JSON object that will be sent to the server
    /// </summary>
    [Serializable]
    public class Message
    {
        public string variable;
        public float value;

        public Message() { }

        public Message(string variable, float value)
        {
            this.variable = variable;
            this.value = value;
        }
    }

    /// <summary>
    /// Class to combine all the messages to be sent as just one JSON
    /// object to the server
    /// </summary>
    [Serializable]
    public class Dispatcher
    {
        public Message[] messages;

        public Dispatcher() { }

        public Dispatcher(Message[] messages)
        {
            //this.messages = new Message[messages.Length];
            this.messages = messages;
            //Debug.Log(this.messages[0].variable);
        }
    }
    #endregion

    #region Tests
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
        //RunTests();
    }

    private void RunTests()
    {
        //Get Test
        string url = getServer() + "/api/test";
        Get(url,
           (error) =>
           {
               Debug.Log($"Error: {error}");
           },
           (result) =>
           {
               Debug.Log("Make a successeful GET requset");
               Message received = JsonUtility.FromJson<Message>(result);
               Debug.Log($"variable: {received.variable}");
               Debug.Log($"value: {received.value}");
           });

        //Invalid Get Test
        url = getServer() + "/api/teste";
        Get(url,
           (error) =>
           {
               Debug.Log("Make an UNsuccesseful GET requset");
               Debug.Log($"Error: {error}");
           },
           (result) =>
           {
               Message received = JsonUtility.FromJson<Message>(result);
               Debug.Log($"variable: {received.variable}");
               Debug.Log($"value: {received.value}");
           });

        //Delete Test
        url = getServer() + "/api/test";
        Delete(url,
           (error) =>
           {
               Debug.Log($"Error: {error}");
           },
           (result) =>
           {
               Debug.Log("Make a successeful DELETE requset");
               Debug.Log($"variable: {result}");
           });

        //Post test
        url = getServer() + "/api/test";
        Message message = new Message();
        message.variable = "Level";
        message.value = 1.0f;
        string json = JsonUtility.ToJson(message);
        Post(json, url,
           (error) =>
           {
               Debug.Log($"Error: {error}");
           },
           (result) =>
           {
               Debug.Log("Make a successeful POST requset");
               Message received = JsonUtility.FromJson<Message>(result);
               Debug.Log($"variable: {received.variable}");
               Debug.Log($"value: {received.value}");
           });

        //Put test
        url = getServer() + "/api/test";
        message.variable = "Level";
        message.value = 1.0f;
        json = JsonUtility.ToJson(message);
        Put(json, url,
           (error) =>
           {
               Debug.Log($"Error: {error}");
           },
           (result) =>
           {
               Debug.Log("Make a successeful PUT requset");
               Message received = JsonUtility.FromJson<Message>(result);
               Debug.Log($"variable: {received.variable}");
               Debug.Log($"value: {received.value}");
           });


        //Get Image test
        url = getServer() + "/images/badge02.png";
        GetImage(url,
           (error) =>
           {
               Debug.Log($"Error: {error}");
           },
           (result) =>
           {
               Debug.Log("Make a successeful image get requset");
               Sprite sprite = Sprite.Create(result, new Rect(0, 0, result.width, result.height), new Vector2(0.5f, 0.5f));
               this.spriteRenderer.sprite = sprite;
           });
    }
    #endregion
}
