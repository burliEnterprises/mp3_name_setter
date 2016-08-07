using Id3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace Musik_Projekt
{
    public partial class ErsteForm : MetroForm
    {
                                                    //Variablendeklarationen
        string pfad;
        FolderBrowserDialog suchePfad;
        CL_DateienAnzeigen x,y,z;
        string[] alleTitel, alleArtists, mischungTitelArtist;
        int anzahlMP3imPfad;


        public ErsteForm()
        {
            InitializeComponent();
        }







        private void btnAlleNamen_Click(object sender, EventArgs e)    //Alle Dateien anzeigen Button
        {
            
                suchePfad = new FolderBrowserDialog();   //Dialog, um Ordner auszuwählen
                suchePfad.ShowDialog();

            try {                                   //Versuche auszuführen, wenn ArgumentException (Klick auf Abbrechen) -> DialogBox
                pfad = suchePfad.SelectedPath;          //Gibt den Pfad des gewählten Ordners zurück
                x = new CL_DateienAnzeigen(pfad);

                CL_DateienAnzeigen.txt1 = lbDateien;  //Übergibt ListBox an eine Variable der CL_DateienAnzeigen
                x.inListBoxSchreiben();

                tbPfad.Text = "Pfad: " + pfad;
            }
            catch (ArgumentException)       //Wenn bei OrdnerWahl auf Abbrechens
            {
                DialogResult dialogResult = MessageBox.Show("Abort?", "You did not choose a directory", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    btnAlleNamen_Click(this, null);     //Führt den "Button Klick" nochmals aus, dieser Sender, keine EventArgumente
                }
            }
            
            
        }







        private void button1_Click(object sender, EventArgs e)   //ListBoxReset
        {
            lbDateien.Items.Clear();      //ListBox leeren
            tbPfad.Text = "";           //Pfadtextbox leer setzen
        }










        private void versionsinfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }









        private void btnArtistPlusTitel_Click(object sender, EventArgs e)   //In Artist+Titel unbenennen
        {
            try
            {//Wenn kein Ordner ausgewählt MessageBox
                z = new CL_DateienAnzeigen(pfad);

                CL_DateienAnzeigen.txt1 = lbDateien;  //Übergibt ListBox an eine Variable der CL_DateienAnzeigen

                anzahlMP3imPfad = z.getanzahlMP3inPfad();   //Anzahl der MP3s im angegebenen Pfad zurückgegeben
                alleTitel = new string[anzahlMP3imPfad];
                alleTitel = z.TitelFinden();               //Gibt alle Titel-Tags zurück
                alleArtists = z.ArtistFinden();              //Gibt alle Artist-Tags zurück

                mischungTitelArtist = z.TitelArtistZusammensetzen(alleTitel, alleArtists, "ArtistFirst");
                //Holt sich zusammengesetzter Array aus Titel+Artist

                z.NameToTitle(mischungTitelArtist);              //Ändert den Dateiname zum Titel-Tag
                z.nurMP3();                         //Aktualisiert ListBox
            }
            catch(ArgumentException)
            {
                MessageBox.Show("Kein Ordner ausgewählt!");
            }

        }

      




        //WEnn Dateien schon so heißen -> Problem









        private void button1_Click_1(object sender, EventArgs e)       //Button Titel+Artist unbenennen
        {
            try
            { //Wenn kein Ordner ausgewählt MessageBox
                z = new CL_DateienAnzeigen(pfad);

            CL_DateienAnzeigen.txt1 = lbDateien;  //Übergibt ListBox an eine Variable der CL_DateienAnzeigen

            anzahlMP3imPfad = z.getanzahlMP3inPfad();   //Anzahl der MP3s im angegebenen Pfad zurückgegeben
            alleTitel = new string[anzahlMP3imPfad];

            alleTitel = z.TitelFinden();        //Gibt alle Titel-Tags zurück
            alleArtists = z.ArtistFinden();     //Gibt alle Artist-Tags zurück
            mischungTitelArtist = z.TitelArtistZusammensetzen(alleTitel, alleArtists, "TitelFirst"); 
            //Holt sich zusammengesetzter Array aus Titel+Artist

            z.NameToTitle(mischungTitelArtist);              //Ändert den Dateiname zum Titel-Tag
            z.nurMP3();                         //Aktualisiert ListBox
        }
            catch(ArgumentException)
            {
                MessageBox.Show("Kein Ordner ausgewählt!");
            }
}




        private void btnNameToTitle_Click(object sender, EventArgs e)       //Button in Titel unbenennen
        {
            try {    //Wenn kein Ordner ausgewählt MessageBox
            z = new CL_DateienAnzeigen(pfad);

            CL_DateienAnzeigen.txt1 = lbDateien;  //Übergibt ListBox an eine Variable der CL_DateienAnzeigen

            anzahlMP3imPfad = z.getanzahlMP3inPfad();   //Anzahl der MP3s im angegebenen Pfad zurückgegeben
            alleTitel = new string[anzahlMP3imPfad];  
            alleTitel = z.TitelFinden();        //Gibt alle Titel-Tags zurück
            z.NameToTitle(alleTitel);              //Ändert den Dateiname zum Titel-Tag
            z.nurMP3();                         //Aktualisiert ListBox

            }
            catch(ArgumentException)
            {
                MessageBox.Show("Kein Ordner ausgewählt!");
            }
}









        private void btnNurMP3Dateien_Click(object sender, EventArgs e)  //Nur MP3sanzeigen
        {
            /* Nur wenn extra Fenster geöffnet werden soll:

             suchePfad = new FolderBrowserDialog();   //Dialog, um Ordner auszuwählen
             suchePfad.ShowDialog();
             pfad = suchePfad.SelectedPath;          //Gibt den Pfad des gewählten Ordners zurück */

            if (tbPfad.Text != "")   //Wenn kein Pfad in Textbox -> Ordner nicht ausgewählt
            {
                y = new CL_DateienAnzeigen(pfad);

                CL_DateienAnzeigen.txt1 = lbDateien;  //Übergibt ListBox an eine Variable der CL_DateienAnzeigen
                y.nurMP3();

                tbPfad.Text = "Path: " + pfad;
            }
            else MessageBox.Show("Kein Ordner ausgewählt!");

        }

    

    }
}
