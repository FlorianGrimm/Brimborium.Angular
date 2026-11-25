namespace Brimborium.Angular.API;

// The return type of methods on the client-side must be Task. 
[TypedSignalR.Client.Receiver]
public interface IClientContract {
    Task OnUpdated(Project project);
}

// The return type of methods on the hub-side must be Task or Task<T>. 
[TypedSignalR.Client.Hub]
public interface IHubContract {
    Task<ResponseAPI<Project>> Update(Project project);
}

#if false
MSBuildProjectFullPath
dotnet tool run dotnet-tsrts -- --project .\src\Brimborium.Angular.Contract\Brimborium.Angular.Contract.csproj --output .\src\Brimborium.AngularUI\src --eol CrLf --serializer  Json --naming-style None --enum Name
dotnet tool run dotnet-tsrts -- --project .\src\Brimborium.Angular\Brimborium.Angular.csproj --output .\src\Brimborium.AngularUI\src --eol CrLf --serializer  Json --naming-style None --enum Name


Usage: dotnet [--project <String>] [--output <String>] [--eol <NewLineOption>] [--asm] [--serializer <SerializerOption>] [--naming-style <NamingStyle>] [--enum <EnumStyle>] [--method <MethodStyle>] [--attr=<true|false>] [--help] [--version]

TypedSignalR.Client.TypeScript.Generator

Options:
  -p, --project <String>                 Path to the project file (Xxx.csproj) (Required)
  -o, --output <String>                  Output directory (Required)
  --eol <NewLineOption>                  End of line (Default: Lf) (Allowed values: Lf, Crlf, Cr)
  --asm                                  Flag whether to extend the transpile target to the referenced assembly.
  -s, --serializer <SerializerOption>    The output type will be suitable for the selected serializer. (Default: Json) (Allowed values: Json, MessagePack)
  -n, --naming-style <NamingStyle>       Naming style. If none is selected, the C# name is used as it is. (Default: CamelCase) (Allowed values: None, CamelCase, PascalCase)
  --enum <EnumStyle>                     Enum representation style (Default: Value) (Allowed values: Value, Name, NameCamel, NamePascal, Union, UnionCamel, UnionPascal)
  -m, --method <MethodStyle>             Method naming style. If none is selected, the C# name is used as it is. (Default: CamelCase) (Allowed values: None, CamelCase, PascalCase)
  --attr=<true|false>                    The flag whether attributes such as JsonPropertyName should affect transpilation. (Default: True)
  -h, --help                             Show help message
  --version                              Show version
#endif

#if false
public class UserDefinedType
{
    public Guid Id { get; set; }
    public DateTime Datetime { get; set; }
}

// The return type of methods on the client-side must be Task. 
public interface IClientContract
{
    // Of course, user defined type is OK. 
    Task ClientMethod1(string user, string message, UserDefinedType userDefine);
    Task ClientMethod2();
}

// The return type of methods on the hub-side must be Task or Task<T>. 
public interface IHubContract
{
    Task<string> HubMethod1(string user, string message);
    Task HubMethod2();
}

class Receiver1 : IClientContract
{
    // implementation
}

class Receiver2 : IClientContract, IHubConnectionObserver
{
    //
#endif
//