
public class ReputationFormula 
{
    public static ReputationState ReputationState(int points)
    {
        return points < 20 ? global::ReputationState.Terrible :
               points >= 20 && points < 40 ? global::ReputationState.Poor :
               points >= 40 && points < 60 ? global::ReputationState.Neutral :
               points >= 60 && points < 80 ? global::ReputationState.Good : global::ReputationState.Excellent;
    }
}