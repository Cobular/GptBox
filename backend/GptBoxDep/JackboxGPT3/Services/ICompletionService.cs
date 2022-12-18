using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JackboxGPT3.Services
{
    public interface ICompletionService
    {
        public struct CompletionResponse
        {
            [JsonProperty("text")]
            public string Text;
            [JsonProperty("finish_reason")]
            public string FinishReason;
        }

        public struct CompletionParameters
        {
            [JsonProperty("max_tokens")]
            public int MaxTokens;
            [JsonProperty("temperature")]
            public float Temperature;
            [JsonProperty("top_p")]
            public float TopP;
            [JsonProperty("logprobs")]
            public int LogProbs;
            [JsonProperty("echo")]
            public bool Echo;
            [JsonProperty("presence_penalty")]
            public float PresencePenalty;
            [JsonProperty("frequency_penalty")]
            public float FrequencyPenalty;
            [JsonProperty("stop")]
            public string? StopSequences;
        }

        public struct SearchResponse
        {
            public int Index;
            public double Score;
        }

        public Task<CompletionResponse> CompletePrompt(
            string prompt,
            CompletionParameters completionParameters,
            Func<CompletionResponse, bool> conditions,
            int maxTries = 5,
            string defaultResponse = ""
        );
        
        public Task<T> CompletePrompt<T>(
            string prompt,
            CompletionParameters completionParameters,
            Func<CompletionResponse, T> process,
            T defaultResponse,
            Func<T, bool>? conditions = null,
            int maxTries = 5
        );

        public Task<List<SearchResponse>> SemanticSearch(
            string query,
            IList<string> documents
        );
    }
}