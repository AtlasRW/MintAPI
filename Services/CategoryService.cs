using System.Data;
using Mint.API.Models;
using System.Reflection;
using System.Data.SqlClient;

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
			List<Category> vResults = new List<Category>();
			DBConnection? vConnection = null;

			try
			{
				vConnection = new DBConnection();

				SqlDataReader vReader = vConnection.ExecuteProcedure("[CATEGORY_getAll]");
				Category vCategory = new Category();
				while (vCategory.Read(vReader))
				{
					vResults.Add(vCategory);
					vCategory = new Category();
				}

				vReader.Close();
			}
			catch (Exception pException)
			{
				MethodError(MethodBase.GetCurrentMethod()?.Name, pException);
			}
			finally
			{
				if (vConnection != null) vConnection.Close();
			}

			return vResults;
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

				SqlDataReader vReader = vConnection.ExecuteProcedure("[CATEGORY_insert]", out int vReturn);
				pResult = vReturn < 0 ? false : true;
				vCategory = new Category();
				if (!vCategory.Read(vReader))
					vCategory = null;

				vReader.Close();
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