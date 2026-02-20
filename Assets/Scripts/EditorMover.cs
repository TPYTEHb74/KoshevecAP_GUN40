using UnityEngine;

namespace DefaultNamespace
{
	
	[RequireComponent(typeof(PositionSaver))]
	public class EditorMover : MonoBehaviour
	{
		private PositionSaver _save;
		private float _currentDelay;

        //todo comment: Что произойдёт, если _delay > _duration?
        //Логика зависящая от этих значений может работать некорректно.
        
        [SerializeField, Range(0.2f, 1.0f)]
		private float _delay = 0.5f;
        
        [SerializeField, Min(0.2f)]
		private float _duration = 5f;

		private void Start()
		{
			//todo comment: Почему этот поиск производится здесь, а не в начале метода Update?
			//Что бы выполнилось при старте, в Update на сколько я помню выполняется каждый кадр.
			_save = GetComponent<PositionSaver>();
			_save.Records.Clear();

			if (_duration < _delay)
			{
				_duration = _delay * 5;
			}
		}

		private void Update()
		{
			_duration -= Time.deltaTime;
			if (_duration <= 0f)
			{
				enabled = false;
				Debug.Log($"<b>{name}</b> finished", this);
				return;
			}
			
			//todo comment: Почему не написать (_delay -= Time.deltaTime;) по аналогии с полем _duration?
			//Потому что если заменить то логика сломается.
			_currentDelay -= Time.deltaTime;
			if (_currentDelay <= 0f)
			{
				_currentDelay = _delay;
				_save.Records.Add(new PositionSaver.Data
				{
					Position = transform.position,
					//todo comment: Для чего сохраняется значение игрового времени?
					//Для отслеживания времени выполнения. 
					Time = Time.time,
				});
			}
		}
	}
}