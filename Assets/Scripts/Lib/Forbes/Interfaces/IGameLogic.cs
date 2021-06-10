using UnityEngine;

public interface IGameLogic
{
    void TargetKilled(GameObject target, GameObject by);
    void ItemPickedUp(GameObject item, GameObject by);
    void ItemSpawned(GameObject item);
    bool InteractWith(ICanInteract go);
}