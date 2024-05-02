using Fakturace.Data;
using Fakturace.Model;

namespace Fakturace;

public partial class NovyOdberatel : ContentPage
{
    ContextDatabaze ContextDatabaze;
    Odberatel O;
    ListView OdberateleList;
    int id2;
    string produkty;

    public NovyOdberatel(ContextDatabaze contextDatabaze, ListView odberateleList, ListView vystaveneList)
    {
        InitializeComponent();
        ContextDatabaze = contextDatabaze;
        OdberateleList = odberateleList;
    }

    private void Button_Generovat(object sender, EventArgs e)
    {
        if ((jmenoInput.Text == null) || (prijmeniInput.Text == null) || (zemeInput.Text == null) || (mestoInput.Text == null) || (uliceInput.Text == null) || (cisloPopisneInput.Text == null) || (pscInput.Text == null) || (icoInput.Text == null))
        {
            DisplayAlert("Chyba", "Vyplòte všechny požadované údaje pro pøidání odbìratele.", "OK");
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

            foreach (Odberatel o in ContextDatabaze.Odberatele.ToList())
            {
                O = o;
            }

            if (O != null)
            {
                id2 = O.Id2 + 1;
            }
            else
            {
                id2 = 1;
            }

            ContextDatabaze.Odberatele.Add(new Model.Odberatel(id2, jmeno, prijmeni, zeme, mesto, ulice, cisloPopisne, psc, ico, produkty) { Jmeno = jmeno, Prijmeni = prijmeni, Zeme = zeme, Mesto = mesto, Ulice = ulice, CisloPopisne = cisloPopisne, Psc = psc, Ico = ico, Produkty = produkty });
            ContextDatabaze.SaveChanges();

            OdberateleList.ItemsSource = null;
            OdberateleList.ItemsSource = ContextDatabaze.Odberatele.ToList();

            DisplayAlert("Info", "Odbìratel byl úspìšnì pøidán.", "OK");
        }
        else
        {
            DisplayAlert("Chyba", "Zadejte nìjaké produkty, které od vás bude odbìratel odebírat.", "OK");
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