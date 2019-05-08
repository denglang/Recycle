using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.Entity;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Recycle.Models
{
    public class RecycleContext : DbContext
    {
        public DbSet<RecycleSites> RecycleSites { get; set; }

        public void AddRecycleSite(RecycleSites sites)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RecycleContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAddRecycleSite", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@Member_Name";
                paramName.Value = sites.Member_Name;
                cmd.Parameters.Add(paramName);

                SqlParameter paramMail = new SqlParameter();
                paramMail.ParameterName = "@Contact_Email";
                paramMail.Value = sites.Contact_Email;
                cmd.Parameters.Add(paramMail);

                SqlParameter paramPhone = new SqlParameter();
                paramPhone.ParameterName = "@Phone_Number";
                paramPhone.Value = sites.Phone_Number;
                cmd.Parameters.Add(paramPhone);

                SqlParameter paramWebsite = new SqlParameter();
                paramWebsite.ParameterName = "@website";
                paramWebsite.Value = sites.Website;
                cmd.Parameters.Add(paramWebsite);

                SqlParameter paramStreet = new SqlParameter();
                paramStreet.ParameterName = "@Street";
                paramStreet.Value = sites.Street;
                cmd.Parameters.Add(paramStreet);

                SqlParameter paramCity = new SqlParameter();
                paramCity.ParameterName = "@City";
                paramCity.Value = sites.City;
                cmd.Parameters.Add(paramCity);

                SqlParameter paramZip = new SqlParameter();
                paramZip.ParameterName = "@Zip_Code";
                paramZip.Value = sites.Zip_Code;
                cmd.Parameters.Add(paramZip);

                SqlParameter paramCounty = new SqlParameter();
                paramCounty.ParameterName = "@County";
                paramCounty.Value = sites.County;
                cmd.Parameters.Add(paramCounty);

                SqlParameter paramState = new SqlParameter();
                paramState.ParameterName = "@State";
                paramState.Value = sites.State;
                cmd.Parameters.Add(paramState);

                SqlParameter paramStatus = new SqlParameter();
                paramStatus.ParameterName = "@Status_1";
                paramStatus.Value = sites.Status_1;
                cmd.Parameters.Add(paramStatus);

                SqlParameter paramHours = new SqlParameter();
                paramHours.ParameterName = "@Profile_Hours";
                paramHours.Value = sites.Profile_Hours;
                cmd.Parameters.Add(paramHours);

                SqlParameter paramConstruction = new SqlParameter();
                paramConstruction.ParameterName = "@Construction_Demolition";
                paramConstruction.Value = sites.Construction_Demolition;
                cmd.Parameters.Add(paramConstruction);

                SqlParameter paramHazardous = new SqlParameter();
                paramHazardous.ParameterName = "@Hazardous_Waste";
                paramHazardous.Value = sites.Hazardous_Waste;
                cmd.Parameters.Add(paramHazardous);

                SqlParameter paramOrganic = new SqlParameter();
                paramOrganic.ParameterName = "@Organics";
                paramOrganic.Value = sites.Organics;
                cmd.Parameters.Add(paramOrganic);

                SqlParameter paramSolid = new SqlParameter();
                paramSolid.ParameterName = "@Solid_Waste";
                paramSolid.Value = sites.Solid_Waste;
                cmd.Parameters.Add(paramSolid);

                SqlParameter paramAddressWeb = new SqlParameter();
                paramAddressWeb.ParameterName = "@Address_Website";
                paramAddressWeb.Value = sites.Address_Website;
                cmd.Parameters.Add(paramAddressWeb);

                SqlParameter paramElectronics = new SqlParameter();
                paramElectronics.ParameterName = "@Electronics";
                paramElectronics.Value = sites.Electronics;
                cmd.Parameters.Add(paramElectronics);

                con.Open();
                cmd.ExecuteNonQuery();

            }
        }

        public void SaveRecycleSite(RecycleSites sites)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RecycleContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spSaveRecycleSite", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramId = new SqlParameter();
                paramId.ParameterName = "@Id";
                paramId.Value = sites.ObjectID;
                cmd.Parameters.Add(paramId);

                SqlParameter paramName = new SqlParameter();
                paramName.ParameterName = "@Member_Name";
                paramName.Value = sites.Member_Name;
                cmd.Parameters.Add(paramName);

                SqlParameter paramMail = new SqlParameter();
                paramMail.ParameterName = "@Contact_Email";
                paramMail.Value = sites.Contact_Email;
                cmd.Parameters.Add(paramMail);

                SqlParameter paramPhone = new SqlParameter();
                paramPhone.ParameterName = "@Phone_Number";
                paramPhone.Value = sites.Phone_Number;
                cmd.Parameters.Add(paramPhone);

                SqlParameter paramWebsite = new SqlParameter();
                paramWebsite.ParameterName = "@website";
                paramWebsite.Value = sites.Website;
                cmd.Parameters.Add(paramWebsite);

                SqlParameter paramStreet = new SqlParameter();
                paramStreet.ParameterName = "@Street";
                paramStreet.Value = sites.Street;
                cmd.Parameters.Add(paramStreet);

                SqlParameter paramCity = new SqlParameter();
                paramCity.ParameterName = "@City";
                paramCity.Value = sites.City;
                cmd.Parameters.Add(paramCity);

                SqlParameter paramZip = new SqlParameter();
                paramZip.ParameterName = "@Zip_Code";
                paramZip.Value = sites.Zip_Code;
                cmd.Parameters.Add(paramZip);

                SqlParameter paramCounty = new SqlParameter();
                paramCounty.ParameterName = "@County";
                paramCounty.Value = sites.County;
                cmd.Parameters.Add(paramCounty);

                SqlParameter paramState = new SqlParameter();
                paramState.ParameterName = "@State";
                paramState.Value = sites.State;
                cmd.Parameters.Add(paramState);

                SqlParameter paramStatus = new SqlParameter();
                paramStatus.ParameterName = "@Status_1";
                paramStatus.Value = sites.Status_1;
                cmd.Parameters.Add(paramStatus);

                SqlParameter paramHours = new SqlParameter();
                paramHours.ParameterName = "@Profile_Hours";
                paramHours.Value = sites.Profile_Hours;
                cmd.Parameters.Add(paramHours);

                SqlParameter paramConstruction = new SqlParameter();
                paramConstruction.ParameterName = "@Construction_Demolition";
                paramConstruction.Value = sites.Construction_Demolition;
                cmd.Parameters.Add(paramConstruction);

                SqlParameter paramHazardous = new SqlParameter();
                paramHazardous.ParameterName = "@Hazardous_Waste";
                paramHazardous.Value = sites.Hazardous_Waste;
                cmd.Parameters.Add(paramHazardous);

                SqlParameter paramOrganic = new SqlParameter();
                paramOrganic.ParameterName = "@Organics";
                paramOrganic.Value = sites.Organics;
                cmd.Parameters.Add(paramOrganic);

                SqlParameter paramSolid = new SqlParameter();
                paramSolid.ParameterName = "@Solid_Waste";
                paramSolid.Value = sites.Solid_Waste;
                cmd.Parameters.Add(paramSolid);

                SqlParameter paramAddressWeb = new SqlParameter();
                paramAddressWeb.ParameterName = "@Address_Website";
                paramAddressWeb.Value = sites.Address_Website;
                cmd.Parameters.Add(paramAddressWeb);

                SqlParameter paramElectronics = new SqlParameter();
                paramElectronics.ParameterName = "@Electronics";
                paramElectronics.Value = sites.Electronics;
                cmd.Parameters.Add(paramElectronics);

                con.Open();
                cmd.ExecuteNonQuery();

            }
        }

        public void DeleteRecycleSite(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["RecycleContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spDeleteRecycleSite", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramId = new SqlParameter();
                paramId.ParameterName = "@Id";
                paramId.Value = id;
                cmd.Parameters.Add(paramId);

                con.Open();
                cmd.ExecuteNonQuery();

            }

        }
    }

    
    
}