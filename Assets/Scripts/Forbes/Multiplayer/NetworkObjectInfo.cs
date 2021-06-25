using UnityEngine;
using MLAPI;

public class NetworkObjectInfo : NetworkBehaviour
{
    void OnGUI()
    {
        NetworkObject no = GetComponent<NetworkObject>();
        if (no == null)
            return;

        Vector3 pos = new Vector3(10, 60, 0);
        pos = Camera.main.WorldToScreenPoint(transform.position);

        GUI.color = Color.black;
        GUI.Label(new Rect(pos.x, Screen.height - pos.y + 30, 500, 30), "IsLocalPlayer: " + no.IsLocalPlayer);
        GUI.Label(new Rect(pos.x, Screen.height - pos.y + 45, 500, 30), "IsPlayerObject: " + no.IsPlayerObject);
        GUI.Label(new Rect(pos.x, Screen.height - pos.y + 60, 500, 30), "IsOwnedByServer: " + no.IsOwnedByServer);
        GUI.Label(new Rect(pos.x, Screen.height - pos.y + 75, 500, 30), "IsOwner: " + no.IsOwner);
        GUI.Label(new Rect(pos.x, Screen.height - pos.y + 90, 500, 30), "OwnerClientId: " + no.OwnerClientId);
        GUI.Label(new Rect(pos.x, Screen.height - pos.y + 105, 500, 30), "NetworkObjectId: " + no.NetworkObjectId);
    }
}