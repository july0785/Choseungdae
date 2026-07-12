using System.Windows;

namespace ChosonTyping.Views;

public partial class InputDialog : Window
{
    public string TitleText => TitleBox.Text.Trim();
    public string SourceText => SourceBox.Text.Trim();

    public InputDialog(string defaultTitle, string defaultSource)
    {
        InitializeComponent();
        Title = Core.Loc.S("imp.title");
        TitleTb.Text = Core.Loc.S("imp.title");
        NameLabel.Text = Core.Loc.S("imp.name");
        SourceLabel.Text = Core.Loc.S("imp.source");
        CancelBtn.Content = Core.Loc.S("imp.cancel");
        SaveBtn.Content = Core.Loc.S("imp.save");
        TitleBox.Text = defaultTitle;
        SourceBox.Text = defaultSource;
        Loaded += (_, _) => TitleBox.Focus();
    }

    void Ok_Click(object sender, RoutedEventArgs e)
    {
        if (TitleText.Length == 0)
        {
            TitleBox.Focus();
            return;
        }
        DialogResult = true;
    }

    void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
