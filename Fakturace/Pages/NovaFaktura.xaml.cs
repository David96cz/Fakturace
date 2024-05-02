using Fakturace.Model;
using Fakturace.Data;

namespace Fakturace;

public partial class NovaFaktura : ContentPage
{
    ContextDatabaze ContextDatabaze;
    ListView VystaveneList;
    Odberatel O;
    string produkty;

    public NovaFaktura(ContextDatabaze contextDatabaze, ListView vystaveneList)
    {
        InitializeComponent();
        ContextDatabaze = contextDatabaze;
        VystaveneList = vystaveneList;
        odberateleList.ItemsSource = ContextDatabaze.Odberatele.ToList();
        if (!ContextDatabaze.Odberatele.Any())
        {
            odberatelPicker.SelectedIndex = 1;
        }
        else
        {
            odberatelPicker.SelectedIndex = 0;
        }
        VolbaOdberatele();
    }

    public async void Button_Generovat(object sender, EventArgs e)
    {
         string jmeno;
         string prijmeni;
         string zeme;
         string mesto;
         string ulice;
         string cisloPopisne;
         string psc;
         string ico;
         bool odberatel = true;
         
         if (odberatelPicker.SelectedItem.ToString() == "Jiný odbìratel")
         {
             if (produkty != null)
             {
                 jmeno = jmenoInput.Text;
                 prijmeni = prijmeniInput.Text;
                 zeme = zemeInput.Text;
                 mesto = mestoInput.Text;
                 ulice = uliceInput.Text;
                 cisloPopisne = cisloPopisneInput.Text;
                 psc = pscInput.Text;
                 ico = icoInput.Text;
             }
             else
             {
                 await DisplayAlert("Chyba", "Nebyly pøidány žádné produkty", "OK");
                 return;
             }
         
         }
         else
         {
             if (O != null)
             {
                 jmeno = O.Jmeno;
                 prijmeni = O.Prijmeni;
                 zeme = O.Zeme;
                 mesto = O.Mesto;
                 ulice = O.Ulice;
                 cisloPopisne = O.CisloPopisne;
                 psc = O.Psc;
                 if (!string.IsNullOrEmpty(produkty)) // Poøešení, aby se poslední èlen pøedposledního záznamu a první èlen posledního záznamu nespojily do jednoho...
                 {
                     produkty += "§";
                 }
                 produkty += O.Produkty;
                 ico = O.Ico;
         
             }
             else
             {
                 await DisplayAlert("Chyba", "Kliknutím vyberte ze seznamu existujícího odbìratele.", "OK");
                 return;
             }
             
         }
         
         ContextDatabaze.VystaveneFaktury.Add(new Model.Faktura(jmeno, prijmeni, zeme, mesto, ulice, cisloPopisne, psc, ico, produkty, odberatel) {Jmeno = jmeno, Prijmeni = prijmeni, Zeme = zeme, Mesto = mesto, Ulice = ulice, CisloPopisne = cisloPopisne, Psc = psc, Ico = ico, Produkty = produkty,Odberatel = odberatel });
         ContextDatabaze.SaveChanges();
         VystaveneList.ItemsSource = null;
         VystaveneList.ItemsSource = ContextDatabaze.VystaveneFaktury.ToList();
         //Vygenerovano();
         await DisplayAlert("Info", "Faktura byla úspìšnì vygenerována.", "OK");
         await Navigation.PopAsync();
         
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
                await DisplayAlert("Chyba", "Vložte prosím platnou celoèíselnou hodnotu pro ID.", "OK");
                return;
            }

            if (!int.TryParse(cenaInput.Text, out cena))
            {
                await DisplayAlert("Chyba", "Vložte prosím platnou celoèíselnou hodnotu pro cenu.", "OK");
                return;
            }

            if (!int.TryParse(pocetKsInput.Text, out pocetKs))
            {
                await DisplayAlert("Chyba", "Vložte prosím platnou celoèíselnou hodnotu pro poèet kusù.", "OK");
                return;
            }
            int cenaZaKusy = cena*pocetKs;

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

    private void Odberatele_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        Odberatel o = (sender as ListView).SelectedItem as Odberatel;
        O = o;
        vybranyOdberatel.Text = "Vybrán odbìratel è. " + O.Jmeno + " " + O.Prijmeni;
    }

    private void OdberatelPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        VolbaOdberatele();
    }

    private async void VolbaOdberatele()
    {
        if (ContextDatabaze.Odberatele.Any())
        {
            if (odberatelPicker.SelectedItem.ToString() == "Existující odbìratel")
            {
                odberateleList.IsEnabled = true;
                jmenoInput.IsEnabled = false;
                prijmeniInput.IsEnabled = false;
                uliceInput.IsEnabled = false;
                cisloPopisneInput.IsEnabled = false;
                mestoInput.IsEnabled = false;
                pscInput.IsEnabled = false;
                zemeInput.IsEnabled = false;
                icoInput.IsEnabled = false;
            }
            else
            {
                odberateleList.IsEnabled = false;
                jmenoInput.IsEnabled = true;
                prijmeniInput.IsEnabled = true;
                uliceInput.IsEnabled = true;
                cisloPopisneInput.IsEnabled = true;
                mestoInput.IsEnabled = true;
                pscInput.IsEnabled = true;
                zemeInput.IsEnabled = true;
                icoInput.IsEnabled = true;
            }
        }
        else if(odberatelPicker.SelectedItem.ToString() == "Existující odbìratel")
        {
            odberatelPicker.SelectedIndex = 1;
            await DisplayAlert("Chyba", "V databázi nejsou žádní existující odbìratelé. Nejprve nìjakého pøidejte.", "OK");
        }
            
    }
}
