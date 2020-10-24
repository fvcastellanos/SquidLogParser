using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace SquidLogParser.Configurations
{
    public static class RabbitMqServiceConfig
    {
        public static IServiceCollection AddRabbitMqConnectionFactory(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();

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