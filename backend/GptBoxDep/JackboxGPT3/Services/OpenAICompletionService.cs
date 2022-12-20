using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenAI.GPT3;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.SharedModels;
using static JackboxGPT3.Services.ICompletionService;

namespace JackboxGPT3.Services
{
    // ReSharper disable once InconsistentNaming
    public class OpenAICompletionService : ICompletionService
    {
        private readonly IOpenAIService _api;

        /// <summary>
        /// Instantiate an <see cref="OpenAICompletionService"/> from the environment.
        /// </summary>
        public OpenAICompletionService(Services.IConfigurationProvider configuration)
        {
            var key = Environment.GetEnvironmentVariable("OPENAI_KEY") ?? configuration.OpenAIKey;
            // Console.WriteLine($"Instantiating OpenAICompletionService, {key}");
            _api = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = key,
                ApiVersion = "v1",
                BaseDomain = "https://api.openai.com"
            });
        }

        public async Task<CompletionResponse> CompletePrompt(
            string prompt,
            CompletionParameters completionParameters,
            Func<CompletionResponse, bool>? conditions = null,
            int maxTries = 5,
            string defaultResponse = ""
        ) {
            var result = new CompletionResponse();
            var validResponse = false;
            var tries = 0;

            while(!validResponse && tries < maxTries)
            {
                tries++;
                var apiResult = await _api.Completions.CreateCompletion(
                    new OpenAI.GPT3.ObjectModels.RequestModels.CompletionCreateRequest
                    {
                        Prompt = prompt,
                        MaxTokens = completionParameters.MaxTokens,
                        Temperature = completionParameters.Temperature,
                        TopP = completionParameters.TopP,
                        LogProbs = completionParameters.LogProbs,
                        Echo = completionParameters.Echo,
                        PresencePenalty = completionParameters.PresencePenalty,
                        FrequencyPenalty = completionParameters.FrequencyPenalty,
                        Stop = completionParameters.StopSequences
                    }
                );

                result = ChoiceToCompletionResponse(apiResult.Choices[0]);

                if (conditions == null) break;
                validResponse = conditions(result);
            }

            if (!validResponse)
                result = new CompletionResponse
                {
                    FinishReason = "no_valid_responses",
                    Text = defaultResponse
                };

            return result;
        }
        
        public async Task<T> CompletePrompt<T>(
            string prompt,
            CompletionParameters completionParameters,
            Func<CompletionResponse, T> process,
            T defaultResponse,
            Func<T, bool>? conditions = null,
            int maxTries = 5
        ) {
            var processedResult = defaultResponse;
            var validResponse = false;
            var tries = 0;

            while(!validResponse && tries < maxTries)
            {
                tries++;
                var apiResult = await _api.Completions.CreateCompletion(
                    new OpenAI.GPT3.ObjectModels.RequestModels.CompletionCreateRequest {
                        Prompt = prompt,
                        MaxTokens = completionParameters.MaxTokens,
                        Temperature = completionParameters.Temperature,
                        TopP = completionParameters.TopP,
                        LogProbs = completionParameters.LogProbs,
                        Echo = completionParameters.Echo,
                        PresencePenalty = completionParameters.PresencePenalty,
                        FrequencyPenalty = completionParameters.FrequencyPenalty,
                        Stop = completionParameters.StopSequences
                    }
                );

                var result = ChoiceToCompletionResponse(apiResult.Choices[0]);
                processedResult = process(result);

                if (conditions == null) break;
                validResponse = conditions(processedResult);
            }

            return processedResult;
        }

        private static CompletionResponse ChoiceToCompletionResponse(ChoiceResponse choice)
        {
            return new()
            {
                Text = choice.Text,
                FinishReason = choice.FinishReason
            };
        }

        public async Task<List<Double>?> Embed(string text)
        {
            var result = await _api.Embeddings.CreateEmbedding(new OpenAI.GPT3.ObjectModels.RequestModels.EmbeddingCreateRequest {
                Input = new List<string> {text},
                Model = "text-embedding-ada-002"
            });

            return result.Data[0].Embedding;
        }

        /// Code from:
        ///     https://stackoverflow.com/a/7562112
        /// Preforms a Cosine Similarity calculation on two lists of doubles, as shown to be needed here:
        ///     https://github.com/openai/openai-cookbook/blob/838f000935d9df03e75e181cbcea2e306850794b/examples/Semantic_text_search_using_embeddings.ipynb
        public static double GetCosineSimilarity(List<double> V1, List<double> V2)
        {
            int N = 0;
            N = ((V2.Count < V1.Count) ? V2.Count : V1.Count);
            double dot = 0.0d;
            double mag1 = 0.0d;
            double mag2 = 0.0d;
            for (int n = 0; n < N; n++)
            {
                dot += V1[n] * V2[n];
                mag1 += Math.Pow(V1[n], 2);
                mag2 += Math.Pow(V2[n], 2);
            }

            return dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2));
        }
        
        public async Task<List<ICompletionService.SearchResponse>> SemanticSearch(string query, IList<string> documents)
        {
            // The list of doubles that represent this embedding
            var search_embedding = await Embed(query);

            // Now, generate an enumberable of the embeddings for each document and the cosine similarity
            var list = documents.Select(async document => {
                var document_embedding = await Embed(document);
                return new ICompletionService.SearchResponse {
                    Index = documents.IndexOf(document),
                    Score = GetCosineSimilarity(search_embedding!, document_embedding!)
                };
            }).Select(task => task.Result).ToList();

            return list;
        }
    }
}
