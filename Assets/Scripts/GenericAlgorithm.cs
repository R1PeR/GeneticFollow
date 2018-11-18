using UnityEngine;
using System.Collections.Generic;
public class GenericAlgorithm : MonoBehaviour {
    public GameObject flappyBirdPrefab;
    public GameObject spawnPosition;
    public GameObject target;
    public int numberToSpawn;
    int generation = 0;
    //int j = 0;
    private void Start()
    {
        Time.timeScale = 1.0f;
        Generation.networks.Clear();
        Network n = new Network(new int[] { 8, 10, 10, 1 });
        for (int i = 0; i < numberToSpawn; i++)
        {
            SpawnFlappyBird(n);
        }
        InvokeRepeating("Routine", 10, 10);
    }
    void Routine()
    {
        target.transform.position = new Vector2(Random.Range(-6f, 6f), Random.Range(-3f, 3f));
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Agent"))
        {
            Generation.networks.Add(g.GetComponent<Agent>().GetNetwork());
            Destroy(g);
        }
        Generation.Sort();
        Generation.networks.RemoveRange(0,Generation.networks.Count/2);
        Generation.networks.AddRange(Generation.networks);
        Generation.Sort();
        for (int i = 0; i < Generation.networks.Count/2; i++)
        {
            Generation.networks[i].Mutate(Generation.networks.Count / (i + 1));
        }
        foreach (Network n in Generation.networks)
        {
            SpawnFlappyBird(n);
        }
        Generation.networks.Clear();
        Debug.Log("Generation: " + generation++);
    }
    static float CheckRay(Transform transform,Vector3 vector)
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(vector*100));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(vector), 100f);
        // Cast a ray straight downwards.
        if (hit.collider != null)
        {
            return hit.distance;
        }
        return 0;
    }
    public static void NetworkRoutine (Network network, Agent fs, Transform transform){
        float[] result = network.FeedForward(new float[] {
            CheckRay(transform, Vector3.up),
            CheckRay(transform, new Vector3(0.5f,0.5f,0)),
            CheckRay(transform, Vector3.right),
            CheckRay(transform, new Vector3(0.5f,-0.5f,0)),
            CheckRay(transform, Vector3.down),
            CheckRay(transform, new Vector3(-0.5f,-0.5f,0)),
            CheckRay(transform, Vector3.left),
            CheckRay(transform, new Vector3(-0.5f,0.5f,0))
        });
        fs.AddRotation(result[0]*3600*Time.deltaTime);
    }
    void SpawnFlappyBird(int n) {
        for (int i = 0; i < n; i++)
        {
            Instantiate(flappyBirdPrefab, spawnPosition.transform.position, spawnPosition.transform.rotation, null);
        }
    }   
    GameObject SpawnFlappyBird(Network net)
    {
        GameObject _temp;
        _temp = Instantiate(flappyBirdPrefab, spawnPosition.transform.position, spawnPosition.transform.rotation, null);
        _temp.GetComponent<Agent>().network = new Network(net);
        return _temp;
    }
}