using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

namespace CourceSystemPogram.Lesson02.Task01
{
    internal sealed class Task01 : MonoBehaviour
    {
        private const int MIN_ARRAY_VALUE = 1;
        private const int MAX_ARRAY_VALUE = 20;
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

        private void FillArrayRandom(NativeArray<int> array, int minValue, int maxValue)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Random.Range(minValue, maxValue);
            }
        }

        private void PrintArray(NativeArray<int> array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Debug.Log($"index {i}; value {array[i]}");
            }
        }

        private void Handle()
        {
            NativeArray<int> array = new NativeArray<int>(ARRAY_LENGTH, Allocator.Persistent);
            FillArrayRandom(array, MIN_ARRAY_VALUE, MAX_ARRAY_VALUE);

            PrintArray(array);

            Debug.Log("Start job...");
            MyJob job = new MyJob();
            job.maxValue = 10;
            job.array = array;
            JobHandle handle = job.Schedule();
            handle.Complete();
            Debug.Log("Job Complete.");

            PrintArray(array);

            array.Dispose();
        }
    }

    internal struct MyJob : IJob
    {
        public int maxValue;
        public NativeArray<int> array;

        public void Execute()
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > maxValue) array[i] = 0;
            }
        }
    }
}
