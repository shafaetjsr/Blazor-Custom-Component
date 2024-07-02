using BlazoriseAppCompo.Data;
using GGAPost.Security;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;

namespace GGAPost.Data.DbObject
{
	public class DALService
	{
		DBhelper db;

		public DataTable GetDataTable(string sql)
		{
			DataTable dt = new DataTable();
			db = new DBhelper();
			dt = db.ExecuteSQLDatatable(sql);

			return dt;
		}

		public int ExecuteCommand(string command)
		{
			db = new DBhelper();
			return db.ExecuteCommand(command);
		}

		public DataTable doLogin(string userId, string pass)
		{
			string password = EncryptionHelper.doEncrypt(pass);
			DataTable dt = new DataTable();
			db = new DBhelper();
			string sql = $@"EXEC sp_login '{userId}','{password}'";

			dt = db.ExecuteSQLDatatable(sql);

			return dt;
		}
		public string doEncrypt(string value)
		{
			string encryptValue = EncryptionHelper.doEncrypt(value);
			return encryptValue;
		}
		public string doDeccrypt(string encryptValue)
		{
			string eValue = EncryptionHelper.doDecrypt(encryptValue);
			return eValue;
		}

		//public DataTable SaveSales(trn_master t, List<SalesEntity> sList)
		//{
		//    DataTable resultDataTable = new DataTable();
		//    try
		//    {          
		//        using (SqlConnection connection = new SqlConnection(DbConnection.CName))
		//        {
		//            connection.Open();
		//            using (SqlTransaction transaction = connection.BeginTransaction())
		//            {
		//                try
		//                {
		//                    DataTable dt = new DataTable();
		//                    string vCode = "";
		//                    string vMsg = "Save Sp Does Not return anything.";
		//                    string traceno = "";
		//                    string query = $@"Exec sp_save_trn_mstr '{t.customer_id}',{t.total_amount},{t.discount_amount},{t.grand_total},'{t.trn_type}','{t.remark}','{t.user_code}','{t.tdate.ToString("dd-MMM-yyyy")}'";

		//                    using (SqlCommand command = new SqlCommand(query, connection, transaction))
		//                    {
		//                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
		//                        {
		//                            adapter.Fill(dt);
		//                        }
		//                    }
		//                    if(dt.hasRow())
		//                    {
		//                        vCode = dt.Rows[0]["vCode"].toString();
		//                        vMsg = dt.Rows[0]["vMsg"].toString();
		//                        traceno = dt.Rows[0]["traceno"].toString();
		//                    }

		//                    if (t.trn_type == "INC")
		//                    {
		//                        if (vCode == "1")
		//                        {
		//                            foreach (var s in sList)
		//                            {
		//                                vCode = "";
		//                                vMsg = "";
		//                                dt = new DataTable();
		//                                query = $@"exec sp_save_sales_details '{traceno}','{s.ProductCode}','{s.Quantity}','{s.SalePrice}'";

		//                                using (SqlCommand command = new SqlCommand(query, connection, transaction))
		//                                {
		//                                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
		//                                    {
		//                                        adapter.Fill(dt);
		//                                    }
		//                                }
		//                                if (dt.hasRow())
		//                                {
		//                                    vCode = dt.Rows[0]["vCode"].toString();
		//                                    vMsg = dt.Rows[0]["vMsg"].toString();
		//                                }

		//                                if (vCode == "1")
		//                                {
		//                                    continue;
		//                                }
		//                                else
		//                                {
		//                                    if(string.IsNullOrEmpty(vMsg))
		//                                    {
		//                                        resultDataTable = getRvDT("0", "Insert Failed with Details Table");
		//                                    }
		//                                    else
		//                                    {
		//                                        resultDataTable = getRvDT("0", vMsg);
		//                                    }

		//                                    transaction.Rollback();
		//                                    return resultDataTable;
		//                                }
		//                            }
		//                        }
		//                        else
		//                        {
		//                            resultDataTable = getRvDT("0", vMsg);
		//                            transaction.Rollback();
		//                            return resultDataTable;
		//                        }
		//                    }

		//                    resultDataTable = getRvDT("1", "Data Save Successfully");
		//                    transaction.Commit();
		//                }
		//                catch (Exception ex)
		//                {
		//                    resultDataTable = getRvDT("0", ex.Message);
		//                    transaction.Rollback();
		//                }
		//            }
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        resultDataTable = getRvDT("0", ex.Message);
		//    }

		//    return resultDataTable;
		//}

		private DataTable getRvDT(string vCode, string vMsg)
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("vCode");
			dt.Columns.Add("vMsg");

			DataRow dr = dt.NewRow();
			dr[0] = vCode;
			dr[1] = vMsg;
			dt.Rows.Add(dr);

			return dt;
		}

		//public DataTable SaveUser(Users u)
		//{
		//    string encpassword = EncryptionHelper.doEncrypt(u.password);

		//    DataTable dt = new DataTable();
		//    string sql = $@"EXEC sp_save_user '{u.user_id}','{u.user_name}','{u.designation}','{u.address}','{u.mobile}','{encpassword}'";
		//    dt = GetDataTable(sql);
		//    return dt;
		//}

		public DataTable resetPassword(string userId, string password)
		{
			DataTable dt = new DataTable();
			if (string.IsNullOrEmpty(password))
			{
				dt = getRvDT("0", "Please Input New Password.");
				return dt;
			}
			string encpassword = EncryptionHelper.doEncrypt(password);


			string sql = $@"exec sp_update_password '{userId}','{encpassword}'";
			dt = GetDataTable(sql);
			return dt;
		}

		//public DataTable SaveSales(Customer c,Sales_Master m, List<AddToCart> cList)
		//{
		//	DataTable resultDataTable = new DataTable();
		//	try
		//	{
		//		using (SqlConnection connection = new SqlConnection(DbConnection.CName))
		//		{
		//			connection.Open();
		//			using (SqlTransaction transaction = connection.BeginTransaction())
		//			{
		//				try
		//				{
		//					DataTable dt = new DataTable();
		//					string vCode = "";
		//					string vMsg = "Save Sp Does Not return anything.";
		//					string invoice_no = "";
		//					string query = $@"exec sp_save_sales_master '{c.customer_name}','{c.mobile_number}','{c.address}',{m.total_amount},{m.discount_amount},{m.sub_total_amount},{m.paid_amount},{m.grand_amount},'{m.tdate.ToString("dd-MMM-yyyy")}','{m.user_code}','{m.payment_type}'";

		//					using (SqlCommand command = new SqlCommand(query, connection, transaction))
		//					{
		//						using (SqlDataAdapter adapter = new SqlDataAdapter(command))
		//						{
		//							adapter.Fill(dt);
		//						}
		//					}
		//					if (dt.hasRow())
		//					{
		//						vCode = dt.Rows[0]["vCode"].toString();
		//						vMsg = dt.Rows[0]["vMsg"].toString();
		//						invoice_no = dt.Rows[0]["invoice_no"].toString();
		//					}

		//					if (vCode == "1")
		//					{
		//						foreach (var s in cList)
		//						{
		//							vCode = "";
		//							vMsg = "";
		//							dt = new DataTable();
		//							query = $@"exec sp_save_sales_details '{invoice_no}',{s.product_purchase_id},'{s.description}','{s.warranty}',{s.quantity},{s.unitPrice},{s.totalPrice},{s.purchase_price},'{s.serialNo}','{m.tdate.ToString("dd-MMM-yyyy")}'";

		//							using (SqlCommand command = new SqlCommand(query, connection, transaction))
		//							{
		//								using (SqlDataAdapter adapter = new SqlDataAdapter(command))
		//								{
		//									adapter.Fill(dt);
		//								}
		//							}
		//							if (dt.hasRow())
		//							{
		//								vCode = dt.Rows[0]["vCode"].toString();
		//								vMsg = dt.Rows[0]["vMsg"].toString();
		//							}

		//							if (vCode == "1")
		//							{
		//								continue;
		//							}
		//							else
		//							{
		//								if (string.IsNullOrEmpty(vMsg))
		//								{
		//									resultDataTable = getRvDT("0", "Insert Failed with Details Table");
		//								}
		//								else
		//								{
		//									resultDataTable = getRvDT("0", vMsg);
		//								}

		//								transaction.Rollback();
		//								return resultDataTable;
		//							}
		//						}
		//					}
		//					else
		//					{
		//						resultDataTable = getRvDT("0", vMsg);
		//						transaction.Rollback();
		//						return resultDataTable;
		//					}


		//					resultDataTable = getRvDT("1", $@"Data Save Successfully With Invoice No: {invoice_no}");
		//					transaction.Commit();
		//				}
		//				catch (Exception ex)
		//				{
		//					resultDataTable = getRvDT("0", ex.Message);
		//					transaction.Rollback();
		//				}
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		resultDataTable = getRvDT("0", ex.Message);
		//	}

		//	return resultDataTable;
		//}

		public async Task<List<SuggestionItem>> autocomplateData(string sqlqury, string searchKey)
		{
			List<SuggestionItem> s = new List<SuggestionItem>();
            //string sql = $@"SELECT product_type_id Value,product_model Text FROM dbo.product_model_tbl WHERE product_model LIKE '%{searchKey}%'";
            db = new DBhelper();
            DataTable dt =await db.getAutocompleteData(sqlqury, searchKey);

            var json = JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
			s = JsonConvert.DeserializeObject<List<SuggestionItem>>(json);

			return s;
		}
	}
}
