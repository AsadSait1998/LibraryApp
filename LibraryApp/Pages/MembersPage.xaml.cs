using LibraryApp.Models;
using LibraryApp.Services;

namespace LibraryApp.Pages;

public partial class MembersPage : ContentPage
{
    private readonly DatabaseService _db;

    public MembersPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        MembersList.ItemsSource = await _db.GetMembersAsync();
    }

    private Member? SelectedMember => MembersList.SelectedItem as Member;

    private void Show(string msg)
    {
        Message.Text = msg;
        Message.IsVisible = true;
    }

    private async void OnAddMember(object sender, EventArgs e)
    {
        Message.IsVisible = false;
        try
        {
            var m = new Member
            {
                Name = NameEntry.Text?.Trim() ?? "",
                Email = EmailEntry.Text?.Trim() ?? ""
            };
            await _db.AddMemberAsync(m);
            await LoadAsync();
            NameEntry.Text = EmailEntry.Text = string.Empty;
        }
        catch (ValidationException vex) { Show(vex.Message); }
        catch (Exception ex) { Show($"Unexpected error: {ex.Message}"); }
    }

    private async void OnUpdateMember(object sender, EventArgs e)
    {
        if (SelectedMember == null) { Show("Select a member to update."); return; }
        try
        {
            SelectedMember.Name = string.IsNullOrWhiteSpace(NameEntry.Text) ? SelectedMember.Name : NameEntry.Text!.Trim();
            SelectedMember.Email = string.IsNullOrWhiteSpace(EmailEntry.Text) ? SelectedMember.Email : EmailEntry.Text!.Trim();
            await _db.UpdateMemberAsync(SelectedMember);
            await LoadAsync();
        }
        catch (ValidationException vex) { Show(vex.Message); }
        catch (Exception ex) { Show($"Unexpected error: {ex.Message}"); }
    }

    private async void OnDeleteMember(object sender, EventArgs e)
    {
        if (SelectedMember == null) { Show("Select a member to delete."); return; }
        try
        {
            await _db.DeleteMemberAsync(SelectedMember);
            await LoadAsync();
        }
        catch (Exception ex) { Show($"Unexpected error: {ex.Message}"); }
    }
}
