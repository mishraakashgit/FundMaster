using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FundNavTracker.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FundNavTracker.Services
{
    public class NavAlertCheckerService : BackgroundService
    {
        private readonly INavService _navService;
        private readonly IAlertRepository _alertRepository;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<NavAlertCheckerService> _logger;
        private readonly TimeSpan _interval;

        public NavAlertCheckerService(
            INavService navService,
            IAlertRepository alertRepository,
            IEmailSender emailSender,
            IConfiguration configuration,
            ILogger<NavAlertCheckerService> logger)
        {
            _navService = navService;
            _alertRepository = alertRepository;
            _emailSender = emailSender;
            _logger = logger;

            var seconds = configuration.GetValue<int?>("Alerting:CheckIntervalSeconds") ?? 300;
            _interval = TimeSpan.FromSeconds(Math.Max(30, seconds));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var alerts = await _alertRepository.GetActiveAsync();
                    foreach (var alert in alerts)
                    {
                        var nav = await _navService.GetNavByFundCodeAsync(alert.FundCode);
                        if (nav == null)
                        {
                            continue;
                        }

                        var triggered = alert.Direction switch
                        {
                            AlertDirection.Buy => nav.Nav <= alert.TargetNav,
                            AlertDirection.Sell => nav.Nav >= alert.TargetNav,
                            _ => false
                        };

                        if (!triggered)
                        {
                            continue;
                        }

                        var subject = $"NAV Alert: {alert.FundCode} hit {alert.TargetNav.ToString(CultureInfo.InvariantCulture)}";
                        var body = $"<p>Your NAV alert was triggered.</p>" +
                                   $"<p><strong>Fund:</strong> {nav.FundName} ({nav.FundCode})</p>" +
                                   $"<p><strong>Current NAV:</strong> {nav.Nav.ToString(CultureInfo.InvariantCulture)}</p>" +
                                   $"<p><strong>Target NAV:</strong> {alert.TargetNav.ToString(CultureInfo.InvariantCulture)}</p>" +
                                   $"<p><strong>Action:</strong> {alert.Direction}</p>" +
                                   $"<p><strong>Date:</strong> {nav.Date}</p>";

                        var sent = await _emailSender.SendAsync(alert.UserEmail, subject, body);
                        if (sent)
                        {
                            alert.IsActive = false;
                            alert.TriggeredAtUtc = DateTime.UtcNow;
                            await _alertRepository.UpdateAsync(alert);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while processing NAV alerts.");
                }

                try
                {
                    await Task.Delay(_interval, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    return;
                }
            }
        }
    }
}
