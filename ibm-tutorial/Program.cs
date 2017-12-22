using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ibm_tutorial
{
	class Program
	{
		private static string _sourceL;
		private static string _targetL;
		private static string _username = "f0627e26-ae68-436f-8c7f-e1253f3143ed";
		private static string _password = "2K6NgruI75dd";

		static void Main()
		{
			// Профиль API
			Console.Write("Enter username (leave empty to use default): ");
			var username = Console.ReadLine();
			if(!String.IsNullOrWhiteSpace(username))
			{
				_username = username;
				Console.Write("Enter password: ");
				_password = Console.ReadLine();
			}

			// Языки
			Console.Write("Enter source language: ");
			_sourceL = Console.ReadLine();
			Console.Write("Enter target language: ");
			_targetL = Console.ReadLine();

			// Подготовка
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			var byteArray = Encoding.ASCII.GetBytes(_username + ":" + _password);
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

			while (true)
			{
				// Исходный текст
				Console.Write("Enter source text: ");
				string textToTranslate = Console.ReadLine();

				// Отправка запроса
				var response = client.PostAsJsonAsync(@"https://gateway.watsonplatform.net/language-translator/api/v2/translate", new Input
				{
					text = textToTranslate,
					model_id = $"{_sourceL}-{_targetL}-conversational"
				}).Result.Content;

				// Обработка ответа
				JObject tr = JObject.Parse(response.ReadAsStringAsync().Result);

				StringBuilder sb = new StringBuilder();

				foreach (var t in tr.First)
				{
					sb.Append(t[0]["translation"].Value<string>());
					sb.Append(", ");
				}

				// Вывод ответаы
				Console.WriteLine("Translated text: " + sb.ToString(0, sb.Length - 2));
			}
		}
	}

	class Input
	{
		public string text;
		public string model_id;
	}
}
