
namespace Application.Common.Exceptions
{
    public class NotFoundException(string resource, int id)
        : Exception($"{resource} with id {id} was not found.");
}
