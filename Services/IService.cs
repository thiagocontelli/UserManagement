namespace UserManagement.Services;

public interface IService<TInput, TOutput>
{
    Task<TOutput> Execute(TInput input);
}

