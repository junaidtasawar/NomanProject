using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebBasedSystem.Models;
using WebBasedSystem.Models.Shared;

namespace WebBasedSystem.ViewModels
{
    public class DbHandleWayPoints
    {
        public WayPoint GetNearestWayPoint(decimal? latitude, decimal? longitude)
        {
            //string connectionstring = @"Data Source=DESKTOP-JH5DU96;Initial Catalog=TestDb;Integrated Security=True";
            WayPoint wp = new WayPoint();
            using (SqlConnection conn = new SqlConnection(AppSettings.ConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("sp_NearestRoute", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    cmd.Parameters.AddWithValue("@lat", latitude);
                    cmd.Parameters.AddWithValue("@lng", longitude);


                    SqlDataReader sdr = cmd.ExecuteReader();


                    sdr.Read();

                        //link object to reader
                        WayPoint waypoint = new WayPoint();
                        waypoint.PostCode = sdr["PostCode"].ToString();
                        waypoint.State = sdr["State"].ToString();
                        waypoint.Suburbs = sdr["Suburbs"].ToString();

                    }


                

            }
            return wp;

        }
    }

}
