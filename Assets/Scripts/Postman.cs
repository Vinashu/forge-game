using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Postman : MonoBehaviour
{
    public static Postman Instance;

    private enum Server
    {
        DEV, PROD
    };

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

    private string getServer()
    {
        switch (this.target) {
            case Server.DEV:
                return ($"{this.devAddress}:{this.devPort}/");
            case Server.PROD:
                return ($"{this.prodAddress}:{this.prodPort}/");
            default:
                return ($"{this.devAddress}:{this.devPort}/");
        }
    }

    private void Get()
    {

    }

    private void GetImage()
    {

    }

    private IEnumerator GetAssync()
    {
        yield return null;
    }

    private IEnumerator GetImageAssync ()
    {
        yield return null;
    }
}
