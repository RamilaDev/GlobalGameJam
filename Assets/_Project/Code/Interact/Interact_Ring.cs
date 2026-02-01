using UnityEngine;

public class Interact_Ring : MonoBehaviour
{
    public void Click()
    {
        Debug.Log("[Interact_Rign] Chin");

        GetComponent<Animator>().SetTrigger("t");
        GetComponent<AudioSource>().Play();
        if(Game_Manager.Instance.Clients[0].End == 0) return;
        Game_Manager.Instance.Clients.RemoveAt(0);
    }
}
