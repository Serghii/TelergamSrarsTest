using Newtonsoft.Json;
using TMPro;
using UnityEngine;
#if !UNITY_EDITOR && UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

public class TelegramConnect : MonoBehaviour
{
    
#if !UNITY_EDITOR && UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void InvoiceInternal(string url);
#endif
    [SerializeField] private TMP_Text _log;
    public UserData UserData { get; private set; }
    
    public static void SendInvoice(string url)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        InvoiceInternal(url);
#else
        Debug.Log($"SendInvoice called with url: {url}");
#endif
    }
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Application.ExternalCall("onUnityReady");
    }
    
    public void CollectUserData(string jsonUserData)
    {
        Debug.LogError("CollectUserData:" + jsonUserData);
        UserData = JsonConvert.DeserializeObject<UserData>(jsonUserData);
        _log.text = UserData.UserId;
    }
    
    /*[DllImport("__Internal")]
    public static extern void Hello();


    [DllImport("__Internal")]
    public static extern void RequestThemeParams();

    [DllImport("__Internal")]
    public static extern void RequestUserData();

    [DllImport("__Internal")]
    public static extern void ShowMainButton(string text);

    [DllImport("__Internal")]
    public static extern void HideMainButton();

    [DllImport("__Internal")]
    public static extern void MainButtonShowProgress();

    [DllImport("__Internal")]
    public static extern void MainButtonHideProgress();

    [DllImport("__Internal")]
    public static extern void ShowBackButton();

    [DllImport("__Internal")]
    public static extern void HideBackButton();

    [DllImport("__Internal")]
    public static extern void ShowAlert(string text);

    [DllImport("__Internal")]
    public static extern void ShowShareJoinCode(string code);

    [DllImport("__Internal")]
    public static extern void Ready();

    [DllImport("__Internal")]
    public static extern void Close();

    [DllImport("__Internal")]
    public static extern void Expand();

    [DllImport("__Internal")]
    public static extern void HapticFeedback(string level);

    [DllImport("__Internal")]
    public static extern void ShowScanQrPopup(string text);

    [DllImport("__Internal")]
    public static extern void CloseScanQrPopup();*/
}
public class UserData
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}