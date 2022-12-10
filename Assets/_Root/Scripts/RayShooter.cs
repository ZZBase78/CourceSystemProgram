using Mirror;
using System.Collections;
using UnityEngine;
public class RayShooter : FireAction
{
    [SyncVar] public int health = 100;
    private Camera _camera;
    protected override void Start()
    {
        base.Start();
        _camera = GetComponentInChildren<Camera>();
    }
    private void Update()
    {
        if (!isOwned) return;
        if (Input.GetMouseButtonDown(0))
        {
            Shooting();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reloading();
        }
        if (Input.anyKey && !Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    protected override void Shooting()
    {
        base.Shooting();
        if (bullets.Count > 0)
        {
            if (!reloading)
            {
                Shoot();
            }
        }
    }
    private void Shoot()
    {
        var shoot = bullets.Dequeue();
        BulletCount = bullets.Count.ToString();
        ammunition.Enqueue(shoot);

        CmdCallRayCalc();

        //shoot.SetActive(true);
        //shoot.transform.position = hit.point;
        //shoot.transform.parent = hit.transform;
        //yield return new WaitForSeconds(2.0f);
        //shoot.SetActive(false);
    }

    [Command]
    private void CmdCallRayCalc()
    {
        RayCalc();
    }

    [Server]
    private void RayCalc()
    {
        var point = new Vector3(_camera.pixelWidth / 2,
        _camera.pixelHeight / 2, 0);
        var ray = _camera.ScreenPointToRay(point);
        if (!Physics.Raycast(ray, out var hit)) return;
        PlayerCharacter playerCharacter = hit.collider.GetComponentInParent<PlayerCharacter>();
        if (playerCharacter != null)
        {
            //Debug.Log("Server health -");
            playerCharacter.health -= 10;
            if (playerCharacter.health <= 0)
            {
                playerCharacter.connectionToClient.Disconnect();
            }
        }
        
    }

}
