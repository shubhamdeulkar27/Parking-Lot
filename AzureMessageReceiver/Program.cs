using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace AzureMessageReceiver
{
    /// <summary>
    /// Main Program For Fetching Message From Azure Service Bus Queue.
    /// </summary>
    class Program
    {
        //Azure Service Bus Queue Connection String.
        const string ServiceBusConnectionString = "Endpoint=sb://parkinglot.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=oLImDNB8fabh+wvOxPdT7wVpUmuQn3eLYrm1DUpLLGg=";
        
        //Azure Service Bus Queue Name.
        const string QueueName = "parkinglotqueue";

        //IQueueClient Instance For Connection.
        static IQueueClient queueClient;

        //SMTP Class object For Sending Email.
        static SMTP smtpObject = new SMTP();

        static void Main(string[] args)
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            //Register QueueClient's MessageHandler and receive messages in a loop.
            RegisterOnMessageHandlerAndReceiveMessages();
            Console.ReadKey();
            queueClient.CloseAsync();
        }

        /// <summary>
        /// Function Fot Prossing Message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            smtpObject.SendMail("Vehical Owner", message.To, Encoding.UTF8.GetString(message.Body));
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        /// <summary>
        /// Event Handler For Exceptions received on message pump. 
        /// </summary>
        /// <param name="exceptionReceivedEventArgs"></param>
        /// <returns></returns>
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Function For Registering Event Handler.
        /// </summary>
        static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            //Configure the message handler options in terms of exception handling, number of concurrent messages to deliver.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                //Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                //Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                //Indicates whether the message pump should automatically complete the messages after returning from user callback.
                //False below indicates the complete operation is handled by the user callback as in ProcessMessagesAsync().
                AutoComplete = false
            };

            //Register the function that processes messages.
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }
    }
}
