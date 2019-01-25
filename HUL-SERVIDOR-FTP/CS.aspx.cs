
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Net;
using System.Data;
using System.Globalization;


public partial class CS : System.Web.UI.Page
{


    protected void Page_Load(object sender, EventArgs e)
    {


        try
        {
            List<string> entries = new List<string>();

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://10.0.0.101:2323/BB/comprovantes/");
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;


            request.Credentials = new NetworkCredential("", "");
            request.UsePassive = true;
            request.UseBinary = true;
            request.EnableSsl = false;


            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {

                entries = reader.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }




            DataTable dtFiles = new DataTable();

            dtFiles.Columns.AddRange(new DataColumn[3] { new DataColumn("Arquivo", typeof(string)), new DataColumn("Name", typeof(string)), new DataColumn("Date", typeof(string)) });




            foreach (string entry in entries)
            {


                string[] splits = entry.Split(new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries);
                bool isFile = splits[0].Substring(0, 1) != "d";
                bool isDirectory = splits[0].Substring(0, 1) == "d";
                if (isFile)
                {
                    dtFiles.Rows.Add();


                    dtFiles.Rows[dtFiles.Rows.Count - 1]["Date"] = string.Join(splits[5], splits[6], splits[7]);
                    string name = string.Empty;
                    for (int i = 8; i < splits.Length; i++)
                    {
                        name = string.Join(" ", name, splits[i]);
                    }
                    dtFiles.Rows[dtFiles.Rows.Count - 1]["Name"] = name.Trim();
                    dtFiles.Rows[dtFiles.Rows.Count - 1]["Arquivo"] = name.Substring(13, 6);//name.Substring(17, 7);
                    var request2 = (FtpWebRequest)WebRequest.Create("ftp://10.0.0.101:2323/BB/comprovantes/" + name.Trim());
                    request2.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                    var response2 = (FtpWebResponse)request2.GetResponse();
                    var data = response2.LastModified;
                    dtFiles.Rows[dtFiles.Rows.Count - 1]["Date"] = data.ToString();

                }

                ViewState["myViewState"] = dtFiles;

            }



            gvFiles.DataSource = dtFiles;
            gvFiles.DataBind();
        }
        catch (WebException ex)
        {
            throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
        }
    }
    protected void DownloadFile(object sender, EventArgs e)
    {
        string fileName = (sender as LinkButton).CommandArgument;


        try
        {


            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://10.0.0.101:2323/BB/comprovantes/" + fileName);
            request.Method = WebRequestMethods.Ftp.DownloadFile;



            request.Credentials = new NetworkCredential("", "");
            request.UsePassive = true;
            request.UseBinary = true;
            request.EnableSsl = false;


            FtpWebResponse response = (FtpWebResponse)request.GetResponse();



            using (MemoryStream stream = new MemoryStream())
            {



                Response.Clear();
                Response.ContentType = "PDF";
                response.GetResponseStream().CopyTo(stream);
                Response.AddHeader("content-disposition", "inline; filename=" + fileName);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(stream.ToArray());
                Response.End();
            }
        }
        catch (WebException ex)
        {
            throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
        }


    }
    protected void searchButton_Click(object sender, EventArgs e)
    {
        string searchTerm = searchBox.Text.ToLower();


        DataTable dt = ViewState["myViewState"] as DataTable;



        DataTable dtNew = dt.Clone();


        foreach (DataRow row in dt.Rows)
        {

            if (row["Name"].ToString().ToLower().Contains(searchTerm) || row["Date"].ToString().ToLower().Contains(searchTerm))
            {

                dtNew.Rows.Add(row.ItemArray);
            }
        }

        gvFiles.DataSource = dtNew;
        gvFiles.DataBind();

    }
    protected void resetSearchButton_Click(object sender, EventArgs e)
    {


        if (ViewState["myViewState"] == null)
            return;



        DataTable dt = ViewState["myViewState"] as DataTable;


        gvFiles.DataSource = dt;
        gvFiles.DataBind();
    }
    protected void gridPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvFiles.PageIndex = e.NewPageIndex;
        gvFiles.DataBind();
    }




}

