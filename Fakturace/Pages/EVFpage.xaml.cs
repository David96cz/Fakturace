using Fakturace.Model;
using Fakturace.Data;

namespace Fakturace;

public partial class EVFpage : ContentPage
{
    ContextDatabaze ContextDatabaze;
    Faktura F;

    public EVFpage(ContextDatabaze contextDatabaze)
	{
        InitializeComponent();
        ContextDatabaze = contextDatabaze;
        vystaveneList.ItemsSource = ContextDatabaze.VystaveneFaktury.ToList();
    }

    public async void Button_Nova_Faktura(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NovaFaktura(ContextDatabaze, vystaveneList));
    }

    public void VystaveneList_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        Faktura f = (sender as ListView).SelectedItem as Faktura;
        F = f;
        vybranaFaktura.Text = "Vybrána faktura è. " + F.CisloFaktury;
    }

    public void Button_Otevrit_Fakturu(object sender, EventArgs e)
    {
        if (F != null)
        {
            F.OpenFile($"Faktura {F.CisloFaktury}.pdf", "application/pdf");
        }
        else
            DisplayAlert("Chyba", "Nebyla vybrána žádná faktura.", "OK");

    }
    private async void Button_Smazat_Fakturu(object sender, EventArgs e)
    {
        if (F != null)
        {
            var result = await DisplayAlert("Pozor", "Opravdu chcete odstranit tuhle fakturu?", "Ano", "Zrušit");
            if (result)
            {
                ContextDatabaze.VystaveneFaktury.Remove(F);
                ContextDatabaze.SaveChanges();
                vystaveneList.ItemsSource = null;
                vystaveneList.ItemsSource = ContextDatabaze.VystaveneFaktury.ToList();
                vybranaFaktura.Text = "Vybrána faktura è. ---";
            }
            F = null;
        }
        else
        {
            await DisplayAlert("Chyba", "Nebyla vybrána žádná faktura", "OK");
        }
    }
}