using Mirror;
using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class Character : NetworkBehaviour
{
    protected Action OnUpdateAction { get; set; }
    protected abstract FireAction fireAction { get; set; }
    [SyncVar] public Vector3 serverPosition;

    protected virtual void Initiate()
    {
        OnUpdateAction += Movement;
    }
    private void Update()
    {
        OnUpdate();
    }
    private void OnUpdate()
    {
        OnUpdateAction?.Invoke();
    }
    [Command]
    protected void CmdUpdatePosition(Vector3 position)
    {
        //Debug.Log("serverPosition:" + serverPosition);
        //Debug.Log("position:" + position);
        serverPosition = position;
    }
    public abstract void Movement();
}
