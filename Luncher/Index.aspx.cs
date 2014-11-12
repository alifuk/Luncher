using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Luncher
{
    public partial class Index : System.Web.UI.Page
    {
        List<CheckBox> listCb = new List<CheckBox>(); //List chechboxů vedle jídel
        DataTable jidla = new DataTable(); //tabulka jídel 

        protected void Page_Load(object sender, EventArgs e)
        {

            string výdejna = "";

            if (rbHarcov.Checked)
            {
                výdejna = "harcov";
            }
            else if (rbHusova.Checked)
            {
                výdejna = "husova";
            }
            else
            {
                výdejna = "voronezska";
            }

            DateTime datum;
            if (rbDnes.Checked)
            {
                datum = DateTime.UtcNow; //Dnešní datum
            }
            else
            {
                datum = DateTime.UtcNow.AddDays(1); //Zítřejší datum
            }

            int rok = datum.Year;
            int mesic = datum.Month;
            int den = datum.Day;


            //Webclient stáhne zdrojový kod stránky
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;

            string html = client.DownloadString("https://menza.tul.cz/"+výdejna+"/obedy/" + rok.ToString() + "-" + mesic.ToString() + "-" + den.ToString() + "/");


            //Zjištění odjakého indexu jsou večeře
            int vecereIndex = html.IndexOf("<h3 class=\"v\"><span>Večeře");
            int minutkyIndex = html.IndexOf("h3 class=\"m\"");

            if (vecereIndex == -1)
            {
                vecereIndex = html.Length - 1;
            }
            if (minutkyIndex == -1)
            {
                minutkyIndex = html.Length - 1;
            }
            int odIndexu = 0;
            int indexPorce = 0;
            int indexKoncePorci = 0;
            int indexJidla = 0;
            int indexKonceJidla = 0;

            int volnychObedu = 0;
            int volnychVeceri = 0;

            
            jidla.Columns.Add("dostupny", typeof(bool));
            jidla.Columns.Add("jmeno", typeof(string));
            jidla.Columns.Add("obed", typeof(bool));


            while ((indexPorce = html.IndexOf("<li class=\"porce\">", odIndexu)) != -1 && indexPorce < minutkyIndex)
            {
                bool obed;

                if (indexPorce < vecereIndex)
                {
                    obed = true;
                }
                else
                {
                    obed = false;
                }

                indexKoncePorci = html.IndexOf("<", indexPorce + 2);
                string porci = html.Substring(indexPorce + 18, indexKoncePorci - indexPorce - 18);

                bool jidloJeVolne = false;
                if (porci.IndexOf("porcí") != -1 || porci.IndexOf("porce") != -1)
                {
                    jidloJeVolne = true;
                    if (indexPorce < vecereIndex)
                    {
                        volnychObedu++;
                    }
                    else
                    {
                        volnychVeceri++;
                    }

                }

                if (porci == "&nbsp;")
                {
                    jidloJeVolne = false;
                    porci = "";
                }

                
                odIndexu = indexPorce + 1;


                indexJidla = html.IndexOf("<li class=\"nazev\">", odIndexu);
                indexKonceJidla = html.IndexOf("<", indexJidla + 2);
                string nazevJidla = html.Substring(indexJidla + 18, indexKonceJidla - indexJidla - 18);
                

                jidla.Rows.Add(jidloJeVolne, nazevJidla, obed);

            }

            lbStatus.Text = "Volných obědů: " + volnychObedu.ToString() + " Volných večeří: " + volnychVeceri.ToString();
            
            if (cbUpozornitObed.Checked && volnychObedu > 0)
            {
                Response.Write("<audio autoplay >  <source src=\"http://soundjax.com/reddo/71351%5Efoodfght.mp3\" type=\"audio/mpeg\"></audio>"); //loop
            }

            if (cbUpozornitVeceri.Checked && volnychVeceri > 0)
            {
                Response.Write("<audio autoplay >  <source src=\"http://soundjax.com/reddo/71351%5Efoodfght.mp3\" type=\"audio/mpeg\"></audio>"); //loop
            }

            if (cbUpozornitObed.Checked || cbUpozornitVeceri.Checked)
            {
                Response.Write("<script>setTimeout(\"window.location.reload()\", 5000);</script>");
            }

            Table tabulkaJidel = new Table();
            int i = 0;
            bool vydatZvuk = false;
            bool nadpisVecere = false;
            PridejNadpisDoTabulky(tabulkaJidel, "Oběd");
            foreach (DataRow jidlo in jidla.Rows)
            {
                
                if (!nadpisVecere && jidlo[2].ToString() == "False")
                {
                    nadpisVecere = true;
                    PridejNadpisDoTabulky(tabulkaJidel, "Večeře");
                }

                TableRow radek = new TableRow();
                TableCell bunka1 = new TableCell();
                TableCell bunka2 = new TableCell();

                Label nazev = new Label();
                nazev.Text = jidlo[1].ToString();
                if (jidlo[0].ToString() == "True") //je dostupný to jídlo
                {
                    nazev.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    nazev.ForeColor = System.Drawing.Color.Red;
                }
                bunka1.Controls.Add(nazev);
                CheckBox cbJidla = new CheckBox();
                cbJidla.ID = "cbJidlo_" + i.ToString();
                cbJidla.AutoPostBack = true;
                listCb.Add(cbJidla);
                bunka2.Controls.Add(cbJidla);
                radek.Controls.Add(bunka1);
                radek.Controls.Add(bunka2);
                tabulkaJidel.Controls.Add(radek);

                i++;

            }
            phMenu.Controls.Add(tabulkaJidel);

            if (vydatZvuk)
            {

                Response.Write("<audio autoplay >  <source src=\"http://soundjax.com/reddo/71351%5Efoodfght.mp3\" type=\"audio/mpeg\"></audio>"); //loop

                Response.Write("<script>setTimeout(\"window.location.reload()\", 5000);</script>");
            }
        }

        protected void Page_SaveStateComplete(object sender, EventArgs e)
        {
            bool vydatZvuk = false;
            bool nejakeZaskrtly = false;
            int i = 0;
            foreach (DataRow jidlo in jidla.Rows)
            {
                CheckBox cb = (CheckBox)listCb[i];
                if (cb.Checked)
                {
                    nejakeZaskrtly = true;
                }
                if (cb.Checked && jidlo[0].ToString() == "True")
                {
                    vydatZvuk = true;
                }



                i++;
            }

            if (vydatZvuk)
            {

                Response.Write("<audio autoplay >  <source src=\"http://soundjax.com/reddo/71351%5Efoodfght.mp3\" type=\"audio/mpeg\"></audio>"); //loop
            }
            if (nejakeZaskrtly)
            {
                Response.Write("<script>setTimeout(\"window.location.reload()\", 5000);</script>");
            }

        }

        

        public void PridejNadpisDoTabulky(Table tabulka, String textNadpisu)
        {

            TableRow radeknadpis = new TableRow();
            TableCell bunka1nadpis = new TableCell();
            TableCell bunka2nadpis = new TableCell();
            Label nadpis = new Label();
            nadpis.Text = "<h1>"+textNadpisu+": </h1>";
            Label empty = new Label();
            empty.Text = "";
            bunka1nadpis.Controls.Add(nadpis);
            bunka2nadpis.Controls.Add(empty);
            radeknadpis.Controls.Add(bunka1nadpis);
            radeknadpis.Controls.Add(bunka2nadpis);
            tabulka.Controls.Add(radeknadpis);
        }
    }
}