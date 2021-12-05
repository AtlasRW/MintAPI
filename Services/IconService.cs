using System.Data;
using Mint.API.Models;
using System.Reflection;

namespace Mint.API.Services
{
    public class IconService
	{

		/// <summary>
		/// Récupérer l'ensemble des Icônes
		/// </summary>
		/// <returns>Liste d'Icônes</returns>
		public static List<Icon> GetAll()
		{
			List<Icon> vIcons = new List<Icon>();
			DBConnection? vConnection = null;

			try
			{
				vConnection = new DBConnection();
				
				vIcons = vConnection.ExecuteProcedure<Icon>("[ICON_getAll]");
			}
			catch (Exception pException)
			{
				MethodError(MethodBase.GetCurrentMethod()?.Name, pException);
			}
			finally
			{
				if (vConnection != null) vConnection.Close();
			}

			return vIcons;
		}

		/// <summary>
		/// Ajoute une Icône
		/// </summary>
		/// <param name="pIconData">Binaire de la nouvelle Icône</param>
		/// <param name="pResult">Résultat de la requête SQL</param>
		/// <returns>Icône</returns>
		public static Icon? Insert(
			byte[] pIconData,
			out bool pResult
		)
		{
			Icon? vIcon = null;
			DBConnection? vConnection = null;

			try
			{
				vConnection = new DBConnection();
				vConnection.AddParameter("@pIconData", SqlDbType.Image, pIconData);

				vIcon = vConnection.ExecuteProcedure<Icon>("[ICON_insert]", out int vReturn)[0];
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

			return vIcon;
		}

		private static void MethodError(string? pMethodName, Exception pException)
		{
			Console.WriteLine($"{typeof(IconService)}.{pMethodName} : {pException.Message}");
		}

	}
}