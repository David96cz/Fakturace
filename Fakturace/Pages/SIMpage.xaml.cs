using Fakturace.Data;
using Fakturace.Model;

namespace Fakturace;

public partial class SIMpage : ContentPage
{
    Random Nahoda;

    ContextDatabaze ContextDatabaze;

    string[] Jmena = { "Jan", "Petr", "Marie", "Jana", "Josef", "Eva", "Hana", "Tereza", "Miroslav", "Veronika", "Martin", "Lenka", "Jakub", "Lucie", "David", "Anna", "Pavel", "Kateøina", "Tomáš", "Petra", "Michal", "Elena", "Filip", "Markéta", "Lukáš", "Zuzana", "Adam", "Barbora", "Robert", "Eliška" };
    string[] Prijmeni = { "Novák", "Svoboda", "Novotnı", "Dvoøák", "Èernı", "Procházka", "Kuèera", "Veselı", "Horák", "Nìmec", "Marek", "Pospíšil", "Hájek", "Janda", "Král", "Vlèek", "Urban", "Richter", "Hájková", "Zeman", "Kratochvíl", "Šastnı", "Sıkora", "Nováková", "Hodnı", "Ondráèek", "Mach", "Beneš", "Pokornı", "Veselá" };
    string[] Zeme = { "Èeská republika"/*, "Slovensko", "Polsko", "Nìmecko", "Rakousko", "Francie", "Itálie"*/ };
    string[] Mesta = { "Praha", "Brno", "Ostrava", "Plzeò", "Liberec", "Olomouc", "Èeské Budìjovice", "Ústí nad Labem", "Pardubice", "Hradec Králové", "Zlín", "Karlovy Vary", "Jihlava", "Teplice", "Chomutov", "Kladno", "Mladá Boleslav", "Opava", "Frıdek-Místek", "Dìèín", "Kolín", "Karviná", "Tøebíè", "Prostìjov", "Cheb", "Pøerov", "Havíøov", "Èeská Lípa", "Kromìøí", "Jablonec nad Nisou", "Tøinec" };
    string[] Ulice = { "Dlouhá", "Krátká", "Námìstí", "Hlavní", "Školní", "Lesní", "Hradecká", "Vinohradská", "Jungmannova", "Korunní", "Staromìstská", "Rybáøská", "Ostravská", "Nádraní", "Námìstí Svobody", "Masarykova", "Praská", "Bezruèova", "Štefánikova", "Pionırská", "Komenského", "Palackého", "U Kamenného mostu", "V Celnici", "Podìbradská", "Sokolovská", "Benešova", "Husova", "Jana Masaryka", "Generála Svobody" };
    string[] CislaPopisne = { "123", "456", "789", "101", "202", "303", "404", "505", "606", "707", "808", "909", "111", "222", "333", "444", "555", "666", "777", "888", "999", "121", "232", "343", "454", "565", "676", "787", "898", "989" };
    string[] Psc = { "110 00", "120 00", "130 00", "140 00", "150 00", "160 00", "170 00", "180 00", "190 00", "200 00", "210 00", "220 00", "230 00", "240 00", "250 00", "260 00", "270 00", "280 00", "290 00", "300 00", "310 00", "320 00", "330 00", "340 00", "350 00", "360 00", "370 00", "380 00", "390 00", "400 00" };
    string[] Ica = { "12345678", "23456789", "34567890", "45678901", "56789012", "67890123", "78901234", "89012345", "90123456", "01234567", "98765432", "87654321", "76543210", "65432109", "54321098", "43210987", "32109876", "21098765", "10987654", "09876543", "98765432", "87654321", "76543210", "65432109", "54321098", "43210987", "32109876", "21098765", "10987654", "09876543" };
    string[] Produkty = new string[]
{
    "1§iPhone 15 128GB Black§22990§3§68970", "2§iPhone 13 128GB Blue§14990§3§44970", "3§Samsung Galaxy S22 Ultra§18990§5§94950", "4§Google Pixel 6 Pro§8490§4§33960", "5§OnePlus 10 Pro 256GB§15990§2§31980",
    "6§Xiaomi Mi 12 Pro§12990§6§77940", "7§Sony Xperia 5 III§17990§3§53970", "8§Huawei P50 Pro§20990§4§83960", "9§LG Velvet 2§9990§5§49950", "10§Motorola Edge 30 Pro§13990§3§41970",
    "11§Nokia X100§6990§6§41940", "12§Asus ROG Phone 6§24990§2§49980", "13§BlackBerry Key3§11990§4§47960", "14§Realme GT Neo 3§12990§3§38970", "15§Oppo Find X5 Pro§16990§5§84950",
    "16§Lenovo Legion Phone 3§18990§3§56970", "17§Vivo X80 Pro§15990§4§63960", "18§ZTE Axon 40 Pro§10990§5§54950", "19§TCL 30 Pro§7990§6§47940", "20§Alcatel 5X§5990§4§23960",
    "21§Meizu 19X§8990§3§26970", "22§Poco F4§15990§5§79950", "23§Honor Magic 4§17990§3§53970", "24§Infinix Zero 10§11990§4§47960", "25§Redmi K50 Pro§14990§2§29980",
    "26§Sharp Aquos R6§20990§3§62970", "27§Tecno Camon 18§9990§5§49950", "28§Wiko Power U30§7990§4§31960", "29§Cubot X20 Pro§8990§6§53940", "30§Ulefone Armor 15§10990§3§32970",
    "31§Sony Xperia 10 IV§17990§3§53970", "32§Xiaomi Redmi Note 11 Pro§13990§4§55960", "33§Samsung Galaxy A53§16990§5§84950", "34§iPhone SE 2022§18990§2§37980",
    "35§OnePlus Nord 3§24990§3§74970", "36§Google Pixel 7§19990§4§79960", "37§Realme GT 2§12990§5§64950", "38§Oppo Reno 7§14990§3§44970", "39§Lenovo K14§9990§4§39960",
    "40§Vivo Y45§8990§5§44950", "41§Xiaomi Mi 11T§18990§2§37980", "42§Huawei Mate 50§20990§3§62970", "43§LG G9§15990§4§63960", "44§Motorola Moto G8§10990§5§54950",
    "45§Nokia C30§7990§3§23970", "46§Asus Zenfone 8§24990§2§49980", "47§BlackBerry Key4§11990§3§35970", "48§Realme Narzo 50§12990§4§51960", "49§Oppo A76§16990§5§84950",
    "50§OnePlus 9RT§19990§2§39980", "51§Samsung Galaxy M52§15990§3§47970", "52§iPhone 14§23990§4§95960", "53§Xiaomi Redmi Note 11T§13990§4§55960", "54§Samsung Galaxy A33§13990§5§69950",
    "55§OnePlus 8T§21990§3§65970", "56§Google Pixel 6A§29990§2§59980", "57§Realme GT Master Edition§15990§4§63960", "58§Oppo A55§13990§5§69950", "59§Nokia X50§19990§3§59970",
    "60§Motorola Moto E8§7990§6§47940", "61§Sony Xperia 10 V§16990§3§50970", "62§Huawei Nova 9§22990§4§91960", "63§Xiaomi Redmi 10§11990§5§59950", "64§Samsung Galaxy M32§15990§4§63960",
    "65§iPhone 13 Mini§19990§3§59970", "66§OnePlus Nord 2§23990§4§95960", "67§Google Pixel 5A§29990§2§59980", "68§Realme GT Neo 2§16990§3§50970", "69§Oppo Reno 6§18990§4§75960",
    "70§Nokia G50§9990§5§49950", "71§Motorola Moto G7§11990§6§71940", "72§Sony Xperia 1 IV§27990§3§83970", "73§Huawei P40§24990§4§99960", "74§Xiaomi Mi 11 Ultra§29990§3§89970",
    "75§Samsung Galaxy S21§31990§4§127960", "76§iPhone 12 Pro§40990§2§81980", "77§OnePlus 9 Pro§35990§3§107970", "78§Google Pixel 4§29990§4§119960", "79§Realme X3 SuperZoom§15990§5§79950",
    "80§Oppo Reno 5 Pro§24990§3§74970", "81§Nokia 8.3§19990§4§79960", "82§Motorola Edge Plus§39990§3§119970", "83§Sony Xperia 5 II§31990§2§63980", "84§Huawei Mate 40 Pro§35990§3§107970",
    "85§Xiaomi Redmi Note 10 Pro§12990§4§51960", "86§Samsung Galaxy A71§22990§5§114950", "87§iPhone SE 2020§14990§6§89940", "88§OnePlus 8 Pro§36990§3§110970", "89§Google Pixel 3A§24990§4§99960",
    "90§Realme X50 Pro§27990§5§139950", "91§Oppo Find X2 Pro§42990§3§128970", "92§Nokia 7.2§11990§4§47960", "93§Motorola Moto G Power§13990§5§69950", "94§Sony Xperia 10 Plus§22990§6§137940",
    "95§Huawei P30 Pro§26990§3§80970", "96§Xiaomi Mi 10§25990§4§103960", "97§Samsung Galaxy Note 10§29990§5§149950", "98§iPhone 11 Pro§39990§3§119970", "99§OnePlus 7T§24990§4§99960",
    "100§Google Pixel 4A§34990§5§174950"
};



    int Rozsah = 30;
    int RozsahProduktu = 100;

    public SIMpage(ContextDatabaze contextDatabaze)
	{
		InitializeComponent();
        ContextDatabaze = contextDatabaze;

    }

    private void Button_Generovat_Dodavatele(object sender, EventArgs e)
    {
        if (!ContextDatabaze.Dodavatele.Any())
        {
            Nahoda = new Random();
            int id2 = 1;

            for (int i = 0; i < dodavateleStepper.Value; i++)
            {
                string jmeno = Jmena[Nahoda.Next(0, Rozsah)]; string prijmeni = Prijmeni[Nahoda.Next(0, Rozsah)]; string zeme = Zeme[0/*Nahoda.Next(0, Rozsah)*/]; string mesto = Mesta[Nahoda.Next(0, Rozsah)]; string ulice = Ulice[Nahoda.Next(0, Rozsah)]; string cisloPopisne = CislaPopisne[Nahoda.Next(0, Rozsah)]; string psc = Psc[Nahoda.Next(0, Rozsah)]; string ico = Ica[Nahoda.Next(0, Rozsah)]; string produkty = "";
                for (int j = 0; j < Nahoda.Next(0, Rozsah); j++)
                {
                    if (!string.IsNullOrEmpty(produkty)) // Poøešení, aby se poslední èlen pøedposledního záznamu a první èlen posledního záznamu nespojily do jednoho...
                    {
                        produkty += "§";
                    }
                    produkty += Produkty[Nahoda.Next(0, RozsahProduktu)];
                }
                ContextDatabaze.Dodavatele.Add(new Model.Dodavatel(id2, jmeno, prijmeni, zeme, mesto, ulice, cisloPopisne, psc, ico, produkty) { Jmeno = jmeno, Prijmeni = prijmeni, Zeme = zeme, Mesto = mesto, Ulice = ulice, CisloPopisne = cisloPopisne, Psc = psc, Ico = ico, Produkty = produkty });
                ContextDatabaze.SaveChanges();
                id2++;
            }

            foreach (Dodavatel d in ContextDatabaze.Dodavatele.ToList())
            {
                bool odberatel = false;
                ContextDatabaze.PrijateFaktury.Add(new Model.Faktura(d.Jmeno, d.Prijmeni, d.Zeme, d.Mesto, d.Ulice, d.CisloPopisne, d.Psc, d.Ico, d.Produkty, odberatel) { Jmeno = d.Jmeno, Prijmeni = d.Prijmeni, Zeme = d.Zeme, Mesto = d.Mesto, Ulice = d.Ulice, CisloPopisne = d.CisloPopisne, Psc = d.Psc, Ico = d.Ico, Produkty = d.Produkty, Odberatel = odberatel });
                ContextDatabaze.SaveChanges();
            }

            if (dodavateleStepper.Value == 1)
            {
                DisplayAlert("Info", $"Úspìšnì byl vygenerován {dodavateleStepper.Value} dodavatel.", "OK");
            }
            else if (dodavateleStepper.Value > 1 && dodavateleStepper.Value < 5)
            {
                DisplayAlert("Info", $"Úspìšnì byli vygenerováni {dodavateleStepper.Value} dodavatelé.", "OK");
            }
            else
            {
                DisplayAlert("Info", $"Úspìšnì bylo vygenerováno {dodavateleStepper.Value} dodavatelù.", "OK");
            }

        }
        else
        {
            DisplayAlert("Chyba", "V databázi jsou existující dodavatelé. Pøidejte je ruènì nebo nejprve vymate nynìjší obsah databáze dodavatelù a dejte generovat znovu.", "OK");
        }
    }

    private void Button_Generovat_Odberatele(object sender, EventArgs e)
    {
        if (!ContextDatabaze.Odberatele.Any())
        {
            Nahoda = new Random();
            int id2 = 1;

            for (int i = 0; i < odberateleStepper.Value; i++)
            {
                string jmeno = Jmena[Nahoda.Next(0,Rozsah)]; string prijmeni = Prijmeni[Nahoda.Next(0, Rozsah)]; string zeme = Zeme[0/*Nahoda.Next(0, Rozsah)*/]; string mesto = Mesta[Nahoda.Next(0, Rozsah)]; string ulice = Ulice[Nahoda.Next(0, Rozsah)]; string cisloPopisne = CislaPopisne[Nahoda.Next(0, Rozsah)]; string psc = Psc[Nahoda.Next(0, Rozsah)]; string ico = Ica[Nahoda.Next(0, Rozsah)]; string produkty = "";
                for (int j = 0; j < Nahoda.Next(0, Rozsah); j++)
                {
                    if (!string.IsNullOrEmpty(produkty))
                    {
                        produkty += "§";
                    }
                    produkty += Produkty[Nahoda.Next(0, RozsahProduktu)];
                }
                ContextDatabaze.Odberatele.Add(new Model.Odberatel(id2, jmeno, prijmeni, zeme, mesto, ulice, cisloPopisne, psc, ico, produkty) { Jmeno = jmeno, Prijmeni = prijmeni, Zeme = zeme, Mesto = mesto, Ulice = ulice, CisloPopisne = cisloPopisne, Psc = psc, Ico = ico, Produkty = produkty });
                ContextDatabaze.SaveChanges();
                id2++;
            }
            if (odberateleStepper.Value == 1)
            {
                DisplayAlert("Info", $"Úspìšnì byl vygenerován {odberateleStepper.Value} odbìratel.", "OK");
            }
            else if (odberateleStepper.Value > 1 && odberateleStepper.Value < 5)
            {
                DisplayAlert("Info", $"Úspìšnì byli vygenerováni {odberateleStepper.Value} odbìratelé.", "OK");
            }
            else
            {
                DisplayAlert("Info", $"Úspìšnì bylo vygenerováno {odberateleStepper.Value} odbìratelù.", "OK");
            }
        }
        else
        {
            DisplayAlert("Chyba", "V databázi jsou existující odbìratelé. Pøidejte je ruènì nebo nejprve vymate nynìjší obsah databáze obìratelù a dejte generovat znovu.", "OK");
        }
    }

    private async void Button_Reset(object sender, EventArgs e)
    {
        var rozhodnuti = await DisplayAlert("Pozor!", "Opravdu chcete resetovat databázi?", "Ano", "Zrušit");

        if (rozhodnuti)
        {
            var dodavatele = ContextDatabaze.Dodavatele.ToList();
            ContextDatabaze.Dodavatele.RemoveRange(dodavatele);
            ContextDatabaze.SaveChanges();

            var prijate = ContextDatabaze.PrijateFaktury.ToList();
            ContextDatabaze.PrijateFaktury.RemoveRange(prijate);
            ContextDatabaze.SaveChanges();

            var odberatele = ContextDatabaze.Odberatele.ToList();
            ContextDatabaze.Odberatele.RemoveRange(odberatele);
            ContextDatabaze.SaveChanges();

            var vystavene = ContextDatabaze.VystaveneFaktury.ToList();
            ContextDatabaze.VystaveneFaktury.RemoveRange(vystavene);
            ContextDatabaze.SaveChanges();

            await DisplayAlert("Info", "Databáze byla resetována.", "OK");
        }  
    }
}