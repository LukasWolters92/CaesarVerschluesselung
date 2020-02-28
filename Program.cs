using System;

namespace Cesar_Verschlüsselung
{
	class Program
	{
		public static void Main(string[] args)
		{
			string ergebnis;
			string fall;
			bool programmrunning = true;

			while (programmrunning)
			{
				//Menü zur Auswahl der Funktionen des Programmes
				Console.WriteLine("Willst du 1.Verschlüsseln, 2.Entschlüsseln oder 3.Knacken? Bitte gib 1, 2 oder 3 ein");
				Console.WriteLine("Andere Eingaben beenden das Programm");
				fall = Console.ReadLine();

				//Aufruf der Methoden der jeweiligen Funktion und Ergebnisausgabe
				switch (fall)
				{
					case "1":
						ergebnis = EnDecode(fall);
						Console.WriteLine("Der Verschlüsselte Text lautet:");
						Console.WriteLine(ergebnis);
						break;
					case "2":
						ergebnis = EnDecode(fall);
						Console.WriteLine("Der Klartext lautet:");
						Console.WriteLine(ergebnis);
						break;
					case "3":
						ergebnis = Crack();
						Console.WriteLine("Der Klartext lautet:");
						Console.WriteLine(ergebnis);
						break;
					default:
						Console.WriteLine("Programm wird beendet");
						programmrunning = false;
						break;
				}
			}
			//Ende des Programms
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}

		//			Methoden			//


		//Methode zum Verschlüsseln und Entschlüsseln eines Textes

		static string EnDecode(string fall)
		{
			string text;
			string encodedtext;
			string direction;
			int steps;

			string texteingabeauforderung;
			string richtungseingabeauforderung;
			
			//Der Text der dem User angezeigt wird, wird abhänig der gewünschten Funktion des Programmes festgelegt.
			texteingabeauforderung =(fall == "1") ? "Geben Sie den zu verschlüsselnden Text ein:"
									: (fall == "2") ? "Geben Sie den zu entschlüsselnden Text ein:"
									: "Fehler bei der Falleingabe";
			richtungseingabeauforderung = (fall == "1") ? "Soll nach rechts oder links verschlüsselt werden?"
									: (fall == "2") ? "Soll nach rechts oder links entschlüsselt werden?"
									: "Fehler bei der Falleingabe";

			//Eingabeauforderungen für Text, Schritte und Richtung der Verschlüsselung/Entschlüsselung
			Console.WriteLine(texteingabeauforderung);
			text = Console.ReadLine();

			Console.WriteLine("Wieviele Schritte soll gedreht werden?");
			steps = Convert.ToInt32(Console.ReadLine());

			//Stellt sicher dass eine Eingabe erfolgt, die die Methode MoveText() versteht
			do
			{
				Console.WriteLine(richtungseingabeauforderung);
				direction = Console.ReadLine();


			} while (direction != "rechts" && direction != "links");

			//Die Ver-/Entschlüsselung findet in der MoveText() Mehtode statt
			encodedtext = MoveText(text, direction, steps);

			return encodedtext;

		}

		//Methode zum Knacken eines verschlüsselten Textes

		static string Crack()
		{
			string encodedtext;
			string crackedtext = "";
			// Einige der häufigsten Wörter der Deutschen Sprache
			string[] meistbenutzenwoerter = {"die", "der", "und", "in", "zu", "den", "das", "nicht", "von", "sie",
											"ist", "des", "sich", "mit", "dem", "dass", "er", "es", "ein", "ich",
											"auf", "so", "eine", "auch", "als", "nach", "wie", "für", 
											"man", "aber", "aus", "durch", "wenn", "nur", "war", "noch", "werden", "bei",
											"wir", "was", "wird", "sein", "einen", "welche", "sind", "oder", "zur",
											"haben", "einer", "mir", "über", "ihm", "diese", "einem", "ihr", "uns"};
			// Zeichen an denen der Text getrennt werden sollen um ihn in einzelne Worte aufzuteilen
			char[] separator = { ',', ' ', '.' };
			string abfrage = "";

			Console.WriteLine("Geben Sie den zu knackenden Text ein:");
			encodedtext = Console.ReadLine();

			// Konvertiert den Text komplett in klein Buchstaben um den Vergleich der Wörter zu vereinfachen.
			crackedtext = encodedtext.ToLower();

			//Jede Möglichkeit wird mindestens einmal Probiert
			for (int i = 0; i < 29; i++)
			{
				crackedtext = MoveText(crackedtext, "rechts", 1);

				// Liste der verschlüsselten Worte
				string[] woerterliste = crackedtext.Split(separator, 100, StringSplitOptions.None);

				//Die verschlüsselten Worte werden jeweils mit den häufigen Worten verglichen
				foreach (string wort in woerterliste)
				{
					foreach(string haeufigeswort in meistbenutzenwoerter)
					{
						//Falls eine Übereinstimmung gefunden wurde, wird der User gefragt ob das Ergebnis richtig sein kann.
						if (wort == haeufigeswort)
						{
							Console.WriteLine("Könnte dies der richtige Text sein? Y/N");
							Console.WriteLine(crackedtext);
							abfrage = Console.ReadLine();
							if (abfrage == "Y") return crackedtext;
						}
					}
				}

			}

			Console.WriteLine("nichts gefunden");
			return crackedtext;
		}

		//Methode zum erstellen des Alphabet Arrays

		static char[] Alphabet()
		{
			char[] umlaute = { 'Ä', 'ä', 'Ö', 'ö', 'Ü', 'ü' };
			char[] alphabet = new char[58];
			for (int i = 0; i < 26; i++)
			{
				alphabet[i * 2] = (char)(i + 65);
				alphabet[i * 2 + 1] = (char)(i + 97);
			}
			for (int j = 0; j < 6; j++)
			{
				alphabet[j + 52] = umlaute[j];
			}
			return alphabet;
		}

		//Methode bewegt einen Buchstaben um einen Schritt

		static char MoveLetter(char letter, string direction)
		{
			char[] alphabet = Alphabet();
			char movedletter = ' ';
			int index;

			// findet den Index des übergebenen Buchstaben "letter" in dem alphabet Array
			index = Array.IndexOf(alphabet, letter);

			//falls kein Index gefunden wurde wird das Zeichen zurück gegeben (zB bei ? ! . , o.ä.)
			if (index == -1)
			{
				return letter;
			}
			//sonst wird der wird der neue Buchstabe, der zurück gegeben wird, der Buchstabe der zwei Indexe weiter im alphabet Array steht.
			//Zwei, weil das Array Groß und Kleinschreibung beachtet.
			//Die Fälle dass der Buchstabe an einer der Enden des Array steht wird auch beachtet.
			else
			{
				if(direction == "rechts")
				{
					movedletter = (index == 56) ? alphabet[0] : (index == 57) ? alphabet[1] : alphabet[index + 2];
				}
				else if(direction == "links")
				{
					movedletter = (index == 0) ? alphabet[56] : (index == 1) ? alphabet[57] : alphabet[index - 2];
				}
			}
			return movedletter;
		}

		//Methode bewegt den ganzen Text um 'steps' Schritte

		static string MoveText(string text, string direction, int steps)
		{
			string finishedtext = "";
			char newletter;

			//Iteriert durch jeden Buchstaben im gegebenen Text.

			foreach (char letter in text)
			{
				newletter = letter;

				//Bewegt jeden Buchstaben mit der MoveLetter Methode um die gewünschten Schritte
				for(int i = 0; i<steps; i++ )
				{
					newletter = MoveLetter(newletter, direction);
				}

				//Fügt den neuen Text Bichstabe für Buchstabe zusammen.
				finishedtext += newletter;
			}

			return finishedtext;
		}
	}
}