using Fakturace.Data;
using Fakturace.Model;
using static System.Net.Mime.MediaTypeNames;

namespace Fakturace;
	
public partial class NovyDodavatel : ContentPage
{
    ContextDatabaze ContextDatabaze;
    Dodavatel D;
    ListView DodavateleList;
    ListView PrijateList;
    Random Nahoda;
    int id2;
    string produkty;

    public NovyDodavatel(ContextDatabaze contextDatabaze, ListView dodavateleList, ListView prijateList)
	{
		InitializeComponent();
        ContextDatabaze = contextDatabaze;
        DodavateleList = dodavateleList;
        PrijateList = prijateList;
        Nahoda = new Random();
	}

    private void Button_Generovat(object sender, EventArgs e)
    {
        if ((jmenoInput.Text == null) || (prijmeniInput.Text == null) || (zemeInput.Text == null) || (mestoInput.Text == null) || (uliceInput.Text == null) || (cisloPopisneInput.Text == null) || (pscInput.Text == null) || (icoInput.Text == null))
        {
            DisplayAlert("Chyba", "Vyplòte všechny požadované údaje pro pøidání dodavatele.", "OK");
        }
        else if (produkty != null)
        {
            string jmeno = jmenoInput.Text;
            string prijmeni = prijmeniInput.Text;
            string zeme = zemeInput.Text;
            string mesto = mestoInput.Text;
            string ulice = uliceInput.Text;
            string cisloPopisne = cisloPopisneInput.Text;
            string psc = pscInput.Text;
            string ico = icoInput.Text;

            foreach (Dodavatel d in ContextDatabaze.Dodavatele.ToList())
            {
                D = d;
            }

            if (D != null)
            {
                id2 = D.Id2 + 1;
            }
            else
            {
                id2 = 1;
            }

            ContextDatabaze.Dodavatele.Add(new Model.Dodavatel(id2, jmeno, prijmeni, zeme, mesto, ulice, cisloPopisne, psc, ico, produkty) { Jmeno = jmeno, Prijmeni = prijmeni, Zeme = zeme, Mesto = mesto, Ulice = ulice, CisloPopisne = cisloPopisne, Psc = psc, Ico = ico, Produkty = produkty });
            ContextDatabaze.SaveChanges();

            DodavateleList.ItemsSource = null;
            DodavateleList.ItemsSource = ContextDatabaze.Dodavatele.ToList();

            foreach (Dodavatel d in ContextDatabaze.Dodavatele.ToList())
            {
                D = d;
            }

            bool odberatel = false;
            ContextDatabaze.PrijateFaktury.Add(new Model.Faktura(D.Jmeno, D.Prijmeni, D.Zeme, D.Mesto, D.Ulice, D.CisloPopisne, D.Psc, D.Ico, D.Produkty, odberatel) { Jmeno = D.Jmeno, Prijmeni = D.Prijmeni, Zeme = D.Zeme, Mesto = D.Mesto, Ulice = D.Ulice, CisloPopisne = D.CisloPopisne, Psc = D.Psc, Ico = D.Ico, Produkty = D.Produkty, Odberatel = odberatel });
            ContextDatabaze.SaveChanges();
            PrijateList.ItemsSource = null;
            PrijateList.ItemsSource = ContextDatabaze.PrijateFaktury.ToList();

            DisplayAlert("Info", "Dodavatel byl úspìšnì pøidán.", "OK");
        }
        else
        {
            DisplayAlert("Chyba", "Zadejte nìjaké produkty, které budete od dodavatele odebírat.", "OK");
        }
    }

    public async void Button_Pridat(object sender, EventArgs e)
    {
        if ((idInput.Text == null) || (nazevProduktuInput.Text == null) || (cenaInput.Text == null) || (pocetKsInput.Text == null))
        {
            await DisplayAlert("Chyba", "Zadejte platné parametry produktu.", "OK");
        }
        else
        {
            int id;
            string nazevProduktu = nazevProduktuInput.Text;
            int cena;
            int pocetKs;

            if (!int.TryParse(idInput.Text, out id))
            {
                await DisplayAlert("Chyba", "Zadejte platnou celoèíselnou hodnotu pro ID.", "OK");
                return;
            }

            if (!int.TryParse(cenaInput.Text, out cena))
            {
                await DisplayAlert("Chyba", "Zadejte platnou celoèíselnou hodnotu pro cenu.", "OK");
                return;
            }

            if (!int.TryParse(pocetKsInput.Text, out pocetKs))
            {
                await DisplayAlert("Chyba", "Zadejte platnou celoèíselnou hodnotu pro poèet kusù.", "OK");
                return;
            }

            int cenaZaKusy = cena * pocetKs;

            idInput.Text = nazevProduktuInput.Text = cenaInput.Text = pocetKsInput.Text = "";

            if (!string.IsNullOrEmpty(produkty)) //Poøešení, aby se poslední èlen pøedposledního záznamu a první èlen posledního záznamu nespojily do jednoho...
            {
                produkty += "§";
            }
            produkty += $"{id}§{nazevProduktu}§{cena}§{pocetKs}§{cenaZaKusy}";

            idecka.Text += $"{id}\n";
            nazvyProduktu.Text += $"{nazevProduktu}\n";
            ceny.Text += $"{cena}Kè\n";
            poctyKusu.Text += $"{pocetKs}ks\n";
            cenyZaKusy.Text += $"{cenaZaKusy}Kè\n";
        }
    }
}