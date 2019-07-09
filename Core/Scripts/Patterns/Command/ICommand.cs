//Developed by Pavel Kravtsov.
namespace Core
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}