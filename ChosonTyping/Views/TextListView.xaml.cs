using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ChosonTyping.Core;

namespace ChosonTyping.Views;

/// <summary>긴글 목록: 내장 긴글과 불러온 글(.ctp)이 나란히 뜬다(설계서 10.3).</summary>
public partial class TextListView : UserControl
{
    readonly MainWindow _main;
    readonly KeyboardLayout _layout;
    readonly bool _isTest;

    public TextListView(MainWindow main, KeyboardLayout layout, bool isTest)
    {
        InitializeComponent();
        _main = main;
        _layout = layout;
        _isTest = isTest;
        BackBtn.Content = Loc.S("nav.start");
        PickLabel.Text = Loc.S("list.pick");
        ImportBtn.Content = Loc.S("list.import");
        TitleText.Text = Loc.S(_isTest ? "list.test" : "list.long");
        SubText.Text = Loc.S(_isTest ? "list.testSub" : "list.longSub");
        Loaded += (_, _) => Reload();
    }

    void Reload()
    {
        TextList.Children.Clear();
        var errors = new List<string>();

        var (builtin, e1) = ContentModule.LoadDir(Path.Combine(AppConfig.DataDir, "longtext"));
        var (imported, e2) = ImportedText.LoadAll();
        errors.AddRange(e1);
        errors.AddRange(e2);

        int i = 0;
        foreach (var (m, isBuiltin) in builtin.Select(m => (m, true))
                     .Concat(imported.Select(m => (m, false))))
        {
            TextList.Children.Add(MakeRow(m, isBuiltin, i++));
        }
        if (i == 0)
        {
            TextList.Children.Add(new TextBlock
            {
                Text = Loc.S("list.empty"),
                FontSize = 13, Foreground = (Brush)FindResource("Faint"),
                Margin = new Thickness(4, 8, 0, 8),
            });
        }
        ErrorDialog.ShowErrors(Window.GetWindow(this), errors);
    }

    Border MakeRow(ContentModule m, bool isBuiltin, int index)
    {
        var row = new DockPanel();
        row.Children.Add(new TextBlock
        {
            Text = (index + 1).ToString(), FontSize = 12, Width = 22,
            Foreground = (Brush)FindResource("Faint"), VerticalAlignment = VerticalAlignment.Center,
        });
        var tagText = new TextBlock
        {
            Text = Loc.S(isBuiltin ? "list.builtin" : "list.imported"), FontSize = 11,
            Foreground = (Brush)FindResource(isBuiltin ? "Faint" : "Sky"),
            HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Center,
        };
        DockPanel.SetDock(tagText, Dock.Right);
        row.Children.Add(tagText);
        var body = new StackPanel();
        body.Children.Add(new TextBlock
        {
            Text = m.Title, FontSize = 15, FontWeight = FontWeights.Bold,
            Foreground = (Brush)FindResource("Ink"),
        });
        if (!string.IsNullOrEmpty(m.Source))
        {
            body.Children.Add(new TextBlock
            {
                Text = m.Source, FontSize = 12, Foreground = (Brush)FindResource("Mid"),
                Margin = new Thickness(0, 2, 0, 0),
            });
        }
        row.Children.Add(body);

        var border = new Border
        {
            BorderBrush = (Brush)FindResource("Hair"),
            BorderThickness = new Thickness(0, index == 0 ? 1 : 0, 0, 1),
            Padding = new Thickness(4, 12, 4, 12),
            Background = Brushes.Transparent,
            Cursor = Cursors.Hand,
            Child = row,
        };
        border.MouseEnter += (_, _) => border.Background = (Brush)FindResource("Soft");
        border.MouseLeave += (_, _) => border.Background = Brushes.Transparent;
        border.MouseLeftButtonUp += (_, _) =>
            _main.Navigate(() => new LongTextView(_main, _layout, m, _isTest));
        return border;
    }

    void Import_Click(object sender, RoutedEventArgs e)
    {
        var dlg = new Microsoft.Win32.OpenFileDialog
        {
            Filter = Loc.S("imp.filter"),
            Title = Loc.S("imp.title"),
        };
        if (dlg.ShowDialog() != true) return;

        string body;
        try
        {
            body = File.ReadAllText(dlg.FileName);
        }
        catch (Exception ex)
        {
            ErrorDialog.ShowErrors(Window.GetWindow(this), new[] { $"《{Path.GetFileName(dlg.FileName)}》 — {ex.Message}" });
            return;
        }
        if (body.Trim().Length == 0) return;

        var input = new InputDialog(Path.GetFileNameWithoutExtension(dlg.FileName), Loc.S("imp.default"))
        {
            Owner = Window.GetWindow(this),
        };
        if (input.ShowDialog() != true) return;

        ImportedText.Save(input.TitleText, input.SourceText, body);
        Reload();
    }

    void Back_Click(object sender, RoutedEventArgs e) => _main.Navigate(() => new StartView(_main));
}
