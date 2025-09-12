using System.Data;
using Dapper;
using GymManagement.Application.Abstractions.Data;
using GymManagement.Domain.Abstractions;
using GymManagement.Domain.Entities.Users.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;

namespace GymManagement.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessages(
    ISqlConnectionFactory connectionFactory,
    IBus bus,
    IOptions<OutboxOptions> outboxOptions,
    ILogger<ProcessOutboxMessages> logger)
    : IJob
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Beginning to process outbox messages");

        using var connection = connectionFactory.CreateConnection();
        using var transaction = connection.BeginTransaction();

        var outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

        foreach (var outboxMessage in outboxMessages)
        {
            Exception? exception = null;

            try
            {
                var jsonObject = JObject.Parse(outboxMessage.Content);
                var typeName = jsonObject["$type"]?.ToString();

                if (string.IsNullOrEmpty(typeName))
                {
                    logger.LogWarning("Missing $type in outbox message {MessageId}", outboxMessage.Id);
                    return;
                }

                var eventType = Type.GetType(typeName);

                if (eventType is null)
                {
                    logger.LogWarning("Unknown event type in outbox message {MessageId}", outboxMessage.Id);
                    return;
                }

                var domainEvent = JsonConvert.DeserializeObject(outboxMessage.Content, eventType, JsonSerializerSettings)!;

                await bus.Publish(domainEvent, context.CancellationToken);

                logger.LogInformation("Processed new event {EventType}", eventType.Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception while processing outbox message {MessageId}",
                    outboxMessage.Id);

                exception = ex;

                transaction.Rollback();
            }

            await MarkOutboxMessageAsProcessedAsync(connection, transaction, outboxMessage, exception);

            transaction.Commit();

            logger.LogInformation("Processed outbox message {MessageId}", outboxMessage.Id);
        }
    }

    private static async Task MarkOutboxMessageAsProcessedAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        OutboxMessageResponse outboxMessage,
        Exception? exception)
    {
        const string sql = """
                           
                                                      UPDATE outbox_messages
                                                      SET processed_on_utc = @ProcessedOnUtc,
                                                          error = @Error
                                                      WHERE id = @Id
                           """;

        await connection.ExecuteAsync(sql, new
        {
            outboxMessage.Id,
            ProcessedOnUtc = DateTime.UtcNow,
            Error = exception?.ToString()
        }, transaction);
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction)
    {
        var sql = $"""
                   SELECT id, content
                   FROM outbox_messages
                   WHERE processed_on_utc IS NULL
                   ORDER BY occurred_on_utc
                   LIMIT {_outboxOptions.BatchSize}
                   FOR UPDATE
                   """;

        var outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(sql, transaction: transaction);

        return outboxMessages.ToList();
    }

    private sealed record OutboxMessageResponse(Guid Id, string Content);
}