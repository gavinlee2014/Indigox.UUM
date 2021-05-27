using System;

namespace Indigox.UUM.Naming.Model
{
    public interface INameStrategy
    {
        bool IsReusable { get; }

        string Naming( string name );
    }
}