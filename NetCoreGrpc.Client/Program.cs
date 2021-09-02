
namespace NetCoreGrpc.Client
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using Google.Protobuf.WellKnownTypes;
    using Grpc.Core;
    using NetCoreGrpc.Action;
    using NetCoreGrpc.Client.Applibs;
    using NetCoreGrpc.Domain.Model;
    using NetCoreGrpc.Model;
    using NetCoreGrpc.Service;
    using Newtonsoft.Json;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var scope = AutofacConfig.Container.BeginLifetimeScope())
                {
                    var client = scope.Resolve<MemberService.MemberServiceClient>();
                    var bidrectionalSvc = scope.Resolve<BidirectionalService.BidirectionalServiceClient>();
                    var cmd = string.Empty;

                    while (cmd != "4")
                    {
                        switch (cmd)
                        {
                            case "1":
                                var insertResult = client.Insert(
                                    new Member()
                                    {
                                        MemberId = $"TEST{TimeStampHelper.ToUtcTimeStamp(DateTime.Now)}",
                                        NickName = $"Nick{TimeStampHelper.ToUtcTimeStamp(DateTime.Now)}",
                                        CreateDateTimeStamp = TimeStampHelper.ToUtcTimeStamp(DateTime.Now)
                                    },
                                    // timeout 要用UTC時間
                                    deadline: DateTime.UtcNow.AddSeconds(3));

                                break;
                            case "2":
                                var getResult = client.GetAll(
                                    new Empty(),
                                    // timeout 要用UTC時間
                                    deadline: DateTime.UtcNow.AddSeconds(3));

                                Console.WriteLine(JsonConvert.SerializeObject(getResult.Members));

                                break;
                            case "3":
                                var headers = new Metadata { new Metadata.Entry("id", Guid.NewGuid().ToString()) };
                                var vidrectional = bidrectionalSvc.ActionAsync(new CallOptions(headers));

                                Task.Run(async () =>
                                {
                                    await foreach (var action in vidrectional.ResponseStream.ReadAllAsync())
                                    {
                                        Console.WriteLine(JsonConvert.SerializeObject(action));
                                    }
                                });

                                var count = 10;

                                while (--count >= 0)
                                {
                                    vidrectional.RequestStream.WriteAsync(new ActionModel()
                                    {
                                        Action = typeof(ChatMessageAction).Name,
                                        Content = JsonConvert.SerializeObject(new ChatMessageAction()
                                        {
                                            ChatMessage = new ChatMessage()
                                            {
                                                NickName = "TEST001",
                                                Message = $"{DateTime.Now}",
                                                CreateDateTimeStamp = TimeStampHelper.ToUtcTimeStamp(DateTime.Now)
                                            }
                                        })
                                    });

                                    SpinWait.SpinUntil(() => false, 1000);
                                }

                                break;
                            default:
                                break;
                        }

                        Console.WriteLine("1. Generate Member");
                        Console.WriteLine("2. Display All Members");
                        Console.WriteLine("3. Chat");
                        Console.WriteLine("4. Exit");
                        cmd = Console.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.Read();
        }
    }
}
