using System.Data;
using System.Reflection;
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
			List<Action> vActions = new List<Action>();
			DBConnection? vConnection = null;

			try
			{
				vConnection = new DBConnection();
				
				vActions = vConnection.ExecuteProcedure<Action>("[ACTION_getAll]");
			}
			catch (Exception pException)
			{
				MethodError(MethodBase.GetCurrentMethod()?.Name, pException);
			}
			finally
			{
				if (vConnection != null) vConnection.Close();
			}

			return vActions;
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
			Action? vAction = null;
			DBConnection? vConnection = null;

			try
			{
				vConnection = new DBConnection();
				vConnection.AddParameter("@pActionTitle", SqlDbType.NVarChar, 50, pActionTitle);
				vConnection.AddParameter("@pActionLink", SqlDbType.NVarChar, pActionLink);

				vAction = vConnection.ExecuteProcedure<Action>("[ACTION_insert]", out int vReturn)[0];
				pResult = vReturn < 0 ? false : true;
			}
			catch (Exception pException)
			{
				MethodError(MethodBase.GetCurrentMethod()?.Name, pException);
				pResult = false;
			}
			finally
			{
				if (vConnection != null) vConnection.Close();
			}

			return vAction;
		}

		private static void MethodError(string? pMethodName, Exception pException)
		{
			Console.WriteLine($"{typeof(ActionService)}.{pMethodName} : {pException.Message}");
		}

	}
}