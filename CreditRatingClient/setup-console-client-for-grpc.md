Create a console application using this command.
```
dotnet new console -o CreditRatingClient
```

Add packages 
* The Grpc.Net.Client package has the basic classes needed to build a grpc client.

```
dotnet add CreditRatingClient.csproj package Grpc.Net.Client
```
* Google protobuf package allows application to manage Protocol buffers.

```
dotnet add CreditRatingClient.csproj package Google.Protobuf
```

* The GRpc.Tools package that's compiles proto files into c# code. 
* The package is only required at build time, and should not be included in generated output process.
* Marks reference to ```PrivateAssets``` attribute.
```
dotnet add CreditRatingClient.csproj package Grpc.Tools
```

Edit the csproj and add reference to the Protobuf file.
* Set the GrpcServices attribute to Client, only generate c# code needed for client.
* Client side code is known as a stub.
``` xml
<ItemGroup>
    <Protobuf Include="Protos\credit-rating-service.proto" GrpcServices="Client" />
</ItemGroup>
```

Generate namespace and types of the proto file by building project.
``` dotnet build ```

You will find generated code in obj folder.
``` obj/Debug/netcoreapp3.1 ```

Create a gRPC channel on port 5001 on current machine.
```
var channel = GrpcChannel.ForAddress("https://localhost:5001");
```

The channel is passed to constructor of the CreditRatingCheckClient which is the actual gRPC client.
```
var client =  new CreditRatingCheck.CreditRatingCheckClient(channel);
```

If message of the CreditRequest is created
```
var creditRequest = new CreditRequest { CustomerId = "id0201", Credit = 7000};
```

Then message is sent to Rmote serviceby invoking the CheckCreditRequestAsync()
```
var reply = await client.CheckCreditRequestAsync(creditRequest);
```

Run dotnet run after running server.
``` dotnet run ```

It will return exception when attempting call service when the service does not use tls.
```
Unhandled exception. Grpc.Core.RpcException: Status(StatusCode=Internal, Detail="Error starting gRPC call: Connection refused")
   at GrpcGreeterClient.Program.Main(String[] args) in /Users/andreachiarelli/Documents/Playground/grpc-dotnet/CreditRatingClient/Program.cs:line 17
   at GrpcGreeterClient.Program.<Main>(String[] args)
```
Disable TLS on client.
``` c#
    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
        // The following statement allows you to call insecure services. To be used only in development environments.
        AppContext.SetSwitch(
            "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        serverAddress = "http://localhost:5000";
    }
```