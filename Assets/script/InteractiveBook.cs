using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveBook : MonoBehaviour
{
    public InteractableUnityEventWrapper eventWrapper;

    public static UnityEvent onGrabbed;
    public GameObject Dialogue;

    private void Awake()
    {
        // ȷ��InteractableUnityEventWrapper�Ѿ���ȷע����InteractableView
        eventWrapper.InjectInteractableView(GetComponent<IInteractableView>());

        // ����ѡ���¼�
        eventWrapper.WhenSelect.AddListener(HandleGrabbed);
    }

    private void HandleGrabbed()
    {
        // �����屻ץȡʱ����
        Debug.Log("Interactable was grabbed.");
        onGrabbed?.Invoke();
        Dialogue.SetActive(true);
    }
}