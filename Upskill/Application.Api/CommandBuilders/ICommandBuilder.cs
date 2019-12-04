using System.Threading.Tasks;
using Application.Api.Commands;

namespace Application.Api.CommandBuilders
{
    public interface ICommandBuilder<TDto, TCommand> 
        where TDto: class 
        where TCommand : ICommand
    {
        Task<TCommand> Build(TDto from);
    }
}
