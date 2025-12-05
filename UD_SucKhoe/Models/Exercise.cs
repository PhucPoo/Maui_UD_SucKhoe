namespace UD_SucKhoe.Models
{
    public class Exercise
    {
        public int ExerciseID { get; set; }
        public string ExerciseName { get; set; }
        public string Type { get; set; }
        public int DurationPerSet { get; set; }
        public int CaloriesBurned { get; set; }
        public string DifficultyLevel { get; set; }

        public string DisplayInfo => $"{Type} | {DurationPerSet} phút | {CaloriesBurned} calo | {DifficultyLevel}";
    }
}
