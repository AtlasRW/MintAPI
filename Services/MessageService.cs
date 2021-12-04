using System.Data;
using Mint.API.Models;
using System.Reflection;
using System.Data.SqlClient;

namespace Mint.API.Services
{
    public static class MessageService
    {

        /// <summary>
        /// Récupérer l'ensemble des messages en cours
        /// </summary>
        /// <returns>Liste de Messages en cours</returns>
        public static List<Message> GetAllCurrent()
        {
            List<Message> vMessages = new List<Message>();
            DBConnection? vConnection = null;

            try
            {
                vConnection = new DBConnection();

                SqlDataReader vReader = vConnection.ExecuteProcedure("[MESSAGE_getAllCurrent]");
                Message vMessage = new Message();
                while (vMessage.Read(vReader))
                {
                    vMessages.Add(vMessage);
                    vMessage = new Message();
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

            return vMessages;
        }

        /// <summary>
        /// Récupérer l'ensemble des messages, optionnellement triés par catégories
        /// </summary>
        /// <param name="pCategories">Liste d'ID des catégories</param>
        /// <param name="pIsCurrent">Booléen pour trier les messages en cours ou non</param>
        /// <param name="pIsDraft">Booléen pour trier les messages en brouillon ou non</param>
        /// <returns>Liste de Messages</returns>
        public static List<Message> GetAll(
            List<int> pCategories,
            bool? pIsCurrent = null,
            bool? pIsDraft = null
        )
        {
            List<Message> vMessages = new List<Message>();
            DBConnection? vConnection = null;

            try
            {
                vConnection = new DBConnection();
                if (pIsCurrent != null)
                    vConnection.AddParameter("@pIsCurrent", SqlDbType.Bit, pIsCurrent);
                if (pIsDraft != null)
                    vConnection.AddParameter("@pIsDraft", SqlDbType.Bit, pIsDraft);

                DataTable vCategoriesTable = new DataTable();
                vCategoriesTable.Columns.Add(new DataColumn("[ID]", typeof(int)));
                foreach (int vCategory in pCategories)
                    vCategoriesTable.Rows.Add(vCategory);
                vConnection.AddParameter("@pCategories", "[ID_list]", vCategoriesTable);

                SqlDataReader vReader = vConnection.ExecuteProcedure("[MESSAGE_getAll]");
                Message vMessage = new Message();
                while (vMessage.Read(vReader))
                {
                    vMessages.Add(vMessage);
                    vMessage = new Message();
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

            return vMessages;
        }

        /// <summary>
        /// Retourne un message via son ID
        /// </summary>
        /// <param name="pMessageID">ID du message</param>
        /// <returns>Message</returns>
        public static Message? GetById(int pMessageID)
        {
            Message? vMessage = null;
            DBConnection? vConnection = null;

            try
            {
                vConnection = new DBConnection();
                vConnection.AddParameter("@pMessageID", SqlDbType.Int, pMessageID);

                SqlDataReader vReader = vConnection.ExecuteProcedure("[MESSAGE_getById]");
                vMessage = new Message();
                if (!vMessage.Read(vReader))
                    vMessage = null;

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

            return vMessage;
        }

        /// <summary>
        /// Ajoute un message, ou le met à jour si existe déjà
        /// </summary>
        /// <param name="pMessageID">ID du message, si il existe déjà</param>
        /// <param name="pMessageTitle">Titre du message</param>
        /// <param name="pMessageIsDraft">Booléen de l'état brouillon</param>
        /// <param name="pMessageHeader">En-tête du message</param>
        /// <param name="pMessageBody">Corps du message</param>
        /// <param name="pMessageBackground">Arrière-plan du message</param>
        /// <param name="pMessageStartDate">Date de début du message</param>
        /// <param name="pMessageEndDate">Date de fin du message</param>
        /// <param name="pMessageIconID">ID de l'icône du message</param>
        /// <param name="pMessageActionID">ID de l'action du message</param>
        /// <param name="pMessageCategories">Liste d'ID des catégories</param>
        /// <param name="pResult">Résultat de la requête SQL</param>
        /// <returns>Message</returns>
        public static Message? UpdateOrInsert(
            int? pMessageID,
            string pMessageTitle,
            bool pMessageIsDraft,
            string? pMessageHeader,
            string? pMessageBody,
            string? pMessageBackground,
            DateTime? pMessageStartDate,
            DateTime? pMessageEndDate,
            int? pMessageIconId,
            int? pMessageActionId,
            List<int> pMessageCategories,
            out bool pResult
        )
        {
            Message? vMessage = null;
            DBConnection? vConnection = null;

            try
            {
                vConnection = new DBConnection();
                vConnection.AddParameter("@pMessageID", SqlDbType.Int, pMessageID ?? null);
                vConnection.AddParameter("@pMessageTitle", SqlDbType.NVarChar, 30, pMessageTitle);
                vConnection.AddParameter("@pMessageIsDraft", SqlDbType.Bit, pMessageIsDraft);
                vConnection.AddParameter("@pMessageHeader", SqlDbType.NVarChar, 100, pMessageHeader);
                vConnection.AddParameter("@pMessageBody", SqlDbType.NVarChar, 200, pMessageBody);
                vConnection.AddParameter("@pMessageBackground", SqlDbType.NChar, 6, pMessageBackground);
                vConnection.AddParameter("@pMessageStartDate", SqlDbType.DateTime, pMessageStartDate);
                vConnection.AddParameter("@pMessageEndDate", SqlDbType.DateTime, pMessageEndDate);
                vConnection.AddParameter("@pMessageIconID", SqlDbType.Int, pMessageIconId);
                vConnection.AddParameter("@pMessageActionID", SqlDbType.Int, pMessageActionId);

                DataTable vCategoriesTable = new DataTable();
                vCategoriesTable.Columns.Add(new DataColumn("[ID]", typeof(int)));
                foreach (int vMessageCategory in pMessageCategories)
                    vCategoriesTable.Rows.Add(vMessageCategory);
                vConnection.AddParameter("@pMessageCategories", "[ID_list]", vCategoriesTable);

                SqlDataReader vReader = vConnection.ExecuteProcedure("[MESSAGE_updateOrInsert]", out int vReturn);
                pResult = vReturn < 0 ? false : true;
                vMessage = new Message();
                if (!vMessage.Read(vReader))
                    vMessage = null;

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

            return vMessage;
        }

        private static void MethodError(string? pMethodName, Exception pException)
        {
            Console.WriteLine($"{typeof(MessageService)}.{pMethodName} : {pException.Message}");
        }

    }
}