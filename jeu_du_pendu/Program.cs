using System;
using AsciiArt;

namespace jeu_du_pendu
{
    internal class Program
    {
        static void AfficherMot(string mot, List<char> lettres)
        {
            for(int i = 0; i < mot.Length; i++)
            {
                char lettre = mot[i];
                if (lettres.Contains(lettre))
                {
                    Console.Write(lettre+" ");
                }
                else
                {
                    Console.Write("_ ");
                }
            }
            Console.WriteLine();
        }

        static char DemanderUneLettre(string mess = "Rentrez une lettre : ")
        {
            string reponse = "";

            while (reponse.Length != 1)
            {
                Console.Write(mess);
                reponse = Console.ReadLine();

                if (reponse.Length != 1)
                {
                    Console.WriteLine("ERREUR : vous ne pouvez rentrer que une lettre !");
                }
            }
            return reponse.ToUpper()[0];
        }

        static void DevinerMot(string mot)
        {
            List<char> lettres = new List<char>();
            List<char> lettresExclues = new List<char>();
            const int NB_VIES = 6;
            int nbViesRestantes = NB_VIES;
            char lettre;

            

            while (nbViesRestantes > 0)
            {
                Console.WriteLine(Ascii.PENDU[NB_VIES-nbViesRestantes]);
                Console.WriteLine();

                Console.WriteLine("Nombre de vies restantes = " + nbViesRestantes);
                AfficherMot(mot, lettres);
                Console.WriteLine();
                lettre = DemanderUneLettre();
                Console.Clear();
                if (mot.Contains(lettre))
                {
                    Console.WriteLine("La lettre " + lettre + " est bien dans le mot");
                    lettres.Add(lettre);
                    if (ToutesLettresTrouvees(mot, lettres))
                    {
                        break;
                    }
                }
                else
                {
                    if (!lettresExclues.Contains(lettre))
                    {
                        lettresExclues.Add(lettre);
                        nbViesRestantes--;
                    }
                    
                }

                if(lettresExclues.Count > 0)
                {
                    Console.WriteLine("Le mot ne contient pas les lettres : " + String.Join(" / ", lettresExclues));
                }
                Console.WriteLine();
            }

            Console.WriteLine(Ascii.PENDU[NB_VIES-nbViesRestantes]);

            if (nbViesRestantes == 0)
            {
                AfficherMot(mot, lettres);
                Console.WriteLine("Vous avez perdu ! le mot était " + mot);
            }
            else
            {
                AfficherMot(mot, lettres);
                Console.WriteLine("Vous avez gagné ! le mot est " + mot);
            }
            

        }

        static bool ToutesLettresTrouvees(string mot, List<char> lettres)
        {
            foreach(char lettre in lettres)
            {
                mot = mot.Replace(lettre.ToString(),"");
            }

            if(mot.Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static string[] ChargerlesMots(string nomFic)
        {
            try
            {
                return File.ReadAllLines(nomFic);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Erreur lors de la lecture du fichier " + nomFic + "\n " + ex.Message);
            }
            return null;
        }

        static bool DemanderDeRejouer()
        {
            char res = DemanderUneLettre("Voulez-vous rejouer ? (o/n) : ");
            if (res.ToString().ToUpper() == "N")
            {
                return false;
            }
            else if (res.ToString().ToUpper() == "O")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Vous devez rentrer o ou n !");
                return DemanderDeRejouer();
            }
            
        }

        static void Main(string[] args)
        {
            var mots = ChargerlesMots("mots.txt");
            bool jouer = true;


            if((mots == null) || (mots.Length == 0))
            {
                Console.WriteLine("Erreur la liste des mots est vide");
            }
            else
            {
                while (jouer)
                {
                    Random rand = new Random();
                    string mot = mots[rand.Next(mots.Length)].Trim().ToUpper();
                    DevinerMot(mot);

                    if (!DemanderDeRejouer())
                    {
                        break;
                    }
                    Console.Clear();

                }
                Console.WriteLine("Merci d'avoir joué! A bientôt :)");

            }
            
        }
    }
}