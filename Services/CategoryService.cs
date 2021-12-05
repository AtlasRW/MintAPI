using System.Data;
using Mint.API.Models;
using System.Reflection;

namespace Mint.API.Services
{
    public class CategoryService
	{

		/// <summary>
		/// Récupérer l'ensemble des Catégories
		/// </summary>
		/// <returns>Liste de Catégories</returns>
		public static List<Category> GetAll()
		{
			List<Category> vCategories = new List<Category>();
			DBConnection? vConnection = null;

			try
			{
				vConnection = new DBConnection();
				
				vCategories = vConnection.ExecuteProcedure<Category>("[CATEGORY_getAll]");
			}
			catch (Exception pException)
			{
				MethodError(MethodBase.GetCurrentMethod()?.Name, pException);
			}
			finally
			{
				if (vConnection != null) vConnection.Close();
			}

			return vCategories;
		}

		/// <summary>
		/// Ajoute une Catégorie
		/// </summary>
		/// <param name="pCategoryTitle">Titre de la nouvelle Catégorie</param>
		/// <param name="pResult">Résultat de la requête SQL</param>
		/// <returns>Catégorie</returns>
		public static Category? Insert(
			string pCategoryTitle,
			out bool pResult
		)
		{
			Category? vCategory = null;
			DBConnection? vConnection = null;

			try
			{
				vConnection = new DBConnection();
				vConnection.AddParameter("@pCategoryTitle", SqlDbType.NVarChar, 50, pCategoryTitle);

				vCategory = vConnection.ExecuteProcedure<Category>("[CATEGORY_insert]", out int vReturn)[0];
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

			return vCategory;
		}

		private static void MethodError(string? pMethodName, Exception pException)
		{
			Console.WriteLine($"{typeof(CategoryService)}.{pMethodName} : {pException.Message}");
		}

	}
}