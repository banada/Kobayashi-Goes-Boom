using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
	public GameObject Ship;
	public int spawn_radius = 20;

	public string type = "server";
	// Use this for initialization
	void Start () {
		if (type == "server") {
			StartServer ();
		} else {
			RefreshHostList();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (refreshing) {
			JoinServer();
			refreshing=false;
		}
	}

	//HOST STUFF
	private bool refreshing = false;
	private const string typeName = "KobyashiGoesBoom";
	private const string gameName = "RoomName";
	
	void StartServer()
	{
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
	}

	void OnServerInitialized()
	{
		Debug.Log("Server Initializied");
		SpawnPlayer ();
	}

	//CLIENT STUFF
	private HostData[] hostList;
	
	void RefreshHostList()
	{
		refreshing = true;

	}

	void JoinServer()
	{
		Network.Connect("192.168.1.107",25000);
	}
	
	void OnConnectedToServer()
	{
		Debug.Log("Server Joined");
		SpawnPlayer ();
	}

	public void SpawnPlayer()
	{
		int x = Random.Range (-spawn_radius, spawn_radius);
		int y = Random.Range (-spawn_radius, spawn_radius);
		int z = 0;

		Network.Instantiate(Ship, new Vector3 (x, y, z), Quaternion.identity, 0);
	}
}