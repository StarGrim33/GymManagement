using Microsoft.Extensions.Options;
using Quartz;

namespace GymManagement.Infrastructure.Outbox;

internal sealed class ProcessOutboxMessagesJobSetup(IOptions<OutboxOptions> options) : IConfigureOptions<QuartzOptions>
{
    private readonly OutboxOptions _outboxOptions = options.Value;

    public void Configure(QuartzOptions options)
    {
        const string jobName = nameof(ProcessOutboxMessages);

        options.AddJob<ProcessOutboxMessages>(job => job
            .WithIdentity(jobName)).AddTrigger(
            configure =>
                configure.ForJob(jobName)
                    .WithSimpleSchedule(
                        schedule => schedule.WithIntervalInSeconds(_outboxOptions.IntervalInSeconds).RepeatForever()));
    }
}