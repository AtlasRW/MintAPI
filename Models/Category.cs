using System.Data;
using System.Reflection;
using Mint.API.Services;
using System.Data.SqlClient;

namespace Mint.API.Models
{
    public class Category
	{

		#region PUBLIC PROPERTIES

		public int Id { get; set; }
		public string Title { get; set; } = "";

		#endregion

		#region PUBLIC METHODS

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

		#endregion

		#region PRIVATE METHODS

		public bool Read(SqlDataReader pReader)
		{
			if (pReader == null || !pReader.Read()) return false;

			Id		= pReader.GetInt32(pReader.GetOrdinal("CATEGORY_id"));
			Title	= pReader.GetString(pReader.GetOrdinal("CATEGORY_title"));

			return true;
		}

		private static void MethodError(string? pMethodName, Exception pException)
		{
			Console.WriteLine($"{typeof(Category)}.{pMethodName} : {pException.Message}");
		}

		#endregion

	}
}