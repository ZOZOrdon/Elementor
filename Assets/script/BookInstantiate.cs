using System.Collections;
using UnityEngine;

public class BookSpawner : MonoBehaviour
{
    [Header("Book Settings")]
    public GameObject bookPrefab;          // ���Ԥ�Ƽ�
    public float bookInitialScale = 0.01f; // ��ĳ�ʼ���ű���
    public float bookTargetScale = 1.0f;   // ���Ŀ�����ű���
    public float bookDropDistance = 1.0f;  // �鼮����ľ���
    public float bookDropDuration = 2.0f;  // �鼮����ĳ���ʱ��

    private PortalController portalController;

    void Start()
    {
        StartCoroutine(BookSequence());
    }

    IEnumerator BookSequence()
    {
        // �ȴ������ų���
        yield return StartCoroutine(WaitForPortal());

        if (portalController == null)
        {
            Debug.LogError("δ���ҵ� PortalController��");
            yield break;
        }

        // �ȴ� 5 ��
        yield return new WaitForSeconds(5.0f);

        // �����鼮
        StartCoroutine(SpawnBook());

        // �ٵȴ� 5 ��
        yield return new WaitForSeconds(5.0f);

        // ���ô����ŵ����ط���
        portalController.HidePortal();
    }

    IEnumerator WaitForPortal()
    {
        // ���ϲ��� PortalController ʵ��
        while (portalController == null)
        {
            portalController = FindObjectOfType<PortalController>();
            if (portalController == null)
            {
                yield return null;
            }
        }
    }

    IEnumerator SpawnBook()
    {
        // ��ȡ�����ŵ�����λ����Ϊ�����ʼλ��
        Vector3 startPosition = portalController.transform.position;

        // �������Ŀ��λ�ã��Ӵ�����λ������ƫ�� bookDropDistance��
        Vector3 targetPosition = startPosition - new Vector3(0, bookDropDistance, 0);

        // ʵ�����鼮
        GameObject bookInstance = Instantiate(bookPrefab, startPosition, Quaternion.identity);
        // ȷ���鼮���������κθ�����
        bookInstance.transform.SetParent(null);
        // ���ó�ʼ����Ϊ��ʼֵ���������ɼ���
        bookInstance.transform.localScale = Vector3.one * bookInitialScale;

        float elapsed = 0f;
        while (elapsed < bookDropDuration)
        {
            float t = elapsed / bookDropDuration;

            // λ�ôӴ������������䵽Ŀ��λ��
            bookInstance.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            // ���Ŵӳ�ʼ���ű�����Ŀ�����ű���
            float scale = Mathf.Lerp(bookInitialScale, bookTargetScale, t);
            bookInstance.transform.localScale = Vector3.one * scale;

            // ʹ�鼮ʼ������ǰ���������Ը�����Ҫ������
            bookInstance.transform.forward = Vector3.forward;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // ȷ������λ�ú�����׼ȷ
        bookInstance.transform.position = targetPosition;
        bookInstance.transform.localScale = Vector3.one * bookTargetScale;

        // ����ٴ�ȷ���鼮������ȷ
        bookInstance.transform.forward = Vector3.forward;
    }
}
