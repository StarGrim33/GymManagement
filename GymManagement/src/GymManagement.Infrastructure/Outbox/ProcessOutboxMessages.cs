using System.Data;
using Dapper;
using GymManagement.Application.Abstractions.Data;
using GymManagement.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace GymManagement.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessages(
    ISqlConnectionFactory connectionFactory,
    IPublisher publisher,
    OutboxOptions outboxOptions,
    ILogger<ProcessOutboxMessages> logger)
    : IJob
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

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
                var domainEvent = JsonConvert
                    .DeserializeObject<IDomainEvent>(
                        outboxMessage.Content,
                        JsonSerializerSettings)!;

                await publisher.Publish(domainEvent, context.CancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception while processing outbox message {MessageId}",
                    outboxMessage.Id);

                exception = ex;
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
                   LIMIT {outboxOptions.BatchSize}
                   FOR UPDATE
                   """;

        var outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(sql, transaction: transaction);

        return outboxMessages.ToList();
    }

    private sealed record OutboxMessageResponse(Guid Id, string Content);
}