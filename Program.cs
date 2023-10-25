using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;

internal class Program // knasigt vid multiplechoice där frågan kommer längst ned
{
    static void Main(string[] args)
    {
        System.Console.WriteLine("Gittest");
        int myPoints = 0;
        string json = File.ReadAllText("Questions.json"); 
        List<QuestionModel> questionModels = JsonSerializer.Deserialize<List<QuestionModel>>(json); 

        foreach (var questionModel in questionModels)
        {
            Question question;
            if (questionModel.Type == "FreeText")
            {
                question = new FreeText(questionModel.Formulation, questionModel.Points, questionModel.CorrectAnswer);
            }
            else if (questionModel.Type == "MultipleChoices")
            { 
                question = new MultipleChoices( questionModel.Formulation, questionModel.Points,questionModel.CorrectAnswer, questionModel.Choices);
                Console.WriteLine(question.Formulation); 

                foreach (var choice in questionModel.Choices)
                {       
                Console.WriteLine(choice); // eftersom choices ligger här så blir det knas när frågan skrivs ut då den kommer under choices. 
                }
                
            }
            else if(questionModel.Type == "OneToTen")
            {
                question = new OneToTen(questionModel.Formulation, questionModel.Points, questionModel.CorrectAnswer, questionModel.OneToTenAnswer);
                System.Console.WriteLine(questionModel.OneToTenAnswer);
            }
            else
            {
                Console.WriteLine($"Okänt frågetyp: {questionModel.Type}");
                continue;
            }
            
            Console.WriteLine(question.Formulation);
            string answer = Console.ReadLine().ToLower();
            if (question.CheckAnswer(answer))
            {
                myPoints += question.Points; 
                System.Console.WriteLine($"Rätt! Du har nu {myPoints} poäng!");
                System.Console.WriteLine();
                Thread.Sleep(1500);
            }
            else
            {
                System.Console.WriteLine($"Fel! Du har {myPoints} poäng!");
                System.Console.WriteLine();
                Thread.Sleep(1500);
            }
        }   

    }
}
abstract class Question
    {
        public string Formulation{get; set;}
        public int Points {get; set;}
        
        public Question(string formulation, int points)
        {
            Formulation = formulation; 
            Points = points; 
        }
        public override string ToString()
        {
        return $"Points: {Points}. Question: {Formulation}";
        }
        public virtual bool CheckAnswer(string userAnswer)
        {
        return true;
        }
        
    }

class FreeText : Question
{
   public string CorrectAnswer{get; set;}
   public FreeText(string formulation, int points, string correctAnswer) : base(formulation, points)
   {
    CorrectAnswer = correctAnswer;
   }
   public override bool CheckAnswer(string userAnswer)
    {
        if(userAnswer == CorrectAnswer)
            return true;
        else
            return false;
    }
}
class MultipleChoices : Question
{
    public string CorrectAnswer{set;get;} 
    public  List<string> Choices{get; set;}
    public MultipleChoices(string formulation, int points, string correctAnswer, List<string> choices) : base(formulation, points)
    {
        CorrectAnswer = correctAnswer;
        Choices = choices;    
    }

    public override bool CheckAnswer(string userAnswer)
    {
        if(userAnswer == CorrectAnswer)
            return true;
        else
            return false;
    }
}
class OneToTen : Question
{
    public string CorrectAnswer{set;get;} 
    public string OneToTenAnswer{set;get;} 
    public OneToTen (string formulation, int points, string correctAnswer, string oneToTenAnswer) : base(formulation, points)
    {
        CorrectAnswer = correctAnswer;
        OneToTenAnswer = oneToTenAnswer; 
    }

    public override bool CheckAnswer(string userAnswer)
    {
        if(userAnswer == CorrectAnswer)
            return true;
        else
            return false;
    }
}

public class QuestionModel
{   
        
        public string Type { get; set; }    
        public string Formulation { get; set; } 
        public int Points { get; set; } 
        public string CorrectAnswer { get; set; }   
        public List<string> Choices { get; set; }   
        public string OneToTenAnswer { get; set; }  
}   