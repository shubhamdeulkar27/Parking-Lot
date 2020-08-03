using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace ParkingLot
{
    /// <summary>
    /// Class For Sending Message To Azure Service Bus.
    /// </summary>
    public class Sender
    {
        //Azure Service Queue Connection String.
        const string ServiceBusConnectionString = "Endpoint=sb://parkinglot.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=oLImDNB8fabh+wvOxPdT7wVpUmuQn3eLYrm1DUpLLGg=";
        
        //Azure Queue Name.
        const string QueueName = "parkinglotqueue";
        
        //IQueueClient Instance For Istablishing Connection With Azure Service Bus Queue.
        static IQueueClient queueClient;

        /// <summary>
        /// Function To Send Message TO Azure Service Bus Queue.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="OperationType"></param>
        /// <param name="email"></param>
        public void Send(string input, string OperationType, string email)
        {
            try
            {
                queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
                var encodedInput = Encoding.UTF8.GetBytes(input);
                Message message = new Message();
                message.Body = encodedInput;
                message.Label = OperationType;
                message.To = email;
                queueClient.SendAsync(message);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
