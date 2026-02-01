using UnityEngine;

public class Player_Interact : MonoBehaviour
{
    [SerializeField] private Transform cam_target;
    [SerializeField] private float interactDistance = 5f;

    public Interact_Trigger selectedTrigger { get; private set; }

    private RaycastHit hit;
    private bool detected_hit;

    void Update()
    {
        // ---- RAYCAST (UNO SOLO) ----
        detected_hit = Physics.Raycast(
            cam_target.position,
            cam_target.forward,
            out hit,
            interactDistance
        );

        Debug_Ray();

        // ---- NO HIT ----
        if (!detected_hit)
        {
            ClearSelection();
            return;
        }

        // ---- TIENE HIT PERO NO ES INTERACTUABLE ----
        Interact_Trigger trigger = hit.transform.GetComponent<Interact_Trigger>();

        if (trigger == null)
        {
            ClearSelection();
            return;
        }

        // ---- CAMBIO DE OBJETO MIRADO ----
        if (selectedTrigger != null && selectedTrigger != trigger)
        {
            ClearSelection();
        }

        // ---- SELECCIÓN ----
        trigger.SetOutline();
        selectedTrigger = trigger;

        // ---- CLICK ----
        if (Input.GetMouseButtonDown(0))
        {
            trigger.onClick?.Invoke();
        }
    }

    void ClearSelection()
    {
        if (selectedTrigger != null)
        {
            selectedTrigger.QuitOutline();
            selectedTrigger = null;
        }
    }

    void Debug_Ray()
    {
        if (detected_hit)
        {
            Debug.DrawLine(cam_target.position, hit.point, Color.red);
        }
        else
        {
            Debug.DrawRay(cam_target.position, cam_target.forward * interactDistance, Color.white);
        }
    }
}
