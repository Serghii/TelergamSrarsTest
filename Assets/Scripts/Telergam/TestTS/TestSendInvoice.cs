using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestSendInvoice : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private Button btnTest;
    private TMP_Text _text;
    private const string invoiceUrl = "https://t.me/$j4RuD3LOWEnbBgAA5o-hIxwq1GM";
    private const string invoiceUrlTEST = "https://t.me/$1jW0QTQKYUkDAAAATBWtryMQQtU";

     
    private void OnEnable()
    {
        _text = btn.GetComponentInChildren<TMP_Text>();
        btn.onClick.AddListener(TrySendInvoice);
        btnTest.onClick.AddListener(TrySendInvoiceTest);
    }

    private void OnDisable()
    {
        btn.onClick.RemoveListener(TrySendInvoice);
        btnTest.onClick.RemoveListener(TrySendInvoiceTest);
    }

    private void TrySendInvoice() => SendInvoiceWithURL(invoiceUrl);

    private void TrySendInvoiceTest() => SendInvoiceWithURL(invoiceUrlTEST);

    private void SendInvoiceWithURL(string url)
    {
        _text.text = $"Try Send {(url == invoiceUrl ? "Orig" : "Test")}";
        TelegramConnect.SendInvoice(url);
        _text.text = "Sending!";
    }
}
