
namespace FastCarSales.Services
{
	public interface IUserService
	{
		Task<IEnumerable<string>> GetAdmins();
	}
}