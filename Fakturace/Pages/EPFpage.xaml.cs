using Fakturace.Model;
using Fakturace.Data;

namespace Fakturace;

public partial class EPFpage : ContentPage
{
    ContextDatabaze ContextDatabaze;
    Faktura Faktura;

    public EPFpage(ContextDatabaze contextDatabaze)
	{
		InitializeComponent();
        ContextDatabaze = contextDatabaze;
        prijateList.ItemsSource = null;
        prijateList.ItemsSource = ContextDatabaze.PrijateFaktury.ToList();
    }

    private void PrijateList_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        Faktura f = (sender as ListView).SelectedItem as Faktura;
        Faktura = f;
        vybranaFaktura.Text = "Vybrána faktura è. " + Faktura.CisloFaktury;
    }

    private void Button_Otevrit_Fakturu(object sender, EventArgs e)
    {
        if (Faktura != null)
        {
            Faktura.OpenFile($"Faktura {Faktura.CisloFaktury}.pdf", "application/pdf");
        }
    }

    private async void Button_Smazat_Fakturu(object sender, EventArgs e)
    {
        if (Faktura != null)
        {
            var result = await DisplayAlert("Fakt?", "Opravdu chcete odstranit tuhle fakturu?", "Ano", "Zrušit");
            if (result)
            {
                ContextDatabaze.PrijateFaktury.Remove(Faktura);
                ContextDatabaze.SaveChanges();
                prijateList.ItemsSource = null;
                prijateList.ItemsSource = ContextDatabaze.PrijateFaktury.ToList();
                vybranaFaktura.Text = "Vybrána faktura è. ---";
            }
            Faktura = null;
        }
        else
        {
            await DisplayAlert("CHYBA", "Nevybrals fakturu", "OK");
        }
    }
}