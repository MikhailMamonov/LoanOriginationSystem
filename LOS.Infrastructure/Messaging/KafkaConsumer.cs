using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace LOS.Infrastructure.Messaging;

public abstract class KafkaConsumer<T> : BackgroundService
{
    private readonly string _bootstrapServers;
    private readonly string _groupId;
    private readonly string _topic;
    private readonly ILogger _logger;
    private IConsumer<string, string> _consumer;

    protected KafkaConsumer(
        string bootstrapServers,
        string groupId,
        string topic,
        ILogger logger)
    {
        _bootstrapServers = bootstrapServers;
        _groupId = groupId;
        _topic = topic;
        _logger = logger;
    }

    protected abstract Task HandleMessageAsync(T message);

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() => ConsumeMessages(stoppingToken), stoppingToken);
    }

    private void ConsumeMessages(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = _groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _consumer.Subscribe(_topic);

        _logger.LogInformation("Kafka consumer started for topic: {Topic}", _topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(stoppingToken);

                    if (result?.Message?.Value != null)
                    {
                        var message = JsonSerializer.Deserialize<T>(result.Message.Value);

                        if (message != null)
                        {
                            HandleMessageAsync(message).GetAwaiter().GetResult();
                            _consumer.Commit(result);
                        }
                    }
                }
                catch (ConsumeException e)
                {
                    _logger.LogError(e, "Error consuming message from Kafka");
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Kafka consumer stopping");
        }
        finally
        {
            _consumer?.Close();
            _consumer?.Dispose();
        }
    }

    public override void Dispose()
    {
        _consumer?.Close();
        _consumer?.Dispose();
        base.Dispose();
    }
}