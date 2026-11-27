using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pipePrefab;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float heightOffset = 0.8f;
    [SerializeField] private int poolSize = 10;

    private float timer;
    private Camera mainCamera;
    private Queue<PipeController> pipePool = new Queue<PipeController>();
    private List<PipeController> allPipes = new List<PipeController>();

    private void Awake()
    {
        mainCamera = Camera.main;
        SetSpawnPosition();
        CreatePool();
    }
    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.Playing) return;
        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            timer = 0f;
            SpawnPipe();
        }
    }
    private void SetSpawnPosition()
    {
        float screenright = mainCamera.orthographicSize * mainCamera.aspect;
        float spawnX = screenright + 0.5f;
        Vector3 pos = transform.position;
        pos.x = spawnX;

        transform.position = pos;
    }
    private void CreatePool()
    {
        for(int i=0; i < poolSize; i++)
        {
            GameObject pipeObj = Instantiate(pipePrefab);
            pipeObj.SetActive(false);

            PipeController pipe = pipeObj.GetComponent<PipeController>();
            pipe.Initialize(this);
            pipePool.Enqueue(pipe);
            allPipes.Add(pipe);
        }
    }
    private void SpawnPipe()
    {
        if (pipePool.Count == 0)
        {
            Debug.LogWarning("No pipes available in the pool!");
            return;
        }
        PipeController pipe = pipePool.Dequeue();
        float randomY = Random.Range(-heightOffset, heightOffset);
        pipe.transform.position = transform.position + new Vector3(0, randomY, 0);

        pipe.gameObject.SetActive(true);
    }
    public void ReturnToPool(PipeController pipe)
    {
        pipe.gameObject.SetActive(false);
        pipePool.Enqueue(pipe);
    }
    public void ResetSpawners()
    {
        timer = 0f;
        pipePool.Clear();

        foreach(PipeController pipe in allPipes)
        {
            pipe.gameObject.SetActive(false);
            pipePool.Enqueue(pipe);
        }
    }

}
