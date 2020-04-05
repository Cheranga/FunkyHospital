using System.Threading.Tasks;
using FunkyHospital.Api.Core;

namespace FunkyHospital.Api.DataAccess.CommandHandlers
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task<Result> ExecuteAsync(TCommand command);
    }
}