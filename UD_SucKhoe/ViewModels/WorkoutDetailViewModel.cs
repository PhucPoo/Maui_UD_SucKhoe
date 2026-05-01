using System.ComponentModel;
using System.Runtime.CompilerServices;
using UD_SucKhoe.Models;
using UD_SucKhoe.Services.Database;

public class WorkoutDetailViewModel : INotifyPropertyChanged
{
    private readonly DatabaseService _db;

    private List<Exercise> _exercises = new();

    public List<Exercise> Exercises
    {
        get => _exercises;
        set
        {
            _exercises = value;
            OnPropertyChanged();
        }
    }

    public bool IsLoggedIn =>
        Preferences.Get("UserId", 0) != 0;

    public WorkoutDetailViewModel()
    {
        _db = new DatabaseService();
    }

    public async Task LoadExercises()
    {
        if (!IsLoggedIn)
        {
            Exercises = new List<Exercise>();
            return;
        }

        Exercises = await _db.GetExpertExercises();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}