using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CourseSystemProgram.Lesson01.Task02
{
    public class Task02Script : MonoBehaviour
    {
        [SerializeField] Button _button;

        private CancellationTokenSource _cancelTokenSource;

        private void Awake()
        {
            _cancelTokenSource = new CancellationTokenSource();
        }
        private void Start()
        {
            _button.onClick.AddListener(RunTasks);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
            _cancelTokenSource.Cancel();
            _cancelTokenSource.Dispose();
        }

        private void RunTasks()
        {
            Task01Example(_cancelTokenSource.Token);
            Task02Example(_cancelTokenSource.Token, 60);
        }

        private async void Task01Example(CancellationToken cancellationToken)
        {
            await Task.Delay(1000, cancellationToken);
            Debug.Log("Task 01 completed.");
        }

        private async void Task02Example(CancellationToken cancellationToken, int frameCount)
        {
            for (int i = 0; i < frameCount; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                await Task.Yield();
            }
            Debug.Log("Task 02 completed.");
        }
    }
}
