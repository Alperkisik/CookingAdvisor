using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingAdvisor
{
    class mail
    {
        public string message,header,sender,receiver;

        public void set(string Message,string Header,string Sender,string Receiver)
        {
            message = Message;
            header = Header;
            sender = Sender;
            receiver = Receiver;
        }

        public void sent()
        {
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO Mail values (@sender,@receiver,@header,@message)", con);
            cmd.Parameters.AddWithValue("@sender", sender);
            cmd.Parameters.AddWithValue("@receiver", receiver);
            cmd.Parameters.AddWithValue("@header", header);
            cmd.Parameters.AddWithValue("@message", message);
            cmd.ExecuteNonQuery();

            con.Close();
        }
    }
}
