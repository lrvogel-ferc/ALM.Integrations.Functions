using ALM.Integrations.Services;
using System;
using System.Threading.Tasks;

namespace ALM.Integrations.Functions
{
    public class CreateWorkItem
    {
        private readonly ILogger<CreateWorkItem> _logger;


        [Function(nameof(CreateWorkItem))]
        public async Task Run(
            [ServiceBusTrigger("createWorkItem", "mysubscription", Connection = "")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            await svc.CreateWorkItem(message.Body.ToString());

             // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }

        private readonly WorkItemService svc;
        public CreateWorkItem(ILogger<CreateWorkItem> logger, WorkItemService workItemService)
        {
            _logger = logger;
            svc = workItemService;
        }
    }
}
