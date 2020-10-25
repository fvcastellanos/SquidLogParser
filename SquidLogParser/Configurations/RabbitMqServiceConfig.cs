using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SquidLogParser.AccessLog;
using SquidLogParser.Data;
using SquidLogParser.RabbitMq;
using SquidLogParser.Services;

namespace SquidLogParser.Configurations
{
    public static class RabbitMqServiceConfig
    {
        public static IServiceCollection AddRabbitMqConnectionFactory(this IServiceCollection services)
        {
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
            var logParser = provider.GetService<IAccessLogParser>();
            var dbContext = provider.GetService<SquidLogContext>();

            var logger = loggerFactory.CreateLogger<SquiqLogConsumer>();

            services.AddSingleton<SquiqLogConsumer>(new SquiqLogConsumer(logger, 
                connectionFactory, logParser, dbContext));

            return services;
        }
    }
}