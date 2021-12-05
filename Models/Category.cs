using System.Data;
using Mint.API.Interfaces;
using System.Data.SqlClient;

namespace Mint.API.Models
{
    public class Category : Model
	{

		public int Id { get; set; }
		public string Title { get; set; } = "";

		public bool Read(SqlDataReader pReader)
		{
			if (pReader == null || !pReader.Read()) return false;

			Id		= pReader.GetFieldValue<int>("CATEGORY_id");
			Title	= pReader.GetFieldValue<string>("CATEGORY_title");

			return true;
		}

	}
}