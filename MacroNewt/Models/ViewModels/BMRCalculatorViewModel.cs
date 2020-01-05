namespace MacroNewt.Models.ViewModels
{
    public class BMRCalculatorViewModel
    {
        public double WeightKG { get; set; }
        public double HeightCM { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public int S { get; set; }
        public double BMR { get; set; }

        public BMRCalculatorViewModel(int weight, int height, int age, string gender)
        {
            WeightKG = weight * 0.45359237;
            HeightCM = height * 2.54;
            Age = age;
            Gender = gender;

            if (gender == "Male")
            {
                S = 5;
            }
            else
            {
                S = -161;
            }

            BMR = (10 * WeightKG) + (6.25 * HeightCM) - (5 * Age) + S;
        }

    }
}
