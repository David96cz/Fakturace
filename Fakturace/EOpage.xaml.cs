using Fakturace.Model;
using Fakturace.Data;

namespace Fakturace;

public partial class EOpage : ContentPage
{
    ContextOdberatelu DbOdberatelu;
    ContextVystavenych DbVystavenych;
    ListView VystaveneList;
    Odberatel O;
    string[] ProduktyList;
    string Produkty;

    public EOpage(ContextOdberatelu dbOdberatelu, ContextVystavenych dbVystavenych, ListView vystaveneList)
	{
		InitializeComponent();
        DbOdberatelu = dbOdberatelu;
        DbVystavenych = dbVystavenych;
        VystaveneList = vystaveneList;
        odberateleList.ItemsSource = DbOdberatelu.Odberatele.ToList();
        vybranyOdberatel.Text = "Vybran� odb�ratel: ---";
    }

    private async void Novy_Odberatel_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NovyOdberatel(DbOdberatelu, DbVystavenych, odberateleList, VystaveneList));
    }

    private void Button_Otevrit(object sender, EventArgs e)
    {
        if (O != null)
        {
            if (O.Produkty != null)
            {
                bool zarovnani = true;

                ProduktyList = O.Produkty.Split("�");
                for (int i = 1; i < ProduktyList.Length; i += 5)
                {
                    if (zarovnani) { Produkty += $"                   {ProduktyList[i]}\n"; zarovnani = false; }
                    else { Produkty += $"                                  {ProduktyList[i]}\n"; }
                }


            }
            DisplayAlert("Podrobnosti o odb�rateli", $"ID:                              {O.Id2}\nJm�no a p��jmen�:      {O.Jmeno} {O.Prijmeni}\nUlice a �P:                 {O.Ulice} {O.CisloPopisne}\nM�sto a PS�:             {O.Mesto} {O.Psc}\nZem�:                        {O.Zeme}\nI�O:                           {O.Ico}\n\nProdukty:{Produkty}", "OK");
            Produkty = "";
        }
        else
            DisplayAlert("Chyba", "Nebyl vybr�n ��dn� odb�ratel.", "OK");
    }

    private async void Button_Smazat(object sender, EventArgs e)
    {
        if (O != null)
        {
            var result = await DisplayAlert("Pozor", "Opravdu chcete odstranit tohoto odb�ratele?", "Ano", "Zru�it");
            if (result)
            {
                DbOdberatelu.Odberatele.Remove(O);
                DbOdberatelu.SaveChanges();
                vybranyOdberatel.Text = "Vybran� odb�ratel: ---";

                int id = 1;
                foreach (Odberatel o in DbOdberatelu.Odberatele.ToList())
                {
                    O = o;
                    O.Id2 = id;
                    DbOdberatelu.SaveChanges();
                    id++;
                }

                odberateleList.ItemsSource = null;
                odberateleList.ItemsSource = DbOdberatelu.Odberatele.ToList();
            }
            O = null;
        }
        else
        {
            await DisplayAlert("Chyba", "Nebyl vybr�n ��dn� odb�ratel", "OK");
        }
    }

    private void odberatelList_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        Odberatel o = (sender as ListView).SelectedItem as Odberatel;
        O = o;
        vybranyOdberatel.Text = "Vybran� odb�ratel: " + O.Jmeno + " " + O.Prijmeni;
    }

    private async void Button_Smazat_Odberatele(object sender, EventArgs e)
    {
        if (DbOdberatelu.Odberatele.Any())
        {
            var rozhodnuti = await DisplayAlert("Pozor!", "Opravdu chcete smazat v�echny odb�ratele z datab�ze?", "Ano", "Zru�it");

            if (rozhodnuti)
            {
                var odberatele = DbOdberatelu.Odberatele.ToList();
                DbOdberatelu.Odberatele.RemoveRange(odberatele);
                DbOdberatelu.SaveChanges();

                var prijate = DbVystavenych.Faktury.ToList();
                DbVystavenych.Faktury.RemoveRange(prijate);
                DbVystavenych.SaveChanges();
                odberateleList.ItemsSource = null;
                odberateleList.ItemsSource = DbOdberatelu.Odberatele.ToList();

                await DisplayAlert("Info", "Odb�ratel� a jim vystaven� faktury byli �sp�n� smaz�ni.", "OK");
            }
        }
        else
            await DisplayAlert("Chyba", "V datab�zi odb�ratel� se nenach�z� ��dn� z�znamy pro smaz�n�.", "OK");
    }
}