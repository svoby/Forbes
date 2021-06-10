using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject Spawn(GameObject go, Vector3 pos, Quaternion rot)
	{
		GameObject clone = go;

		if (!go.scene.isLoaded)
			clone = Instantiate(go, pos, rot);

		foreach (var c in clone.GetComponents<ISpawn>())
			c.Spawn();

		go.SetActive(true);
		return clone;
	}
	public RespawnSelf Spawn(RespawnSelf self)
	{
		foreach (var c in self.GetComponents<ISpawn>())
			c.Spawn();

		self.gameObject.SetActive(true);
		return self;
	}

	public void Despawn(RespawnSelf o)
	{
		o.Despawn();
		o.gameObject.SetActive(false);
	}

	public void Despawn(GameObject go, bool kill = false)
	{
		foreach (var c in go.GetComponents<ISpawn>())
			c.Despawn();

		go.SetActive(false);

		if (kill)
			Destroy(go);
	}
}