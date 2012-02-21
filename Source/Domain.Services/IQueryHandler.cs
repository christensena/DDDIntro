namespace DDDIntro.Domain.Services
{
    // design pinched from http://www.cuttingedge.it/blogs/steven/pivot/entry.php?id=92
    // but might use Linq.Specs or something later. I like to chain query specifications
    // and LINQ is amenable to that. however I like the side if IQueryHandler that means
    // I could go direct to a NHibernate Session and use whatever query method I wanted
    public interface IQueryHandler<in TQuery, out TResult> 
        where TQuery : IQuery<TResult> 
        where TResult : class
    {
        TResult Handle(TQuery query);
    }
}