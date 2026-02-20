using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static DefaultNamespace.PositionSaver;

namespace DefaultNamespace
{
    [System.Serializable]
    public class PositionSaver : MonoBehaviour
	{
        [System.Serializable]
        public struct Data
		{
			public Vector3 Position;
			public float Time;
		}

        [SerializeField, HideInInspector, TextArea(3, 10)]
        [Tooltip("Чтобы заполнить это поле, воспользуйтесь контекстным меню инспектора" +
			     " и выберите команду  'Create File' ")]
        private TextAsset _json;



		[field: SerializeField, HideInInspector]
		public List<Data> Records { get; set; } = default!;

		private void Awake()
		{
			//todo comment: Что будет, если в теле этого условия не сделать выход из метода?
			//Если не сделать выход то будет ошибка NullReferenceException.
			if (_json == null)
			{
				gameObject.SetActive(false);
				Debug.LogError("Please, create TextAsset and add in field _json");
				return;
			}

			JsonUtility.FromJsonOverwrite(_json.text, this);
			//todo comment: Для чего нужна эта проверка (что она позволяет избежать)?
			//Защищает от ошибок десериализации.
			if (Records == null)
				Records = new List<Data>(10);
		}

		private void OnDrawGizmos()
		{
			//todo comment: Зачем нужны эти проверки (что они позволляют избежать)?
			//Предотвращают зависание и вылет редактора.
			if (Records == null || Records.Count == 0) return;
			var data = Records;
			var prev = data[0].Position;
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(prev, 0.3f);
			//todo comment: Почему итерация начинается не с нулевого элемента?
			//Для оптимизации отрисовки линий между точками.
			for (int i = 1; i < data.Count; i++)
			{
				var curr = data[i].Position;
				Gizmos.DrawWireSphere(curr, 0.3f);
				Gizmos.DrawLine(prev, curr);
				prev = curr;
			}
		}

#if UNITY_EDITOR
		[ContextMenu("Create File")]
		private void CreateFile()
		{
			//todo comment: Что происходит в этой строке?
			//Создается файл Path.txt в папке Assets.
			var stream = File.Create(Path.Combine(Application.dataPath, "Path.txt"));
			//todo comment: Подумайте для чего нужна эта строка? (а потом проверьте догадку, закомментировав) 
			//Чтобы избежать блокировки файла.
			stream.Dispose();
			UnityEditor.AssetDatabase.Refresh();
			//В Unity можно искать объекты по их типу, для этого используется префикс "t:"
			//После нахождения, Юнити возвращает массив гуидов (которые в мета-файлах задаются, например)
			var guids = UnityEditor.AssetDatabase.FindAssets("t:TextAsset");
			foreach (var guid in guids)
			{
				//Этой командой можно получить путь к ассету через его гуид
				var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
				//Этой командой можно загрузить сам ассет
				var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(path);
				//todo comment: Для чего нужны эти проверки?
				//Первая проверяет что ассет действительно загружен а вторая ищет именно текстовый ассет с именем "Path" 
				if(asset != null && asset.name == "Path")
				{
					_json = asset;
					UnityEditor.EditorUtility.SetDirty(this);
					UnityEditor.AssetDatabase.SaveAssets();
					UnityEditor.AssetDatabase.Refresh();
					//todo comment: Почему мы здесь выходим, а не продолжаем итерироваться?
					//Как только он найден поиск не нужен, это для оптимизации. 
					return;
				}
			}
		}

		private void OnDestroy()
        {
            if (_json != null && Records != null)
            {
                string json = JsonUtility.ToJson(this, true);
                string path = UnityEditor.AssetDatabase.GetAssetPath(_json);
                System.IO.File.WriteAllText(path, json);
                UnityEditor.AssetDatabase.Refresh();
            }
        }
#endif
	}
}
