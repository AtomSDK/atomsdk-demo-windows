namespace Atom.VPN.Demo.Interfaces
{
    public interface IConnection
    {
        void Initialize();
        void Connect();
        bool CanConnect { get; }
    }
}
