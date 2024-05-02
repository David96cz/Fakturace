using Fakturace.Data;
using Fakturace.Model;

namespace Fakturace;

public partial class EDpage : ContentPage
{
    ContextDatabaze ContextDatabaze;
    ListView PrijateList;
    Dodavatel D;
    string[] ProduktyList;
    string Produkty;

    public EDpage(ContextDatabaze contextDatabaze, ListView prijateList)
    {
        InitializeComponent();
        ContextDatabaze = contextDatabaze;
        PrijateList = prijateList;
        dodavateleList.ItemsSource = ContextDatabaze.Dodavatele.ToList();
        vybranyDodavatel.Text = "Vybraný dodavatel: ---";
    }

    private async void Novy_Dodavatel_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NovyDodavatel(ContextDatabaze, dodavateleList, PrijateList));
    }

    public void Button_Otevrit(object sender, EventArgs e)
    {
        if (D != null)
        {
            if (D.Produkty != null)
            {
                bool zarovnani = true;

                ProduktyList = D.Produkty.Split("§");
                for (int i = 1; i < ProduktyList.Length; i += 5)
                {
                    if (zarovnani) { Produkty += $"                   {ProduktyList[i]}\n"; zarovnani = false; }
                    else { Produkty += $"                                  {ProduktyList[i]}\n"; } 
                }


            }
            DisplayAlert("Podrobnosti o dodavateli", $"ID:                              {D.Id2}\nJméno a pøíjmení:      {D.Jmeno} {D.Prijmeni}\nUlice a ÈP:                 {D.Ulice} {D.CisloPopisne}\nMìsto a PSÈ:             {D.Mesto} {D.Psc}\nZemì:                        {D.Zeme}\nIÈO:                           {D.Ico}\n\nProdukty:{Produkty}", "OK");
            Produkty = "";
        }
        else
            DisplayAlert("Chyba", "Nebyl vybrán žádný dodavatel.", "OK");
    }

    private async void Button_Smazat(object sender, EventArgs e)
    {
        if (D != null)
        {
            var result = await DisplayAlert("Pozor", "Opravdu chcete odstranit tohoto dodavatele?", "Ano", "Zrušit");
            if (result)
            {
                ContextDatabaze.Dodavatele.Remove(D);
                ContextDatabaze.SaveChanges();
                vybranyDodavatel.Text = "Vybraný dodavatel: ---";

                int id = 1;
                foreach (Dodavatel o in ContextDatabaze.Dodavatele.ToList())
                {
                    D = o;
                    D.Id2 = id;
                    ContextDatabaze.SaveChanges();
                    id++;
                }

                dodavateleList.ItemsSource = null;
                dodavateleList.ItemsSource = ContextDatabaze.Dodavatele.ToList();
            }
            D = null;
        }
        else
        {
            await DisplayAlert("Chyba", "Nebyla vybrán žádný dodavatel", "OK");
        }
    }

    private void dodavatelList_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        Dodavatel d = (sender as ListView).SelectedItem as Dodavatel;
        D = d;
        vybranyDodavatel.Text = "Vybraný dodavatel: " + D.Jmeno + " " + D.Prijmeni;
    }

    private async void Button_Smazat_Dodavatele(object sender, EventArgs e)
    {
        if (ContextDatabaze.Dodavatele.Any())
        {
            var rozhodnuti = await DisplayAlert("Pozor!", "Opravdu chcete smazat všechny dodavatele z databáze?", "Ano", "Zrušit");

            if (rozhodnuti)
            {
                var dodavatele = ContextDatabaze.Dodavatele.ToList();
                ContextDatabaze.Dodavatele.RemoveRange(dodavatele);
                ContextDatabaze.SaveChanges();

                var prijate = ContextDatabaze.PrijateFaktury.ToList();
                ContextDatabaze.PrijateFaktury.RemoveRange(prijate);
                ContextDatabaze.SaveChanges();
                dodavateleList.ItemsSource = null;
                dodavateleList.ItemsSource = ContextDatabaze.Dodavatele.ToList();

                await DisplayAlert("Info", "Dodavatelé a pøijaté fakutry od nich byli úspìšnì smazáni.", "OK");
            }
        }
        else
            await DisplayAlert("Chyba", "V databázi dodavatelù se nenachází žádné záznamy pro smazání.", "OK");

    }
}