using Atom.Core.Models;
using System.Collections.Generic;

namespace Atom.VPN.Demo.Interfaces
{
    public interface IConnection
    {
        void Initialize(List<Protocol> protocols = null, List<Country> countries = null);
        void Connect();
        bool CanConnect { get; }
    }
}
