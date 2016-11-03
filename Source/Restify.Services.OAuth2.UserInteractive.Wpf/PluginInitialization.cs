namespace JanHafner.Restify.Services.OAuth2.UserInteractive.Wpf
{
    using Handler;
    using Ninject.Modules;

    /// <summary>
    /// Registers the dependencies of this implementation of<see cref="IAccessCodeHandler"/>.
    /// </summary>
    public sealed class PluginInitialization : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            this.Rebind<IAuthorizationHandler>().To<UserInteractiveAuthorizationHandler>();
            this.Rebind<IAccessCodeHandler>().To<WpfAccessCodeHandler>();
        }
    }
}