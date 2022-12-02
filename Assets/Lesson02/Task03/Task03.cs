using Unity.Collections;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UI;

namespace CourceSystemPogram.Lesson02.Task03
{
    internal sealed class Task03 : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _maxPositionRadius;
        [SerializeField] private int _arrayLenght;
        [SerializeField] private float _rotationSpeed;

        private TransformAccessArray _transformAccessArray;
        private Transform[] _transforms;
        private bool isRunning;

        private void Start()
        {
            _button.onClick.AddListener(Handle);
            isRunning = false;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
            if (isRunning)
            {
                _transformAccessArray.Dispose();
            }
            isRunning = false;
        }

        private void Handle()
        {
            if (isRunning)
            {
                _transformAccessArray.Dispose();
                DestroyTransformArrayFromScene();
                isRunning = false;
                return;
            }

            _transforms = new Transform[_arrayLenght];
            InstantiateObjects(_transforms);
            _transformAccessArray = new TransformAccessArray(_transforms);
            isRunning = true;
        }

        private void DestroyTransformArrayFromScene()
        {
            for (int i = 0; i < _transforms.Length; i++)
            {
                Destroy(_transforms[i].gameObject);
            }
        }

        private void Update()
        {
            if (!isRunning) return;

            MyJob myJob = new MyJob();
            myJob.rotation_speed = _rotationSpeed;
            myJob.deltaTime = Time.deltaTime;
            JobHandle handle = myJob.Schedule(_transformAccessArray);
            handle.Complete();
        }

        private void InstantiateObjects(Transform[] transforms)
        {
            for (int i = 0; i < transforms.Length; i++)
            {
                GameObject go = GameObject.Instantiate(_prefab, GetRandomPosition(), Random.rotation);
                transforms[i] = go.transform;
            }
        }

        private Vector3 GetRandomPosition()
        {
            Vector3 position = Random.insideUnitSphere * _maxPositionRadius;
            return position;
        }
    }

    internal struct MyJob : IJobParallelForTransform
    {
        [ReadOnly]
        public float deltaTime;
        [ReadOnly]
        public float rotation_speed;

        public void Execute(int index, TransformAccess transform)
        {
            Vector3 euler = transform.localRotation.eulerAngles;
            euler.y += rotation_speed * deltaTime;
            transform.localRotation = Quaternion.Euler(euler);
        }
    }
}
