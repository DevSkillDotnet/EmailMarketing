using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using EmailMarketing.Common.Constants;
using EmailMarketing.Common.Extensions;
using EmailMarketing.Common.Services;
using EmailMarketing.EmailSendingWorkerService.Services;
using EmailMarketing.EmailSendingWorkerService.Templates;
using EmailMarketing.Framework.Entities.Campaigns;
using EmailMarketing.Framework.Services.Campaigns;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmailMarketing.EmailSendingWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IWorkerMailerService _mailerService;
        private readonly ICampaignService _campaignService;
        private readonly ICampaignReportService _campaignReportService;
        private readonly IMailerService _confirmationMailerService;

        public Worker(ILogger<Worker> logger, 
            IWorkerMailerService mailerService,
            ICampaignService campaignService,
            ICampaignReportService campaignReportService,
            IMailerService confirmationMailerService)
        {
            _logger = logger;
            _mailerService = mailerService;
            _campaignService = campaignService;
            _campaignReportService = campaignReportService;
            _confirmationMailerService = confirmationMailerService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Worker started at: {DateTime.Now}");
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                //Fetching campaignList based on isProcessing status
                var campaignList = await _campaignService.GetAllProcessingCampaign();

                try
                {
                    var campaignReportList = new List<CampaignReport>();

                    foreach(var item in campaignList)
                    {
                        try
                        {
                            int totalSuccessCount = 0, totalFailCount = 0;
                            //Fetching campaign with email list
                            var result = await _campaignService.GetAllEmailByCampaignId(item.Id);

                            //Fetching contact list from result
                            var contactList = result.CampaignGroups.Select(x => x.Group).SelectMany(x => x.ContactGroups).Select(x => x.Contact).ToList();

                            foreach (var singleContact in contactList)
                            {
                                var fieldmapDict = singleContact.ContactValueMaps.ToList().ToDictionary(x => x.FieldMap.DisplayName, x => x.Value);
                                fieldmapDict.Add(ConstantsValue.ContactFieldMapEmail, singleContact.Email);

                                var emailTemplate = result.EmailTemplate.EmailTemplateBody;
                                if (item.IsPersonalized)
                                {
                                    emailTemplate = ConvertExtension.FormatStringFromDictionary(emailTemplate, fieldmapDict);
                                }

                                var status = await _mailerService.SendBulkEmailAsync(singleContact.Email, item.EmailSubject, emailTemplate, result.SMTPConfig);

                                //Counting email success and failure
                                if (status)
                                {
                                    totalSuccessCount++;
                                }
                                else
                                {
                                    totalFailCount++;
                                }

                                //Adding value to campaignReport based on each mail sending status
                                var campaignReport = new CampaignReport
                                {
                                    CampaignId = result.Id,
                                    ContactId = singleContact.Id,
                                    SMTPConfigId = result.SMTPConfigId,
                                    IsDelivered = status,
                                    IsSeen = false,
                                    IsPersonalized = item.IsPersonalized,
                                    SendDateTime = item.SendDateTime,
                                    //SeenDateTime = DateTime.Now
                                };

                                campaignReportList.Add(campaignReport);
                            }

                            //Upating Campaign isProcessing Status
                            await _campaignService.UpdateCampaignAsync(item);

                            //Sending email confirmation
                            if (item.IsSendEmailNotify)
                            {
                                var emailSubject = "Campaign Sent Confirmation";
                                var demoEmailTemplatePartial = new DemoEmailTemplate("Sir", totalSuccessCount, totalFailCount);
                                var body = demoEmailTemplatePartial.TransformText();
                                await _confirmationMailerService.SendEmailAsync(item.SendEmailAddress, emailSubject, body);
                            }

                        }
                        catch (Exception ex)
                        {
                            _logger.LogInformation($"Failed to send email for this campaign : {item.Name}");
                        }

                    }

                    await _campaignReportService.AddCampaingReportAsync(campaignReportList);
                }
                catch(Exception ex)
                {
                    _logger.LogInformation($"Error Message: {ex.Message}");
                }

                await Task.Delay(20000, stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Worker stopped at: {DateTime.Now}");
            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation($"Worker disposed at: {DateTime.Now}");
            base.Dispose();
        }

    }
}
