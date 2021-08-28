
namespace NetCoreGrpc.Client
{
    using System;
    using System.Text.Json;
    using Autofac;
    using Google.Protobuf.WellKnownTypes;
    using NetCoreGrpc.Client.Applibs;
    using NetCoreGrpc.Domain.Model;
    using NetCoreGrpc.Model;
    using NetCoreGrpc.Service;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var scope = AutofacConfig.Container.BeginLifetimeScope())
                {
                    var client = scope.Resolve<MemberService.MemberServiceClient>();
                    var cmd = string.Empty;

                    while (cmd != "3")
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

                                Console.WriteLine(JsonSerializer.Serialize(getResult.Members));

                                break;
                            default:
                                break;
                        }

                        Console.WriteLine("1. Generate Member");
                        Console.WriteLine("2. Display All Members");
                        Console.WriteLine("3. Exit");
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
