using System;
using UnityEngine;

namespace DefaultNamespace
{
	[RequireComponent(typeof(PositionSaver))]
	public class ReplayMover : MonoBehaviour
	{
		private PositionSaver _save;

		private int _index;
		private PositionSaver.Data _prev;
		private float _duration;

		private void Start()
		{
			//todo comment: зачем нужны эти проверки?
			//Есть ли на этом  объекте компонент, а вторая проверяет не пуст ли список.
			if (!TryGetComponent(out _save) || _save.Records.Count == 0)
			{
				Debug.LogError("Records incorrect value", this);
				//todo comment: Для чего выключается этот компонент?
				//Предотвращает ненужные вычисления.
				enabled = false;
			}
		}

		private void Update()
		{
			var curr = _save.Records[_index];
			//todo comment: Что проверяет это условие (с какой целью)? 
			//Проверяет наступило ли время перехода к следующей записи.
			if (Time.time > curr.Time)
			{
				_prev = curr;
				_index++;
				//todo comment: Для чего нужна эта проверка?
				//Это для проверки достигли ли мы конца списка записей.
				if (_index >= _save.Records.Count)
				{
					enabled = false;
					Debug.Log($"<b>{name}</b> finished", this);
				}
			}
            //todo comment: Для чего производятся эти вычисления (как в дальнейшем они применяются)?
            //delta — это прогресс перехода между двумя точками во времени,
			//используемый для плавного движения между _prev.Position и curr.Position.
            var delta = (Time.time - _prev.Time) / (curr.Time - _prev.Time);
			//todo comment: Зачем нужна эта проверка?
			//Установка delta = 0 предотвращает ошибки.
			if (float.IsNaN(delta)) delta = 0f;
            //todo comment: Опишите, что происходит в этой строчке так подробно, насколько это возможно
            //Эта строка плавно перемещает объект из позиции _prev.Position в curr.Position на основе начения delta.
            transform.position = Vector3.Lerp(_prev.Position, curr.Position, delta);
		}
	}
}