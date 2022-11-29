using System.Threading.Tasks;

namespace P1_Nikita.Command;

public abstract class AsyncCommandBase : CommandBase
{
    public bool IsExecuting;
    
    public override bool CanExecute(object? parameter)
    {
        return !IsExecuting && base.CanExecute(parameter);
    }

    public override async void Execute(object? parameter)
    {
        IsExecuting = true;
        try
        {
            await ExecuteAsync(parameter);
        }
        finally
        {
            IsExecuting = false;
        }
    }

    public abstract Task ExecuteAsync(object? parameter);
}