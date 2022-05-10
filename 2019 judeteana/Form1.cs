using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace _2019_judeteana
{
    public partial class Form1 : Form
    {
        public string connx;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            connx = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=" + path +@"\FreeBook.mdf;Integrated Security=True;Connect Timeout=30";
            SqlConnection con = new SqlConnection(connx);
            con.Open();
            string[] liniiuti = File.ReadAllLines("utilizatori.txt");
            string[] liniicarti = File.ReadAllLines("carti.txt");
            string[] imprumuturi = File.ReadAllLines("imprumuturi.txt");
            SqlCommand inituti = new SqlCommand("delete from utilizatori", con);
            inituti.ExecuteNonQuery();
            SqlCommand initcarti = new SqlCommand("delete from carti", con);
            initcarti.ExecuteNonQuery();
            SqlCommand initimprumuturi = new SqlCommand("delete from imprumut", con);
            initimprumuturi.ExecuteNonQuery();


            for (int i = 0; i < liniiuti.Length; i++)
            {
                Utilizator ut = new Utilizator(liniiuti[i]);
                SqlCommand insuti = new SqlCommand("insert into utilizatori (email,parola,nume,prenume) values (@email,@parola,@nume,@prenume)", con);
                insuti.Parameters.AddWithValue("@email", ut.email);
                insuti.Parameters.AddWithValue("@parola", ut.parola);
                insuti.Parameters.AddWithValue("@nume", ut.nume);
                insuti.Parameters.AddWithValue("@prenume", ut.prenume);
                insuti.ExecuteNonQuery();
            }
            for (int i = 0; i < liniicarti.Length; i++)
            {
                Carte car = new Carte(liniicarti[i]);
                SqlCommand inscar = new SqlCommand("insert into carti(id_carte,titlu,autor,gen) values (@id,@titlu,@autor,@gen)", con);
                inscar.Parameters.AddWithValue("@id", i.ToString());
                inscar.Parameters.AddWithValue("@titlu", car.titlu);
                inscar.Parameters.AddWithValue("@autor", car.autor);
                inscar.Parameters.AddWithValue("@gen", car.gen);
                inscar.ExecuteNonQuery();

            }
            for (int i = 0; i < imprumuturi.Length; i++)
            {
                Imprumut impr = new Imprumut(imprumuturi[i]);
                SqlCommand insimpr = new SqlCommand("insert into imprumut(id_carte,id_imprumut,email,data_imprumut) select id_carte,@id,@email,@data from carti where titlu=@titlu", con);
                insimpr.Parameters.AddWithValue("@id", i);
                insimpr.Parameters.AddWithValue("@email", impr.email);
                insimpr.Parameters.AddWithValue("@data", impr.data);
                insimpr.Parameters.AddWithValue("@titlu", impr.titlu);
                insimpr.ExecuteNonQuery();
            }




        }


        public class Utilizator{
            public string email;
            public string parola;
            public string nume;
            public string prenume;
            public Utilizator(string linie){
                string[] r = linie.Split('*');
                email = r[0];
                parola = r[1];
                nume = r[2];
                prenume = r[3];
            }
        }

        public class Carte
        {
            public string titlu;
            public string autor;
            public string gen;
            public Carte(string linie)
            {
                string[] r = linie.Split('*');
                titlu = r[0];
                autor = r[1];
                gen = r[2];
            }
        }

        public class Imprumut
        {
            public string titlu;
            public string email;
            public string data;
            public Imprumut(string linie)
            {
                string[] r=linie.Split('*');
                titlu = r[0];
                email = r[1];
                data = r[2];
            }
        }
    }
}
