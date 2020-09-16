Explanation of the credit-rating-service.proto file.


First row indicate what syntax version the proto file is in.
```
syntax = "proto3";
```
Second row indicate the namespace the interface is implemented into.
```
option csharp_namespace = "CreditRatingService";
```
Specify name of the service to prevent naming conflicts with protocol message naming types
```
package CreditRating;
```

Define the rpc service interface
* Similar to class with methods, rpc keyword with method name, with a list of input and output types.
* service keyword (service name) { <br /> 
    rpc (method name) (argument message type) return (message type) <br />
}
```
service CreditRatingCheck {
    rpc CheckCreditReport (CreditRequest) return (CreditReply);
}
```
Define protocol messages types
* message types define the structure of the message with respective types
* Can use scalar types, enumerations and other types for fields.
* Each field has a unique id that is used to identify field when converted to a binary format
* NUMBERS(the ids) should not be changed once the application is operative.
```
message CreditRequest {
    string customerId = 1;
    int32 credit = 2;
}

message CreditReply {
    bool isAccepted = 1;
}

```

Once the contract is defined update the csproject.
* Protobuf references the proto file.
* Assign the value to the server using the GrpcServices attribute.
* This allows system to generate c# code for basic general infrastructure to establish communication.
```
 <ItemGroup>
    <Protobuf Include="Protos\credit-rating-service.proto" GrpcServices="Server" />
  </ItemGroup>
 ```

This will contain two files after the ```dotnet run``` command (which will run the dotnet server.)
* It will have the created service implemented the gRPC communication, and the base class that is generated from the credit-rating-service proto file.
```
obj/Debug/netcoreapp3.1
```

NOTE: If they are any errors running dotnet grpc service in mac or old windows versions. 
* Is becuase it doesn't support tls with http2.
* The solution is using Http2 without tls.
* This code will integrate http2 in the Program.cs
``` c#
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                webBuilder.ConfigureKestrel(options =>
                {
                    // Setup a HTTP/2 endpoint without TLS.
                    options.ListenLocalhost(5000, o => o.Protocols = 
                        HttpProtocols.Http2);
                });
            }
            webBuilder.UseStartup<Startup>();
        });
} 
```