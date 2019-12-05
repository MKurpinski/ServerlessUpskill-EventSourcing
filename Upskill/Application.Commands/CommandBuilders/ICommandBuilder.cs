using System.Threading.Tasks;
using Application.Commands.Commands;

namespace Application.Commands.CommandBuilders
{
    public interface ICommandBuilder<TDto, TCommand> 
        where TDto: class 
        where TCommand : ICommand
    {
        Task<TCommand> Build(TDto from);
    }
}
