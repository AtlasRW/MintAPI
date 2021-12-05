using System.Data.SqlClient;

namespace Mint.API.Interfaces
{
    public interface Model
	{

		bool Read(SqlDataReader pReader);

	}
}