namespace DDDIntro.Domain.Services
{
    // design pinched from http://www.cuttingedge.it/blogs/steven/pivot/entry.php?id=92
    // but might use Linq.Specs or something later. I like to chain query specifications
    // and LINQ is amenable to that
    public interface IQueryHandler<in TQuery, out TResult> 
        where TQuery : IQuery<TResult> 
        where TResult : class
    {
        TResult Handle(TQuery query);
    }
}