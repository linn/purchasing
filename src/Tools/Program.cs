namespace Linn.Purchasing.Tools;

using System;

using Linn.Common.Messaging.RabbitMQ.Dispatchers;
using Linn.Common.Persistence;
using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
using Linn.Purchasing.Domain.LinnApps.Suppliers;
using Linn.Purchasing.IoC;
using Linn.Purchasing.Resources.Messages;

using Microsoft.Extensions.DependencyInjection;

class Program
{
    static bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false;
        }
        try
        {
            var address = new System.Net.Mail.MailAddress(email);
            return address.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddLog();
        services.AddPersistence();
        services.AddRabbitConfiguration();
        services.AddMessageDispatchers();
    }

    static void Main(string[] args)
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        
        if (args.Length < 2)
        {
            Console.WriteLine("Not enough arguments passed!");
            Console.WriteLine("First arg (boolean) must specify whether to send emails to the specified test address or to send them to the real suppliers.");
            Console.WriteLine("Second arg (string) comma separated list of supplier ids to send, or 'all' to send to all");
            Console.WriteLine("Third arg (string) if test = true, must specify the test address");
            return;
        }
        
        if (!bool.TryParse(args[0], out var test))
        {
            Console.WriteLine("First argument must be a boolean to specify whether to run in test configuration or not");
            return;
        }
        
        var supplierIds = new List<int>();
        
        if (args[1] != "all")
        {
            var supplierIdStrings = args[1].Split(",");
            foreach (var idString in supplierIdStrings)
            {
                if (!int.TryParse(idString, out var supplierId))
                {
                    Console.WriteLine("Invalid Supplier Id(s) in list -- must be a comma separated list of numbers.");
                    return;
                }
               
                supplierIds.Add(supplierId);
            }
        }
        
        if (test)
        {
            if (args.Length < 3 || !IsValidEmail(args[2]))
            {
                Console.WriteLine("Invalid test email address supplied!");
                return;
            }
        }

        var serviceProvider = services.BuildServiceProvider();

        var repository =
            serviceProvider.GetRequiredService<IRepository<SupplierAutoEmails, int>>();

        var outstandingPosRepository 
            = serviceProvider.GetRequiredService<IQueryRepository<MrPurchaseOrderDetail>>();

        var mrMaster = serviceProvider.GetRequiredService<ISingleRecordRepository<MrMaster>>();

        var emailOrderBookMessageDispatcher 
            = serviceProvider.GetRequiredService<IMessageDispatcher<EmailOrderBookMessageResource>>();

        // dispatch a message for all the suppliers to receive an order book
        foreach (var s in repository.FindAll().Where(x => x.OrderBook.Equals("Y")))
        {
            if (outstandingPosRepository.FilterBy(o =>
                    o.JobRef == mrMaster.GetRecord().JobRef
                    && o.SupplierId == s.SupplierId
                    && o.PartSupplierRecord != null
                    && !o.DateCancelled.HasValue
                    && o.OurQuantity > o.QuantityReceived
                    && !string.IsNullOrEmpty(o.AuthorisedBy)).Any())
            {
                emailOrderBookMessageDispatcher.Dispatch(
                    new EmailOrderBookMessageResource
                        {
                            ForSupplier = s.SupplierId,
                            Timestamp = DateTime.Now,
                            ToAddress = s.EmailAddresses,
                            Test = test
                        });
            }
        }
    }
}
