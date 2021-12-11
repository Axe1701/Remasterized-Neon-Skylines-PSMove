using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ChangeScene : NetworkBehaviour
{
    [ClientRpc]
    public void PongClientRpc(string text)
    {
        Debug.Log(text);
    }

    public void LoadStartScene()
    {
        //SceneManager.LoadScene("Game");
        NetworkManager.singleton.ServerChangeScene("Game");
        //PongClientRpc("CAMBIO DE ESCENA");
    }

}
