using Id3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using MetroFramework.Forms;
namespace Musik_Projekt
{
    public class CL_DateienAnzeigen
    {
        int anzahlMP3inPfad;
        string pfad;
        DirectoryInfo location;   //Pfad
        public static ListBox txt1 = new ListBox();  //Globale ListBox um ErsteForm() zu verwenden.
                                                     //Aufgerufen in Form1.cs

        string[] musicFiles;
        int i;
        string[] titel, artist, zusammengesetzt;
        int nummertitel = 0;
        //char[] invalidFileChars; unten deklariert



        public CL_DateienAnzeigen(string pfad)        //Konstruktor
        {
            
            location = new DirectoryInfo(pfad);
            this.pfad = string.Concat(pfad, "\\");          //BEinhaltet Pfad mit Backslash am Schluss
            anzahlMP3inPfad = location.GetFiles("*.mp3").Length;
        }








        public void inListBoxSchreiben()
        {
            foreach (FileInfo info in location.GetFiles())  //Im jeweiligen Pfad alle Dateien getten
            {
                txt1.Items.Add(info.Name);  //Fügt der Listbox der Name ("Name") der Dateien hinzu.
            }
        }








        public void nurMP3()         //Zeigt nur MP3-Files an
        {
            txt1.Items.Clear();  //Bisherige Dateienauswahl löschen (Dateien aus "Ordner wählen Button")
            foreach (FileInfo info in location.GetFiles("*.mp3"))  //Alle MP3 Dateien getten
            {
                
                txt1.Items.Add(info.Name);  //Fügt der Listbox der Name ("Name") der Dateien hinzu.
            }

        }






        public int getanzahlMP3inPfad()         //Gibt die Anzahl an MP3Files m Pfad zurück
        {
            return anzahlMP3inPfad;   //siehe Konstruktor
        }








        public string[] TitelFinden()           //Titel-Tag in Dateien werden zurückgegeben
        {
            i = 0;
            musicFiles = Directory.GetFiles(pfad, "*.mp3");    //Alle Dateien werden im String[] gespeichert
            titel = new string[musicFiles.Length];           //Array "Titel" mit der Länge alle Files

            foreach (string musicFile in musicFiles)
            {
                using (var mp3 = new Mp3File(musicFile))
                {
                    //Verweise->Verweis hinzufuegen-> die drei .dll Dateien aus der ID3-Library wählen
                    //ID3-Tag Library: https://id3.codeplex.com/
                    //Implementierung: https://www.youtube.com/watch?v=C305CxPCBKY (selbes Prinzip)


                    Id3Tag tag = mp3.GetTag(Id3TagFamily.FileStartTag);   //Tags herauslesen
                    titel[i] = tag.Title.Value;  //Titel-Tag im Array speichern
                                                 // MessageBox.Show(tag.Title.Value);

                }
                i++;
            }

            return titel;    //Gibt den Array mit den Titel-Tags zurück
        }





        public string[] ArtistFinden()           //Titel-Tag in Dateien werden zurückgegeben
        {
            i = 0;
            musicFiles = Directory.GetFiles(pfad, "*.mp3");    //Alle Dateien werden im String[] gespeichert
            artist = new string[musicFiles.Length];           //Array "Titel" mit der Länge alle Files

            foreach (string musicFile in musicFiles)
            {
                using (var mp3 = new Mp3File(musicFile))
                {
                    //Verweise->Verweis hinzufuegen-> die drei .dll Dateien aus der ID3-Library wählen
                    //Library: https://id3.codeplex.com/
                    //Implementierung: https://www.youtube.com/watch?v=C305CxPCBKY (selbes Prinzip)


                    Id3Tag tag = mp3.GetTag(Id3TagFamily.FileStartTag);   //Tags herauslesen
                    artist[i] = tag.Artists.Value;  //Titel-Tag im Array speichern
                                                 // MessageBox.Show(tag.Title.Value);

                }
                i++;
            }

            return artist;    //Gibt den Array mit den Titel-Tags zurück
        }









        public string[] TitelArtistZusammensetzen(string[] titel, string[] artist, string wasZuerst)
        {

            //Alle Titel und Artists werden in einem Array übergeben, als String wird übergeben welcher Button gedrückt wurde
            zusammengesetzt = new string[titel.Length];

            if (wasZuerst == "TitelFirst")      //Wenn "TitelFirst" übergeben -> system merkt, dass Titel+Artist Button
            {
                for (int m = 0; m < titel.Length; m++)
                {
                    zusammengesetzt[m] = titel[m] + "_" + artist[m];  //"Neuer" Array zusammengestetzt aus Tags
                }

            }
            else if (wasZuerst == "ArtistFirst")
            {
                //Wenn "ArtistFirst" übergeben -> system merkt, dass Artist+Titel Button
                if (wasZuerst == "ArtistFirst")
                {
                    for (int m = 0; m < titel.Length; m++)
                    {
                        zusammengesetzt[m] = artist[m] + "_" + titel[m];   //"Neuer" Array zusammengestetzt aus Tags
                    }
                }
                
            }
            return zusammengesetzt;
        }











        public void NameToTitle(string[] titles)    //AUCH bei Titel+Artist, dann wird zusammengesetzter Array uebergeben
        {                                           //dieser läuft dann unter "titles[]" weiter
            for (int j = 0; j < titles.Length; j++)
            {

                string path = musicFiles[j];            //Pfad der Musikdateien mit dem NAmen der Musikdatei + Format (mp3)
                string path2 = string.Concat(pfad, titles[j], ".mp3");      //Zielpfad: Gleicher Ordner, mit dem Titel-Tag als Name
                try
                {
                    if (!File.Exists(path))             //Exception Handling #1
                    {
                        // This statement ensures that the file is created,
                        // but the handle is not kept.
                        using (FileStream fs = File.Create(path)) { }
                    }

                    /*// Ensure that the target does not exist.   //Exception Handling #2
                    if (File.Exists(path2))
                        File.Delete(path2);
                        */                  //Nicht nötig, da er sonst die Files löscht die bereits so benannt sind

                    // Move the file.
                    try {
                        File.Move(path, path2);     //Tatsächliche NAmensänderung
                    }
                    catch (IOException) {       //Wenn neuer Name bereits vorhanden fügt Nummer hinzu
                        path2 = string.Concat(pfad, titles[j], nummertitel, ".mp3");
                        nummertitel = nummertitel + 1;
                    }
                    catch (ArgumentException)
                    {

                        //Zeichen die nicht in einem Namen vorkommen duerfen
                        char[] invalidFileChars = { '"', '*', ':', '<', '>', '?', '\\', '/', '|', '+', '[', ']'};
                        
                        for (int n = 0; n < invalidFileChars.Length; n++)
                        {
                            path2.Replace(invalidFileChars[n], ' ');    //Zeichen ersetzen druch Leerzeichen
                        }


                    }

                
                }
                catch (Exception e)         //Exception Handling #3 - Code via Microsoft
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
    }
}

