using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class GameBarPointer : MonoBehaviour
{
    // Bar �� Image�A�Ψ�����ܼƭ�
    [SerializeField] private Image barImage;

    // Pointer �� Image�A�Ψӫ��ܥثe num ����
    [SerializeField] private Image pointerImage;

    // Text ����A�Ψ���̲ܳת����G
    [SerializeField] private Text resultText;

    // �z���B�n�A�ΨӧP�_�I��
    [SerializeField] private RectTransform maskRect;

    // �ܼ� num�A��l�Ȭ� 0
    private float num = 0f;

    // �����ܼƻ��W�λ���
    private bool isIncreasing = true;

    // �������O�_�B��
    private bool isRunning = true;

    // GraphicRaycaster �Ω� UI ���g�u�˴�
    private GraphicRaycaster raycaster;

    // EventSystem �ΨӳB�z�ƥ�
    private EventSystem eventSystem;

    void Start()
    {
        // ��l�� GraphicRaycaster �M EventSystem
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();

        // �Ұʨ�{���� num �W��
        StartCoroutine(ChangeNum());
    }

    void Update()
    {
        // �P�_�I���O�_�o�ͦb Bar Image �ξB�n�W
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUIObject())
            {
                isRunning = false; // �����{
                PrintResult();     // �P�_���G
            }
        }

        // ��s Bar �M Pointer ����m
        UpdateBarAndPointer();
    }

    // �P�_Ĳ���O�_�I���� UI ����]�B�n�� Bar�^
    bool IsPointerOverUIObject()
    {
        // �Ы� PointerEventData �ӫO�s�I���ƥ��T
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        // �O�s�g�u�˴������G
        List<RaycastResult> results = new List<RaycastResult>();

        // �ϥ� GraphicRaycaster �i��g�u�˴�
        raycaster.Raycast(pointerEventData, results);

        // �ˬd�O�_�I����F Bar Image �� MaskRect
        foreach (RaycastResult result in results)
        {
            if (result.gameObject == barImage.gameObject || result.gameObject == maskRect.gameObject)
            {
                return true;
            }
        }
        return false;
    }

    // ��{�����ܼƻ��W�M����
    IEnumerator ChangeNum()
    {
        while (isRunning)
        {
            if (isIncreasing)
            {
                // �C��N num �W�[�� 100
                num += Time.deltaTime * 100f;
                if (num >= 100f)
                {
                    num = 100f;
                    isIncreasing = false; // ����������
                }
            }
            else
            {
                // �C��N num ��֦� 0
                num -= Time.deltaTime * 100f;
                if (num <= 0f)
                {
                    num = 0f;
                    isIncreasing = true; // ���������W
                }
            }

            // �T�O num �C���ܤּW�[�δ�� 1 ���
            num = Mathf.Round(num);
            yield return null; // ���ݤU�@�V
        }
    }

    // ��s Bar �M Pointer ����m
    void UpdateBarAndPointer()
    {
        // �ھ� num �ȧ�s Bar ����R�q�A���] num �d��b 0 �� 100 ����
        barImage.fillAmount = num / 100f;

        // �ھ� num �ȧ�s Pointer ����m
        // �o�̰��] Pointer �����I�M Bar �@�P
        Vector3 pointerPosition = pointerImage.rectTransform.localPosition;
        pointerPosition.x = (num / 100f) * barImage.rectTransform.rect.width;
        pointerImage.rectTransform.localPosition = pointerPosition;
    }

    // �P�_���G
    void PrintResult()
    {
        string result;

        // �ϥ��Y�Ԫ� switch case ���c�ӧP�_���G
        switch (num)
        {
            case float n when (n >= 45f && n <= 55f):
                result = "�j���\";
                break;
            case float n when (n >= 35f && n <= 65f):
                result = "���\";
                break;
            default:
                result = "����";
                break;
        }

        // ��X���G
        Debug.Log($"Log | {result}");

        // ��s���G Text �����
        resultText.text = $"���G: {num}";
    }
}
