using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
public class WatchGrab : MonoBehaviour
{
    [SerializeField] private Collider col;
    [SerializeField] private Collider col2;
    [SerializeField] private Rigidbody rb;
    private XRGrabInteractable interactable;

    Transform currentSocket = null;
    Transform lastSocket = null;

    [SerializeField] private XRSocketInteractor defaultSocket;
    [SerializeField] private GameObject socketVisuals;
    [SerializeField] private float returnDelay = 3f;
    [SerializeField] private float blinkInterval = 0.5f;

    private float timeSinceDropped = 0f;
    private bool start = false;
    private float blinkTimer = 0f;

    private Animator a;
    private void Start()
    {
        a = GetComponent<Animator>();
        interactable = GetComponent<XRGrabInteractable>();
        if (defaultSocket != null)
        {
            lastSocket = defaultSocket.transform;
            var weaponID = lastSocket.gameObject.GetComponentInChildren<WeaponID>(true);
            if (weaponID != null) socketVisuals = weaponID.gameObject;
        }
    }
    private void Update()
    {
        if (!start && interactable.isSelected) start = true;

        if (col != null && col2 != null)
        {
            col.enabled = GameManager.Instance.GunsGrabbed() <= 0 || currentSocket == null;
            col2.enabled = GameManager.Instance.GunsGrabbed() <= 0 || currentSocket == null;
        }

        if (currentSocket == null && !interactable.isSelected && start)
        {
            timeSinceDropped += Time.deltaTime;

            blinkTimer += Time.deltaTime;
            if (blinkTimer >= blinkInterval)
            {
                blinkTimer = 0f;
                if (socketVisuals != null) socketVisuals.SetActive(!socketVisuals.activeSelf);
            }

            if (timeSinceDropped >= returnDelay)
            {
                ReturnToLastSocket();
            }
        }
        else
        {
            timeSinceDropped = 0f;
            if (socketVisuals != null) socketVisuals.SetActive(false);
        }
    }
    public void EnterSocket(SelectEnterEventArgs m)
    {
        currentSocket = m.interactorObject.transform;

        var weaponID = currentSocket.gameObject.GetComponentInChildren<WeaponID>(true);
        if (weaponID != null) socketVisuals = weaponID.gameObject;

        lastSocket = currentSocket;
        rb.isKinematic = true;
    }
    public void LeaveSocket()
    {
        currentSocket = null;
        rb.isKinematic = false;
    }

    private void ReturnToLastSocket()
    {
        XRSocketInteractor socket = GetActiveSocket();

        if (!socket.hasSelection && socket != null)
        {
            var manager = interactable.interactionManager;
            manager.SelectEnter(socket, (IXRSelectInteractable)interactable);
            timeSinceDropped = 0f;
        }
    }
   
    private XRSocketInteractor GetActiveSocket()
    {
        if (lastSocket != null) return lastSocket.GetComponent<XRSocketInteractor>();
        return defaultSocket;
    }

    public void GO()
    {
        ReturnToLastSocket();
        if (a) a.SetBool("On", true);
    }
}
