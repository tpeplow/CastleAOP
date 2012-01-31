namespace Photon.Contrib.Castle.AOP
{
    using System;

    using global::Castle.MicroKernel;

    public class KernalAspectFactory : IAspectFactory
    {
        private readonly IKernel kernel;

        public KernalAspectFactory(IKernel kernel)
        {
            if (kernel == null) throw new ArgumentNullException("kernel");
            this.kernel = kernel;
        }

        public IAspect Create(Type aspect)
        {
            return (IAspect)kernel.Resolve(aspect);
        }
    }
}
