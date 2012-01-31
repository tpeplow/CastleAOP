namespace Photon.Contrib.Castle.AOP
{
    using global::Castle.MicroKernel;

    public interface IAspectRequiresConfiguration
    {
        void Configure(IKernel kernel);
    }
}