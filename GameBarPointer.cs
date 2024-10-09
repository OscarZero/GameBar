using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class GameBarPointer : MonoBehaviour
{
    // Bar 的 Image，用來顯示變數值
    [SerializeField] private Image barImage;

    // Pointer 的 Image，用來指示目前 num 的值
    [SerializeField] private Image pointerImage;

    // Text 元件，用來顯示最終的結果
    [SerializeField] private Text resultText;

    // 透明遮罩，用來判斷點擊
    [SerializeField] private RectTransform maskRect;

    // 變數 num，初始值為 0
    private float num = 0f;

    // 控制變數遞增或遞減
    private bool isIncreasing = true;

    // 控制機制是否運行
    private bool isRunning = true;

    // GraphicRaycaster 用於 UI 的射線檢測
    private GraphicRaycaster raycaster;

    // EventSystem 用來處理事件
    private EventSystem eventSystem;

    void Start()
    {
        // 初始化 GraphicRaycaster 和 EventSystem
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();

        // 啟動協程控制 num 增減
        StartCoroutine(ChangeNum());
    }

    void Update()
    {
        // 判斷點擊是否發生在 Bar Image 或遮罩上
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUIObject())
            {
                isRunning = false; // 停止協程
                PrintResult();     // 判斷結果
            }
        }

        // 更新 Bar 和 Pointer 的位置
        UpdateBarAndPointer();
    }

    // 判斷觸控是否點擊到 UI 元件（遮罩或 Bar）
    bool IsPointerOverUIObject()
    {
        // 創建 PointerEventData 來保存點擊事件資訊
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        // 保存射線檢測的結果
        List<RaycastResult> results = new List<RaycastResult>();

        // 使用 GraphicRaycaster 進行射線檢測
        raycaster.Raycast(pointerEventData, results);

        // 檢查是否點擊到了 Bar Image 或 MaskRect
        foreach (RaycastResult result in results)
        {
            if (result.gameObject == barImage.gameObject || result.gameObject == maskRect.gameObject)
            {
                return true;
            }
        }
        return false;
    }

    // 協程控制變數遞增和遞減
    IEnumerator ChangeNum()
    {
        while (isRunning)
        {
            if (isIncreasing)
            {
                // 每秒將 num 增加至 100
                num += Time.deltaTime * 100f;
                if (num >= 100f)
                {
                    num = 100f;
                    isIncreasing = false; // 切換為遞減
                }
            }
            else
            {
                // 每秒將 num 減少至 0
                num -= Time.deltaTime * 100f;
                if (num <= 0f)
                {
                    num = 0f;
                    isIncreasing = true; // 切換為遞增
                }
            }

            // 確保 num 每次至少增加或減少 1 單位
            num = Mathf.Round(num);
            yield return null; // 等待下一幀
        }
    }

    // 更新 Bar 和 Pointer 的位置
    void UpdateBarAndPointer()
    {
        // 根據 num 值更新 Bar 的填充量，假設 num 範圍在 0 到 100 之間
        barImage.fillAmount = num / 100f;

        // 根據 num 值更新 Pointer 的位置
        // 這裡假設 Pointer 的錨點和 Bar 一致
        Vector3 pointerPosition = pointerImage.rectTransform.localPosition;
        pointerPosition.x = (num / 100f) * barImage.rectTransform.rect.width;
        pointerImage.rectTransform.localPosition = pointerPosition;
    }

    // 判斷結果
    void PrintResult()
    {
        string result;

        // 使用嚴謹的 switch case 結構來判斷結果
        switch (num)
        {
            case float n when (n >= 45f && n <= 55f):
                result = "大成功";
                break;
            case float n when (n >= 35f && n <= 65f):
                result = "成功";
                break;
            default:
                result = "失敗";
                break;
        }

        // 輸出結果
        Debug.Log($"Log | {result}");

        // 更新結果 Text 的顯示
        resultText.text = $"結果: {num}";
    }
}
