using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MessagePipe;
using Newtonsoft.Json;
using TestTankProject.Runtime.AssetLoading;
using TestTankProject.Runtime.Core.SaveLoad;
using TestTankProject.Runtime.Gameplay;
using TestTankProject.Runtime.SaveLoad;
using TestTankProject.Runtime.SceneLoading;
using TestTankProject.Runtime.Sounds;
using TestTankProject.Runtime.UserInput;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ContainerBuilderExtensions = MessagePipe.ContainerBuilderExtensions;

namespace TestTankProject.Runtime.Bootstrap
{
    public class BootstrapScope : LifetimeScope
    {
        [SerializeField] private List<GameConfig> _registeredGameConfigs;
        [SerializeField] private List<CardIconConfig> _registeredCardIconConfigs;
        [SerializeField] private AudioManager _audioManagerPrefab;
        
        protected override void Configure(IContainerBuilder builder)
        {
            Instantiate(_audioManagerPrefab);
            
            MessagePipeOptions options = builder.RegisterMessagePipe();
            RegisterMessageBrokers(builder, options);
            RegisterJsonSerializer(builder);
            
            builder.RegisterInstance(Camera.main);
            builder.RegisterInstance(_registeredGameConfigs);
            builder.RegisterInstance(_registeredCardIconConfigs);
            builder.Register<SceneLoader>(Lifetime.Singleton);
            builder.Register<InputLogger>(Lifetime.Singleton);
            builder.Register<Raycaster>(Lifetime.Singleton);
            builder.Register<SpriteLoader>(Lifetime.Singleton);
            builder.Register<LocalGameSaver>(Lifetime.Singleton).As<IGameSaver>();
            builder.Register<LocalGameLoader>(Lifetime.Singleton).As<IGameLoader>();
            builder.Register<AudioManager>(Lifetime.Singleton);
                
            builder.RegisterEntryPoint<BootstrapFlow>();
        }
        
        private void RegisterMessageBrokers(IContainerBuilder builder, MessagePipeOptions messagePipeOptions)
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            MethodInfo genericBindMethod = typeof(ContainerBuilderExtensions).GetMethods()
                .First(method => method.Name == "RegisterMessageBroker" && method.IsGenericMethod);
            object[] methodArgs = { builder, messagePipeOptions };

            foreach (Type type in currentAssembly.GetTypes())
            {
                MessageAttribute targetAttribute = type.GetCustomAttribute<MessageAttribute>();
                
                if (targetAttribute == null)
                    continue;

                MethodInfo concreteBindMethod = genericBindMethod.MakeGenericMethod(type);
                concreteBindMethod.Invoke(builder, methodArgs);
            }
        }

        private void RegisterJsonSerializer(IContainerBuilder builder)
        {
            JsonSerializerSettings settings = new()
            {
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter>()
                {
                    new Vector2IntConverter(),
                    new CardModelConverter(),
                    new GameModelConverter()
                }
            };

            JsonSerializer jsonSerializer = JsonSerializer.Create(settings);
            builder.RegisterInstance(jsonSerializer);
        }
    }
}