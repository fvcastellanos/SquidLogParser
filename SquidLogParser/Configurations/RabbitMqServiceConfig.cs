using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SquidLogParser.RabbitMq;

namespace SquidLogParser.Configurations
{
    public static class RabbitMqServiceConfig
    {
        public static IServiceCollection AddRabbitMqConnectionFactory(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            var rabbitMqServer = Environment.GetEnvironmentVariable("RABBIT_SERVER") ?? 
                "cloud.cavitos.net";

            var userName = Environment.GetEnvironmentVariable("RABBIT_USER") ??
                "guest";

            var password = Environment.GetEnvironmentVariable("RABBIT_PASSWORD") ??
                "guest";

            services.AddSingleton<IConnectionFactory>(new ConnectionFactory()
            {
                HostName = rabbitMqServer,
                UserName = userName,
                Password = password,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5),
                DispatchConsumersAsync = true
            });

            return services;
        }

        public static IServiceCollection AddQueueConsumer(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var loggerFactory = provider.GetService<ILoggerFactory>();
            var connectionFactory = provider.GetService<IConnectionFactory>();

            var logger = loggerFactory.CreateLogger<QueueConsumer>();

            services.AddSingleton<QueueConsumer>(new QueueConsumer(logger, connectionFactory));

            return services;
        }

        // public static IServiceCollection AddRabbitMqTemplate(this IServiceCollection services)
        // {
        //     var provider = services.BuildServiceProvider();
        //     var configuration = provider.GetRequiredService<IConfiguration>();
        //     var connectionFactory = provider.GetRequiredService<IConnectionFactory>();
        //     var loggerFactory = provider.GetRequiredService<ILoggerFactory>();

        //     var exchange = configuration["RabbitMq:Exchange"];
        //     var queue = configuration["RabbitMq:CommandQueue"];

        //     services.AddSingleton<IRabbitMqTemplate>(new RabbitMqTemplate(loggerFactory, connectionFactory, exchange, queue));

        //     return services;
        // }        
    }
}