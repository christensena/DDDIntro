namespace DDDIntro.Web.Infrastructure.Bootstrapping
{
    public interface IBootstrapper
    {
        void StartUp();
        void ShutDown();
    }
}