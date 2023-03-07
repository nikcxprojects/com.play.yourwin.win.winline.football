using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _asteroidsPrefab;

    private void Start()
    {
        if(Random.value >= 0.5f)
            SpawnBall();
        else
            Invoke(nameof(SpawnBall), Random.Range(5.0f, 10.0f));

    }

    private void SpawnBall()
    {
        if (GameManager.instance.isEnded)
            return;

        var obj = Instantiate(_asteroidsPrefab);
        obj.transform.position = transform.position;

        Invoke(nameof(SpawnBall), Random.Range(5.0f, 15.0f));
    }
}
