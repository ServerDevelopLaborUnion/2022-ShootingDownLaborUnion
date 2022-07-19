# Generate C# Codes
.\\protoc.exe --csharp_out="../../client/Assets\01.Scripts\08. Protobuf" Server.proto
.\\protoc.exe --csharp_out="../../client/Assets\01.Scripts\08. Protobuf" Client.proto
.\\protoc.exe --csharp_out="../../client/Assets\01.Scripts\08. Protobuf" Type.proto

# Generate Javascript Codes Bundle
.\\protoc.exe --js_out="../src/proto" Server.proto
.\\protoc.exe --js_out="../src/proto" Client.proto
.\\protoc.exe --js_out="../src/proto" Type.proto