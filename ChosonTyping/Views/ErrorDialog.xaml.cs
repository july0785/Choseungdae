using System.Windows;

namespace ChosonTyping.Views;

public partial class ErrorDialog : Window
{
    public ErrorDialog(IEnumerable<string> errors)
    {
        InitializeComponent();
        Title = Core.Loc.S("err.title");
        TitleTb.Text = Core.Loc.S("err.title");
        SkipTb.Text = Core.Loc.S("err.skip");
        OkBtn.Content = Core.Loc.S("common.ok");
        MessageText.Text = string.Join("\n", errors);
    }

    public static void ShowErrors(Window? owner, IReadOnlyCollection<string> errors)
    {
        if (errors.Count == 0) return;
        var d = new ErrorDialog(errors);
        if (owner is not null) d.Owner = owner;
        d.ShowDialog();
    }

    void Ok_Click(object sender, RoutedEventArgs e) => Close();
}
