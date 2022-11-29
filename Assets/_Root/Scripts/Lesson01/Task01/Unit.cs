using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CourseSystemProgram.Lesson01.Task01
{
    internal sealed class Unit : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private int _health;
        private Coroutine _coroutine;

        private void Awake()
        {
            _health = 0;
            _coroutine = null;
            _button.onClick.AddListener(ReceiveHealing);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void ReceiveHealing()
        {
            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(HealingCoroutine(3f, 0.5f, 5, 100));
            }
            
        }

        private IEnumerator HealingCoroutine(float healingTime, float delayTime, int IncHealth, int maxHealth)
        {
            float passedTime = 0;

            while (passedTime < healingTime)
            {
                yield return new WaitForSeconds(delayTime);
                passedTime += delayTime;

                _health += IncHealth;
                if (_health > maxHealth)
                {
                    _health= maxHealth;
                    break;
                }
            }
            _coroutine = null;
            Debug.Log(_health);
        }
    }
}

