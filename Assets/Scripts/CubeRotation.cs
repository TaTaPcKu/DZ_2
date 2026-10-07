using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CubeRotation : MonoBehaviour
{
    [SerializeField] GameObject CubePrefab;

    [SerializeField, Range(0.1f, 50f)] private float radius = 20;
    [SerializeField, Range(0f, 360f)] private float angularSpeedInDegree = 30;
    [SerializeField, Range(0, 64)] private int cubeCount = 8;
    [SerializeField] private bool isClockwise = true;
    private List<GameObject> cubes = new List<GameObject>();
    private float currentRadius;

    //Не знаю, может нужно было выделить в отдельный монобех
    void SpawnCubes()
    {
        var angleStep = 2 * Mathf.PI / cubeCount;
        var angleInRadians = 0f;
        Vector3 rotateVector;
        GameObject cube;

        if (cubeCount == 0)
        {
            Debug.Log("Указанное количество кубов равно нулю!");
            return;
        }

        DestroyCubes();

        for (int i = 0; i < cubeCount; ++i)
        {
            cube = Instantiate(CubePrefab, transform);
            rotateVector = new Vector3(radius * Mathf.Cos(angleInRadians),
                                       radius * Mathf.Sin(angleInRadians),
                                       0);
            cube.transform.localPosition = rotateVector;
            cube.transform.localRotation = Quaternion.LookRotation(rotateVector);

            cubes.Add(cube);
            angleInRadians += angleStep;
        }

        currentRadius = radius;
    }

    private void DestroyCubes()
    {
        foreach(var cube in cubes)
        {
            Destroy(cube);
        }
        cubes.Clear();
    }

    private void Rotatecubes()
    {
        var rotation = (isClockwise ? -1 : 1) * Time.fixedDeltaTime * angularSpeedInDegree;
        foreach (var cube in cubes)
        {
            cube.transform.RotateAround(transform.position, transform.forward, rotation);
        }
    }

    private void Applyradius()
    {
        foreach (var cube in cubes)
        {
            cube.transform.localPosition = cube.transform.localPosition.normalized * radius;
        }

        currentRadius = radius;
    }

    private void UpdateCubes()
    {
        if (cubeCount != cubes.Count)
        {
            SpawnCubes();
        }

        if (!Mathf.Approximately(currentRadius, radius))
        {
            Applyradius();
        }

        Rotatecubes();
    }

    private void Awake()
    {
        SpawnCubes();
    }

    private void FixedUpdate()
    {
        UpdateCubes();
    }
}
