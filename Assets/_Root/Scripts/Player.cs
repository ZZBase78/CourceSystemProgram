using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    private GameObject playerCharacter;
    private void Start()
    {
        SpawnCharacter();
    }
    private void SpawnCharacter()
    {
        if (!isServer)
        {
            return;
        }
        playerCharacter = Instantiate(playerPrefab, transform.position, transform.rotation);
        NetworkServer.Spawn(playerCharacter,
        connectionToClient);
    }
}