using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac;
using JackboxGPT3.Engines;
using JackboxGPT3.Games.BlatherRound;
using JackboxGPT3.Games.Common;
using JackboxGPT3.Games.Common.Models;
using JackboxGPT3.Games.Fibbage3;
using JackboxGPT3.Games.Quiplash3;
using JackboxGPT3.Games.SurviveTheInternet;
using JackboxGPT3.Games.WordSpud;
using JackboxGPT3.Services;
using Newtonsoft.Json;

namespace JackboxGPT3
{
    public static class Startup
    {
        private static readonly HttpClient _httpClient = new();

        public static IContainer InternalSetup(Services.IConfigurationProvider configuration, ILogger<IGptBoxDependency> logger) {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(configuration).As<Services.IConfigurationProvider>();
            builder.RegisterType<OpenAICompletionService>().As<ICompletionService>();
            builder.RegisterInstance<ILogger>(logger).SingleInstance();

            builder.RegisterGameEngines();

            return builder.Build();
        }

        private static void RegisterGameEngines(this ContainerBuilder builder)
        {
            // Game engines, keyed with appTag
            builder.RegisterType<Fibbage3Client>();
            builder.RegisterType<Fibbage3Engine>().Keyed<IJackboxEngine>("fibbage3");

            builder.RegisterType<Quiplash3Client>();
            builder.RegisterType<Quiplash3Engine>().Keyed<IJackboxEngine>("quiplash3");
            
            builder.RegisterType<WordSpudClient>();
            builder.RegisterType<WordSpudEngine>().Keyed<IJackboxEngine>("wordspud");
            
            builder.RegisterType<SurviveTheInternetClient>();
            builder.RegisterType<SurviveTheInternetEngine>().Keyed<IJackboxEngine>("survivetheinternet");
            
            builder.RegisterType<BlatherRoundClient>();
            builder.RegisterType<BlatherRoundEngine>().Keyed<IJackboxEngine>("blanky-blank");
        }
    }
}
