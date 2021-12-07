using System.Data;
using Mint.API.Interfaces;
using System.Data.SqlClient;

namespace Mint.API.Models
{
    public class Action : Model
	{

		public int Id { get; set; }
		public string Title { get; set; } = "";
		public string Link { get; set; } = "";

		public bool Read(SqlDataReader pReader)
		{
			if (pReader == null || !pReader.Read()) return false;

			Id		= pReader.GetFieldValue<int>("ACTION_id");
			Title	= pReader.GetFieldValue<string>("ACTION_title");
			Link	= pReader.GetFieldValue<string>("ACTION_link");

			return true;
		}

	}
}