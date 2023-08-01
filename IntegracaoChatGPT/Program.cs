using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChatGPTExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Insira sua chave de API aqui
            string apiKey = "";

            // Criação do cliente HttpClient com a autorização
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);

            // Função para enviar mensagens para a API do Chat GPT
            async Task<string> SendMessageAsync(string message)
            {
                string apiUrl = "https://api.openai.com/v1/chat/completions";

                var requestContent = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                        new { role = "system", content = "You are a helpful assistant." },
                        new { role = "user", content = message }
                    }
                };

                var requestJson = JsonConvert.SerializeObject(requestContent);
                var requestContentString = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(apiUrl, requestContentString);

                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic responseJson = JsonConvert.DeserializeObject(responseContent);

                string reply = responseJson.choices[0].message.content;
                return reply;
            }

            Console.WriteLine("Bem-vindo ao ChatGPT!");

            // Loop de interação com o usuário
            while (true)
            {
                Console.Write("Você: ");
                string userInput = Console.ReadLine();

                if (userInput.ToLower() == "sair")
                {
                    Console.WriteLine("Obrigado por utilizar o ChatGPT. Até logo!");
                    break;
                }

                string response = await SendMessageAsync(userInput);

                Console.WriteLine("ChatGPT: " + response);
                Console.WriteLine();
            }
        }
    }
}