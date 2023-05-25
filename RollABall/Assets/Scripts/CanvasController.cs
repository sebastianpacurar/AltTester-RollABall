using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
    [SerializeField] private GameObject scrollContent;
    [SerializeField] private GameObject scrollItemPrefab;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private TextMeshProUGUI itemCountIndicator;
    private RectTransform _scrollContentRect;
    private RectTransform _scrollItemRect;
    private int _topPaddingVal;

    private void Awake() {
        _topPaddingVal = scrollContent.GetComponent<VerticalLayoutGroup>().padding.top;
        _scrollContentRect = scrollContent.GetComponent<RectTransform>();
        _scrollItemRect = scrollItemPrefab.GetComponent<RectTransform>();
    }

    private void Update() => SetData(scrollContent.transform.childCount);


    public void AddScrollItem() {
        var item = Instantiate(scrollItemPrefab, scrollContent.transform);
        var count = scrollContent.transform.childCount;
        var btn = item.GetComponentInChildren<Button>();

        btn.onClick.AddListener(() => RemoveScrollItem(item));
        item.GetComponentInChildren<TextMeshProUGUI>().text = $"Btn {count}";
        item.name = count.ToString();
    }


    public void Add10ScrollItems() {
        var countPlusTen = scrollContent.transform.childCount + 10;

        while (scrollContent.transform.childCount < countPlusTen) {
            AddScrollItem();
        }
    }


    private void RemoveScrollItem(GameObject obj) => Destroy(obj);


    private void SetData(int count) {
        var rect = _scrollItemRect.rect;

        itemCountIndicator.text = count.ToString();
        _scrollContentRect.sizeDelta = new Vector2(rect.width, count * rect.height + 2 * _topPaddingVal);
        scrollbar.interactable = count > 5f;
    }
}