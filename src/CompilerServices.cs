// https://stackoverflow.com/questions/64749385/predefined-type-system-runtime-compilerservices-isexternalinit-is-not-defined
// required for C# 9's "records"

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}