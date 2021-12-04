using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using Action = Mint.API.Models.Action;

namespace Mint.API.Services
{
    public class ActionService
	{

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

		private static void MethodError(string? pMethodName, Exception pException)
		{
			Console.WriteLine($"{typeof(ActionService)}.{pMethodName} : {pException.Message}");
		}

	}
}