using System.Collections;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    [Header("Portal Settings")]
    public float appearDuration = 1.0f;        // ���������ֵĳ���ʱ��
    public float rotationSpeed = 90f;          // ��ת�ٶȣ���/�룩
    public Vector3 targetScale = new Vector3(1.2f, 1.2f, 1.2f); // Ŀ��Ŵ����

    private Vector3 initialScale;
    private bool isRotating = false;

    [Header("Portal Surface")]
    public string portalSurfaceName = "PortalSurface_low"; // �Ӷ�������
    private Transform portalSurface; // �Ӷ����Transform

    void Start()
    {
        StartCoroutine(PortalSequence());
    }

    IEnumerator PortalSequence()
    {
        // 1. ��ʼ��������Ϊ���ɼ�������Ϊ0��
        initialScale = Vector3.zero;
        transform.localScale = initialScale;

        // 2. �����Ӷ���PortalSurface_low
        portalSurface = transform.Find(portalSurfaceName);
        if (portalSurface == null)
        {
            Debug.LogError($"�Ӷ��� '{portalSurfaceName}' δ�ҵ�����ȷ���������ڴ�����Prefab�С�");
            yield break;
        }

        // 3. ����������
        yield return StartCoroutine(ScaleOverTime(initialScale, Vector3.one, appearDuration));

        // 4. ��ʼ��ת�ͷŴ�
        isRotating = true;
        StartCoroutine(RotatePortal());

        // 5. �Ŵ�Ŀ���ģ
        yield return StartCoroutine(ScaleOverTime(Vector3.one, targetScale, 1.0f));

        // �����ű�����Ŀ���ģ���ȴ��ⲿָ��������� BookSpawner��
    }

    IEnumerator ScaleOverTime(Vector3 fromScale, Vector3 toScale, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(fromScale, toScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = toScale;
    }

    IEnumerator RotatePortal()
    {
        while (isRotating && portalSurface != null)
        {
            // ȷ��ʹ�� Space.Self ������������ת
            portalSurface.Rotate(0, 0, rotationSpeed * Time.deltaTime, Space.Self);
            yield return null;
        }
    }

    // �����������������ش�����
    public void HidePortal()
    {
        StartCoroutine(HidePortalSequence());
    }

    IEnumerator HidePortalSequence()
    {
        // ֹͣ��ת
        isRotating = false;

        // ��С������
        yield return StartCoroutine(ScaleOverTime(transform.localScale, Vector3.zero, 1.0f));

        // ���ٴ����Ŷ���
        Destroy(gameObject);
    }
}
