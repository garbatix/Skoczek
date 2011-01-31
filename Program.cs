using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skoczek
{
    public struct Pole  // zawiera informacje o danym polu szachownicy
    {
        public int nr, wybor;
        public Pole(int a, int b)
        {
            nr = a;     // kolejność w jakiej skoczek dociera do tego pola
            wybor = b;  // który prawidłowy ruch skoczka prowadzi do celu, domyślnie 1 czyli pierwszy, ale jeśli się cofamy wartość ta jest powiększana
        }
    }

    public struct Pozycja   // współrzędne; współrzędne określamy od górnego lewego rogu 0;0 do dolnego prawego BokSzachownicy-1;BokSzachownicy-1
    {
        public int x, y;
        public Pozycja(int a, int b)
        {
            x = a;
            y = b;
        }
    }

    class Program
    {        
        private const int BokSzachownicy = 8; // długość boku szachownicy - ilość pól
        public static Pole[,] Szachownica = new Pole[BokSzachownicy, BokSzachownicy]; // deklarowanie pustej tablicy szachownicy

        static void PokazSzachownice(Pole[,] Szachownica)   // TESTOWE procedura do pokazywania szachownicy całej z informacjami o każdym polu
        {
            for (int y = 0; y < BokSzachownicy; y++)
            {
                Console.Write(BokSzachownicy-y + "  ");
                for (int x = 0; x < BokSzachownicy; x++)
                {
                    Console.Write(Szachownica[x, y].wybor);
                    Console.Write("  ");
                }
                Console.WriteLine(" ");
            }
            Console.Write("   A  B  C  D  E");  // więcej liter to na razie testowe
            Console.WriteLine(" ");
            // Console.ReadKey();
        }

        private static bool Sprawdz(Pozycja p)  // funkcja do sprawdzania, czy na danym polu konik już był i czy jest to pole wewnątrz szachownicy
        {
            if ((p.x >=0) && (p.x <= (BokSzachownicy - 1)) && (p.y >=0) && (p.y <= (BokSzachownicy - 1)) && (Szachownica[p.x,p.y].nr == 0)) return true;
            else 
               return false;

        }

        private static Pozycja PoprzedniRuch (Pozycja Poz)
        {
            Pozycja Poprz = new Pozycja();
            for (int x = 0; x < BokSzachownicy; x++)   // Przeszukujemy tablicę
                for (int y = 0; y < BokSzachownicy; y++)
                {
                    if (Szachownica[x, y].nr == (Szachownica[Poz.x, Poz.y].nr - 1))
                    {
                        Poprz.x = x;
                        Poprz.y = y;
                        break;
                    }
                }
            return Poprz;
        }

        private static Pozycja Skok (Pozycja polestartowe)  // funkcja wykonująca skok na kolejne pole - zwraca nowa pozycję konika, pobiera obecną pozycję konika
        {
            Pozycja wspolrzedne = new Pozycja();
            int BiezPoprawnych = 0;  // to się ma zwiększać przy sukcesie aż dojdzie do wartości MaxPoprawnych - wtedy bierzemy pozycję
            int MaxPoprawnych = (Szachownica[polestartowe.x, polestartowe.y].wybor);
            
            wspolrzedne.x = polestartowe.x + 1; // pierwszy możliwy skok
            wspolrzedne.y = polestartowe.y - 2;
            if ((Sprawdz(wspolrzedne)) && (++BiezPoprawnych == MaxPoprawnych)) return wspolrzedne;
            else
            {
                wspolrzedne.x = polestartowe.x + 2; // drugi możliwy skok
                wspolrzedne.y = polestartowe.y - 1;
                if ((Sprawdz(wspolrzedne)) && (++BiezPoprawnych == MaxPoprawnych)) return wspolrzedne;
                else
                {
                    wspolrzedne.x = polestartowe.x + 1; // trzeci możliwy skok
                    wspolrzedne.y = polestartowe.y + 2;
                    if ((Sprawdz(wspolrzedne)) && (++BiezPoprawnych == MaxPoprawnych)) return wspolrzedne;
                    else
                    {
                        wspolrzedne.x = polestartowe.x + 2; // czwarty możliwy skok
                        wspolrzedne.y = polestartowe.y + 1;
                        if ((Sprawdz(wspolrzedne)) && (++BiezPoprawnych == MaxPoprawnych)) return wspolrzedne;
                        else
                        {
                            wspolrzedne.x = polestartowe.x - 1; // piąty możliwy skok
                            wspolrzedne.y = polestartowe.y + 2;
                            if ((Sprawdz(wspolrzedne)) && (++BiezPoprawnych == MaxPoprawnych)) return wspolrzedne;
                            else
                            {
                                wspolrzedne.x = polestartowe.x - 2; // szósty możliwy skok
                                wspolrzedne.y = polestartowe.y + 1;
                                if ((Sprawdz(wspolrzedne)) && (++BiezPoprawnych == MaxPoprawnych)) return wspolrzedne;
                                else
                                {
                                    wspolrzedne.x = polestartowe.x - 1; // siódmy możliwy skok
                                    wspolrzedne.y = polestartowe.y - 2;
                                    if ((Sprawdz(wspolrzedne)) && (++BiezPoprawnych == MaxPoprawnych)) return wspolrzedne;
                                    else
                                    {
                                        wspolrzedne.x = polestartowe.x - 2; // ósmy możliwy skok
                                        wspolrzedne.y = polestartowe.y - 1;
                                        if ((Sprawdz(wspolrzedne)) && (++BiezPoprawnych == MaxPoprawnych)) return wspolrzedne;
                                        else
                                        {
                                            wspolrzedne = polestartowe;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return wspolrzedne;
        }

        static void Main(string[] args)
        {
            int MaksSkokow = BokSzachownicy * BokSzachownicy;
            
            for (int x=0; x < BokSzachownicy; x++)   // Wypełniamy tablicę szachownicy
                for (int y = 0; y < BokSzachownicy; y++)
                {
                    Szachownica[x, y].nr = 0;
                    Szachownica[x, y].wybor = 1;
                }
            Szachownica[0, BokSzachownicy-1].nr = 1; // Określamy pole startowe, czyli pole A1 (pole o współrzędnych 0;DługośćBoku)
            Pozycja AktualnaPoz, NowaPoz = new Pozycja();
            AktualnaPoz.x = 0;
            AktualnaPoz.y = BokSzachownicy-1; // bo współrzędne określamy od 0 a nie od 1, a BokSzachownicy to ilość pól
            NowaPoz = AktualnaPoz;
            int max = 0;

            do
            {
                do
                {
                    AktualnaPoz = NowaPoz;
                    NowaPoz = Skok(AktualnaPoz); // skok
                    if ((NowaPoz.x != AktualnaPoz.x) && (NowaPoz.y != AktualnaPoz.y))
                    {
                        Szachownica[NowaPoz.x, NowaPoz.y].nr = Szachownica[AktualnaPoz.x, AktualnaPoz.y].nr + 1;
                        // Console.Write("Skok " + (Szachownica[NowaPoz.x, NowaPoz.y].nr) + ": " + NowaPoz.x + ", " + NowaPoz.y + "; "); // TESTOWE pozycja po skoku
                    }
                }
                while ((NowaPoz.x != AktualnaPoz.x) && (NowaPoz.y != AktualnaPoz.y));
                
                if (Szachownica[AktualnaPoz.x, AktualnaPoz.y].nr != MaksSkokow) // jeśli nie doszliśmy do końca szachownicy
                {                    
                    // cofnij aktualną pozycję o jeden do tyłu
                    NowaPoz = PoprzedniRuch(NowaPoz);
                    if (Szachownica[AktualnaPoz.x, AktualnaPoz.y].nr == 63) { max++; Console.WriteLine("Próba " + max); }
                    Szachownica[AktualnaPoz.x, AktualnaPoz.y].nr = 0;
                    Szachownica[AktualnaPoz.x, AktualnaPoz.y].wybor = 1;
                    // Console.WriteLine("Cofam się na " + NowaPoz.x + " : " + NowaPoz.y);
                    // Console.ReadKey();
                    // zaznacz, że trzeba brać inny wybór
                    
                    if (Szachownica[NowaPoz.x, NowaPoz.y].wybor < 8) Szachownica[NowaPoz.x, NowaPoz.y].wybor ++;
                    
                }
                
            }
            while (Szachownica[AktualnaPoz.x, AktualnaPoz.y].nr != MaksSkokow);
            Console.WriteLine();
            Console.WriteLine("SUKCES!");
            PokazSzachownice(Szachownica);    // TESTOWE pokazuje całą szachownicę z kolejnymi ruchami
            Console.ReadKey();


        }
    }
}
