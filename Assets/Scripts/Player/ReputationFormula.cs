
public class ReputationFormula 
{
    public static ReputationLevel ReputationLevel(int points)
    {
        return points < 20 ? global::ReputationLevel.Terrible :
               points >= 20 && points < 40 ? global::ReputationLevel.Poor :
               points >= 40 && points < 60 ? global::ReputationLevel.Neutral :
               points >= 60 && points < 80 ? global::ReputationLevel.Good : global::ReputationLevel.Excellent;
    }
}