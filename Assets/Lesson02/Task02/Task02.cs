using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

namespace CourceSystemPogram.Lesson02.Task02
{
    internal sealed class Task02 : MonoBehaviour
    {
        private const int MAX_POSITION_RADIUS = 10;
        private const int MAX_VELOCITY_RADIUS = 10;
        private const int ARRAY_LENGTH = 10;

        [SerializeField] Button _button;

        private void Start()
        {
            _button.onClick.AddListener(Handle);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void FillArrayRandom(NativeArray<Vector3> array, int maxValue)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Random.insideUnitSphere * maxValue;
            }
        }

        private void PrintArray(NativeArray<Vector3> positions, NativeArray<Vector3> velocities, NativeArray<Vector3> finalPositions)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                Debug.Log($"index {i}; position {positions[i]}; velocity {velocities[i]}; final {finalPositions[i]}");
            }
        }

        private void Handle()
        {
            NativeArray<Vector3> positions = new NativeArray<Vector3>(ARRAY_LENGTH, Allocator.Persistent);
            NativeArray<Vector3> velocities = new NativeArray<Vector3>(ARRAY_LENGTH, Allocator.Persistent);
            NativeArray<Vector3> finalPositions = new NativeArray<Vector3>(ARRAY_LENGTH, Allocator.Persistent);
            FillArrayRandom(positions, MAX_POSITION_RADIUS);
            FillArrayRandom(velocities, MAX_VELOCITY_RADIUS);

            Debug.Log("Start job...");
            MyJob job = new MyJob();
            job.positions = positions;
            job.velocities = velocities;
            job.finalPositions = finalPositions;
            JobHandle handle = job.Schedule(ARRAY_LENGTH, 0);
            handle.Complete();
            Debug.Log("Job Complete.");

            PrintArray(positions, velocities, finalPositions);

            positions.Dispose();
            velocities.Dispose();
            finalPositions.Dispose();
        }
    }

    internal struct MyJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Vector3> positions;
        [ReadOnly]
        public NativeArray<Vector3> velocities;
        [WriteOnly]
        public NativeArray<Vector3> finalPositions;

        public void Execute(int index)
        {
            finalPositions[index] = positions[index] + velocities[index];
        }
    }
}
