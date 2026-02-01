using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Game_Outline))]
public class Interact_Trigger : MonoBehaviour
{
    public UnityEvent onClick;

    public Renderer Mesh;

    private Game_Outline outline;
    private Material[] originalMaterials;

    void Awake()
    {
        outline = GetComponent<Game_Outline>();
        outline.enabled = false;
        outline.OutlineWidth = 10;

        // Guardamos los materiales originales
        originalMaterials = Mesh.materials;
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        yield return new WaitForNextFrameUnit();
        outline.enabled = true;
        yield return new WaitForNextFrameUnit();
        outline.enabled = false;
    }

    public void SetOutline()
    {
        outline.enabled = true;
        Debug.Log("[Interact_Trigger] Set Outline");
    }

    public void QuitOutline()
    {
        outline.enabled = false;

        // Restauramos materiales originales
        Mesh.materials = originalMaterials;

        Debug.Log("[Interact_Trigger] Quit Outline");
    }
}
