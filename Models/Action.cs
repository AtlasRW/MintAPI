using System.Data;
using System.Reflection;
using Mint.API.Services;
using System.Data.SqlClient;

namespace Mint.API.Models
{
    public class Action
	{

		#region PUBLIC PROPERTIES

		public int Id { get; set; }
		public string Title { get; set; } = "";
		public string Link { get; set; } = "";

		#endregion

		#region PUBLIC METHODS

		/// <summary>
		/// Récupérer l'ensemble des Actions
		/// </summary>
		/// <returns>Liste d'Actions</returns>
		public static List<Action> GetAll()
		{
			List<Action> vResults = new List<Action>();
			DBConnection? vQConnection = null;

			try
			{
				vQConnection = new DBConnection();

				SqlDataReader vReader = vQConnection.ExecuteProcedure("[ACTION_getAll]");
				Action vResult = new Action();
				while (vResult.Read(vReader))
				{
					vResults.Add(vResult);
					vResult = new Action();
				}

				vReader.Close();
			}
			catch (Exception pException)
			{
				MethodError(MethodBase.GetCurrentMethod()?.Name, pException);
			}
			finally
			{
				if (vQConnection != null) vQConnection.Close();
			}

			return vResults;
		}

		/// <summary>
		/// Ajoute une Action
		/// </summary>
		/// <param name="pActionTitle">Titre de la nouvelle Action</param>
		/// <param name="pActionLink">Lien de la nouvelle Action</param>
		/// <param name="pResult">Résultat de la requête SQL</param>
		/// <returns>Action</returns>
		public static Action? Insert(
			string pActionTitle,
			string pActionLink,
			out bool pResult
		)
		{
			Action? vResult = null;
			DBConnection? vQConnection = null;

			try
			{
				vQConnection = new DBConnection();
				vQConnection.AddParameter("@pActionTitle", SqlDbType.NVarChar, 50, pActionTitle);
				vQConnection.AddParameter("@pActionLink", SqlDbType.NVarChar, pActionLink);

				SqlDataReader vReader = vQConnection.ExecuteProcedure("[ACTION_insert]", out int vReturn);
				pResult = vReturn < 0 ? false : true;
				vResult = new Action();
				if (!vResult.Read(vReader))
					vResult = null;

				vReader.Close();
			}
			catch (Exception pException)
			{
				MethodError(MethodBase.GetCurrentMethod()?.Name, pException);
				pResult = false;
			}
			finally
			{
				if (vQConnection != null) vQConnection.Close();
			}

			return vResult;
		}

		#endregion

		#region PRIVATE METHODS

		public bool Read(SqlDataReader pReader)
		{
			if (pReader == null || !pReader.Read()) return false;

			Id		= pReader.GetInt32(pReader.GetOrdinal("ACTION_id"));
			Title	= pReader.GetString(pReader.GetOrdinal("ACTION_title"));
			Link	= pReader.GetString(pReader.GetOrdinal("ACTION_link"));

			return true;
		}

		private static void MethodError(string? pMethodName, Exception pException)
		{
			Console.WriteLine($"{typeof(Action)}.{pMethodName} : {pException.Message}");
		}

		#endregion

	}
}