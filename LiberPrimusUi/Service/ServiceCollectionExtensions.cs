using LiberPrimusAnalysisTool.Application.Queries;
using LiberPrimusAnalysisTool.Utility.Character;
using LiberPrimusAnalysisTool.Utility.Message;
using LiberPrimusUi.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LiberPrimusUi.Service;

public static class ServiceCollectionExtensions {
    public static void AddCommonServices(this IServiceCollection collection) {
        // Mediatr
        collection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetPageData).Assembly));
        collection.AddSingleton<ICharacterRepo, CharacterRepo>();
        collection.AddSingleton<IMessageBus, MessageBus>();
        collection.AddScoped<IPermutator, Permutator>();
        collection.AddTransient<MainWindowViewModel>();
    }
}