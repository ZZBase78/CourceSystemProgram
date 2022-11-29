using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CourseSystemProgram.Lesson01.Task03
{
    public class Task03Script : MonoBehaviour
    {
        [SerializeField] Button _button;

        private CancellationTokenSource _cancelTokenSource;

        private void Start()
        {
            _button.onClick.AddListener(RunTasks);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
            FreeCancellationTokenSource();
        }

        private void FreeCancellationTokenSource()
        {
            _cancelTokenSource?.Cancel();
            _cancelTokenSource?.Dispose();
        }

        private async void RunTasks()
        {
            FreeCancellationTokenSource();
            _cancelTokenSource = new CancellationTokenSource();
            Task task1 = Task01Example(_cancelTokenSource.Token);
            Task task2 = Task02Example(_cancelTokenSource.Token, 60);

            bool result = await WhatTaskFasterAsync(_cancelTokenSource, task1, task2);
            Debug.Log(result);
        }

        private async Task Task01Example(CancellationToken cancellationToken)
        {
            await Task.Delay(1000, cancellationToken);
        }

        private async Task Task02Example(CancellationToken cancellationToken, int frameCount)
        {
            for (int i = 0; i < frameCount; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                await Task.Yield();
            }
        }

        public static async Task<bool> WhatTaskFasterAsync(CancellationTokenSource cts, Task task1, Task task2)
        {
            var taskResult = await Task.WhenAny(task1, task2);
            if (cts.Token.IsCancellationRequested)
            {
                Debug.Log("Cancel requested.");
                return false;
            }

            cts.Cancel();

            if (taskResult == task1)
            {
                Debug.Log("Task 01 completed.");
                return true;
            }
            else 
            {
                Debug.Log("Task 02 completed.");
                return false;
            }
        }
    }
}
