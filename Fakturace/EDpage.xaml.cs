using Fakturace.Data;
using Fakturace.Model;

namespace Fakturace;

public partial class EDpage : ContentPage
{
    ContextDodavatelu DbDodavatelu;
    ContextPrijatych DbPrijatych;
    ListView PrijateList;
    Dodavatel D;
    string[] ProduktyList;
    string Produkty;

    public EDpage(ContextDodavatelu dbDodavatelu, ContextPrijatych dbPrijatych, ListView prijateList)
    {
        InitializeComponent();
        DbDodavatelu = dbDodavatelu;
        DbPrijatych = dbPrijatych;
        PrijateList = prijateList;
        dodavateleList.ItemsSource = DbDodavatelu.Dodavatele.ToList();
        vybranyDodavatel.Text = "Vybran� dodavatel: ---";
    }

    private async void Novy_Dodavatel_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NovyDodavatel(DbDodavatelu, DbPrijatych, dodavateleList, PrijateList));
    }

    public void Button_Otevrit(object sender, EventArgs e)
    {
        if (D != null)
        {
            if (D.Produkty != null)
            {
                bool zarovnani = true;

                ProduktyList = D.Produkty.Split("�");
                for (int i = 1; i < ProduktyList.Length; i += 5)
                {
                    if (zarovnani) { Produkty += $"                   {ProduktyList[i]}\n"; zarovnani = false; }
                    else { Produkty += $"                                  {ProduktyList[i]}\n"; } 
                }


            }
            DisplayAlert("Podrobnosti o dodavateli", $"ID:                              {D.Id2}\nJm�no a p��jmen�:      {D.Jmeno} {D.Prijmeni}\nUlice a �P:                 {D.Ulice} {D.CisloPopisne}\nM�sto a PS�:             {D.Mesto} {D.Psc}\nZem�:                        {D.Zeme}\nI�O:                           {D.Ico}\n\nProdukty:{Produkty}", "OK");
            Produkty = "";
        }
        else
            DisplayAlert("Chyba", "Nebyl vybr�n ��dn� dodavatel.", "OK");
    }

    private async void Button_Smazat(object sender, EventArgs e)
    {
        if (D != null)
        {
            var result = await DisplayAlert("Pozor", "Opravdu chcete odstranit tohoto dodavatele?", "Ano", "Zru�it");
            if (result)
            {
                DbDodavatelu.Dodavatele.Remove(D);
                DbDodavatelu.SaveChanges();
                vybranyDodavatel.Text = "Vybran� dodavatel: ---";

                int id = 1;
                foreach (Dodavatel o in DbDodavatelu.Dodavatele.ToList())
                {
                    D = o;
                    D.Id2 = id;
                    DbDodavatelu.SaveChanges();
                    id++;
                }

                dodavateleList.ItemsSource = null;
                dodavateleList.ItemsSource = DbDodavatelu.Dodavatele.ToList();
            }
            D = null;
        }
        else
        {
            await DisplayAlert("Chyba", "Nebyla vybr�n ��dn� dodavatel", "OK");
        }
    }

    private void dodavatelList_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        Dodavatel d = (sender as ListView).SelectedItem as Dodavatel;
        D = d;
        vybranyDodavatel.Text = "Vybran� dodavatel: " + D.Jmeno + " " + D.Prijmeni;
    }

    private async void Button_Smazat_Dodavatele(object sender, EventArgs e)
    {
        if (DbDodavatelu.Dodavatele.Any())
        {
            var rozhodnuti = await DisplayAlert("Pozor!", "Opravdu chcete smazat v�echny dodavatele z datab�ze?", "Ano", "Zru�it");

            if (rozhodnuti)
            {
                var dodavatele = DbDodavatelu.Dodavatele.ToList();
                DbDodavatelu.Dodavatele.RemoveRange(dodavatele);
                DbDodavatelu.SaveChanges();

                var prijate = DbPrijatych.Faktury.ToList();
                DbPrijatych.Faktury.RemoveRange(prijate);
                DbPrijatych.SaveChanges();
                dodavateleList.ItemsSource = null;
                dodavateleList.ItemsSource = DbDodavatelu.Dodavatele.ToList();

                await DisplayAlert("Info", "Dodavatel� a p�ijat� fakutry od nich byli �sp�n� smaz�ni.", "OK");
            }
        }
        else
            await DisplayAlert("Chyba", "V datab�zi dodavatel� se nenach�z� ��dn� z�znamy pro smaz�n�.", "OK");

    }
}